using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Van.Helper;
using Van.Helper.HelperClasses;
using static Van.Helper.StaticInfo.Enums;
using System.Collections.ObjectModel;
using System;
using Van.AbstractClasses;
using Dragablz;
using MaterialDesignThemes.Wpf;
using System.Threading;
using Van.Helper.StaticInfo;
using Van.DataBase;
using Van.LocalDataBase.Models;
using System.Data.SQLite;
using Van.LocalDataBase;
using Dapper;

namespace Van.Windows.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public MainWindowViewModel()
        {
            isMessagePanelContent = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(1200)); 

            Modules = StaticReflectionHelper.GetAllInstancesOf<ModuleBase>().Where(x=>x.IsActive).ToList();

            var leftMenu = Modules.Where(x => x.modelClass == ModelBaseClasses.LeftMenu).OrderBy(x=>x.Num);
            LeftMenuNodes = GetTreeViewItems(leftMenu);

            var rightMenu = Modules.Where(x => x.modelClass == ModelBaseClasses.RightMenu).OrderBy(x => x.Num);
            RightMenuNodes = GetTreeViewItems(rightMenu);
            
            SetViewModels(); 

            var themes = StaticReflectionHelper.GetAllInstancesOf<ThemeBase>().ToList();

            Themes = themes.Where(x => x.ThemeClass == ThemeBaseClasses.GeneralTheme).OrderBy(m => m.Num).ToList();
            SelectedTheme = this.Themes.FirstOrDefault();
             
            DarkLightThemes = themes.Where(x => x.ThemeClass == ThemeBaseClasses.GlobalTheme).OrderBy(m => m.Num).ToList();
            SelectedThemeDarkOrLight = this.DarkLightThemes.FirstOrDefault();

            //загрузка тем из локальной БД
            GetThemes();

            //подключение к внешней БД (загрузка connectionString из локальной БД)
            DatabaseOperation.ConnectionString();
        }

        #region Fields

        public List<ThemeBase> Themes { get; private set; }

        public List<ThemeBase> DarkLightThemes { get; private set; }

        public ObservableCollection<TabControlData> ViewModels { get; set; } = new ObservableCollection<TabControlData>();

        private IList<ModuleBase> Modules { get; set; }

        #region Выбранный Таб Контрол

        private TabControlData _selectedViewModel;

        public TabControlData SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                if (_selectedViewModel == value) return;
                _selectedViewModel = value;

                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedViewModel)));
            }
        }

        #endregion

        #region Дерево для левого меню

        private ObservableCollection<Node> leftNodeData = new ObservableCollection<Node>();
        public ObservableCollection<Node> LeftMenuNodes
        {
            get { return leftNodeData; }
            set
            {
                leftNodeData = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(LeftMenuNodes)));
            }
        }

        #endregion

        #region Дерево для правого меню

        private ObservableCollection<Node> rightNodeData = new ObservableCollection<Node>();
        public ObservableCollection<Node> RightMenuNodes
        {
            get { return rightNodeData; }
            set
            {
                rightNodeData = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(RightMenuNodes)));
            }
        }

        #endregion

        #region Видимость прогресс бара бесконечного

        public bool isLoadingPanelVisible = false;

        public bool IsLoadingPanelVisible
        {
            get
            {
                return isLoadingPanelVisible;
            }
            set
            {
                isLoadingPanelVisible = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsLoadingPanelVisible)));
            }
        }

        #endregion

        #region Сообщение в SnackBar

        public SnackbarMessageQueue isMessagePanelContent;

        public SnackbarMessageQueue IsMessagePanelContent
        {
            get
            {
                return isMessagePanelContent;
            }
            set
            {
                isMessagePanelContent.IgnoreDuplicate = true;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsMessagePanelContent)));
            }
        }

        #endregion

        #region Обычная тема

        private ThemeBase _SelectedTheme;
        public ThemeBase SelectedTheme
        {
            get { return _SelectedTheme; }
            set
            {
                if (value == _SelectedTheme)
                    return;
                if (_SelectedTheme != null)
                    _SelectedTheme.Deactivate();

                _SelectedTheme = value;
                _SelectedTheme.SelectTheme();
            }
        }

        #endregion

        #region Глобальная тема

        private ThemeBase _SelectedThemeDarkOrLight;
        public ThemeBase SelectedThemeDarkOrLight
        {
            get { return _SelectedThemeDarkOrLight; }
            set
            {
                if (value == _SelectedThemeDarkOrLight)
                    return;
                if (_SelectedThemeDarkOrLight != null)
                    _SelectedThemeDarkOrLight.Deactivate();

                _SelectedThemeDarkOrLight = value;
                _SelectedThemeDarkOrLight.SelectTheme();
            }
        }

        #endregion

        #endregion

        #region Methods

        public void GetThemes()
        {
            var themes = StaticReflectionHelper.GetAllInstancesOf<ThemeBase>().ToList();
            Settings selectedThemeData;
            Settings selectedThemeDarkOrLightData;
            using (var slc = new SQLiteConnection(SQLExecutor.LoadConnectionString))
            {
                slc.Open();
                //обычная тема
                selectedThemeData = slc.Query<Settings>($"SELECT * FROM {nameof(Settings)} Where Name = '{InfoKeys.SelectedThemeKey}'").FirstOrDefault();
            }

            if (selectedThemeData != null && !string.IsNullOrEmpty(selectedThemeData.Value))
            {
                SelectedTheme = themes.Where(x => x.ThemeClass == ThemeBaseClasses.GeneralTheme && x.Name == selectedThemeData.Value).FirstOrDefault();
            }
            else
            {
                SelectedTheme = themes.Where(x => x.ThemeClass == ThemeBaseClasses.GeneralTheme).FirstOrDefault();
                selectedThemeData = new Settings() { Name = InfoKeys.SelectedThemeKey, Value = SelectedTheme.Name };
                SQLExecutor.InsertExecutor(selectedThemeData, selectedThemeData);
            }

            using (var slc = new SQLiteConnection(SQLExecutor.LoadConnectionString))
            {
                slc.Open();
                //глобальная тема
                selectedThemeDarkOrLightData = slc.Query<Settings>($"SELECT * FROM {nameof(Settings)} Where Name = '{InfoKeys.SelectedThemeDarkOrLightKey}'").FirstOrDefault();
            }

            if (selectedThemeDarkOrLightData != null && !string.IsNullOrEmpty(selectedThemeDarkOrLightData.Value))
            {
                SelectedThemeDarkOrLight = themes.Where(x => x.ThemeClass == ThemeBaseClasses.GlobalTheme && x.Name == selectedThemeDarkOrLightData.Value).FirstOrDefault();
            }
            else
            {
                SelectedThemeDarkOrLight = themes.Where(x => x.ThemeClass == ThemeBaseClasses.GlobalTheme).FirstOrDefault();
                selectedThemeDarkOrLightData = new Settings() { Name = InfoKeys.SelectedThemeDarkOrLightKey, Value = SelectedThemeDarkOrLight.Name };
                SQLExecutor.InsertExecutor(selectedThemeDarkOrLightData, selectedThemeDarkOrLightData);
            }
        }

        public void SetViewModels()
        {
            var mainmenu = Modules.Where(x => x.ID == Types.ViewData.MainMenuView).FirstOrDefault();

            mainmenu = mainmenu ?? Modules.FirstOrDefault();

            var tabControlViewModel = new TabControlData { Name = mainmenu.Name, ViewContent = mainmenu.UserInterface, ID = mainmenu.ID, ModuleBaseItem = mainmenu };

            ViewModels.Add(tabControlViewModel);

            SelectedViewModel = tabControlViewModel;
        }

        /// <summary>
        /// Выбор всех элементов в виде дерева
        /// </summary>
        public ObservableCollection<Node> GetTreeViewItems(IEnumerable<ModuleBase> items) {

            var nodes = new ObservableCollection<Node>();

            foreach (var module in items) {
                if (module.ParentID == null)
                {
                    var node = new Node();
                    node.ID = module.ID;
                    node.Name = module.Name;
                    node.ParentName = string.Empty; 
                    node.View = module;
                    node.Nodes = GetNodes(module.ID);

                    nodes.Add(node);
                }
            }

            return nodes;
        }

        /// <summary>
        /// Добавляет корни в дерево
        /// </summary>
        public ObservableCollection<Node> GetNodes(Guid moduleID) {
            var nodes = new ObservableCollection<Node>();

            foreach (var module in Modules) {
                if (module.ParentID != null && module.ParentID == moduleID)
                {
                    var node = new Node();
                    node.ID = module.ID;
                    node.Name = module.Name;
                    node.ParentName = Modules.Where(x=>x.ID == moduleID).FirstOrDefault().Name; 
                    node.View = module;
                    node.Nodes = GetNodes(module.ID);

                    nodes.Add(node);
                }
            }

            return nodes;
        }

        #endregion

        #region Команда для вызова настроек

        private RelayCommand setSettingsView;
        public RelayCommand SetSettingsView
        {
            get
            {
                return setSettingsView ??
                    (setSettingsView = new RelayCommand(obj =>
                    {
                        Thread thread = new Thread(new ThreadStart(() =>
                        {
                            Application.Current.Dispatcher.BeginInvoke(
                               System.Windows.Threading.DispatcherPriority.Normal, (Action)delegate
                               {
                                   SetSettings();
                               }); 
                        }));
                        thread.Start(); 
                    }));
            }
        }

        private void SetSettings()
        {
            var settings = Modules.Where(x => x.modelClass == Enums.ModelBaseClasses.Settings).FirstOrDefault();
            AddItemInTabControl(settings.Name, settings.UserInterface, settings.ID, settings);
        }

        #endregion

        #region Команда для добавления в ТабКонтрол

        private RelayCommand setSelectedTreeViewItem;
        public RelayCommand SetSelectedTreeViewItem
        {
            get
            {
                return setSelectedTreeViewItem ??
                    (setSelectedTreeViewItem = new RelayCommand(obj =>
                    {
                        var Node = (Node)obj;
                        if (Node != null)
                        {
                            Thread thread = new Thread(new ThreadStart(() =>
                            {
                                Application.Current.Dispatcher.BeginInvoke(
                                   System.Windows.Threading.DispatcherPriority.Normal, (Action)delegate
                                   {
                                       AddItemInTabControl(Node.Name, Node.View.UserInterface, Node.ID, Node.View);
                                       Node.Selected = false;
                                   });
                            }));
                            thread.Start();
                        }
                    }));
            }
        }

        private void AddItemInTabControl(string name, UserControl userInterface, Guid id, ModuleBase moduleBase)
        {
            var existingViewModel = ViewModels.Where(x => x.ID == id).FirstOrDefault();
            int? index = null;
            if (existingViewModel != null)
            {
                index = ViewModels.IndexOf(existingViewModel);
            }

            if (index == null)
            {
                var mainmenu = ViewModels.Count() == 1 ? ViewModels.Where(x => x.ID == Types.ViewData.MainMenuView).FirstOrDefault() : null;
                
                var TabControlViewModel = new TabControlData() { Name = name, ViewContent = userInterface, ID = id, ModuleBaseItem = moduleBase };
                ViewModels.Add(TabControlViewModel);
                SelectedViewModel = TabControlViewModel;

                if (mainmenu != null) {
                    ViewModels.Remove(mainmenu);
                }
            }
            else
            { 
                SelectedViewModel = existingViewModel; 
            }
        } 

        #endregion

        #region Команда для переопределения удаления вкладки

        public ItemActionCallback ClosingTabItemHandler
        {
            get { return ClosingTabItemHandlerImpl; }
        }
        
        private void ClosingTabItemHandlerImpl(ItemActionCallbackArgs<TabablzControl> args)
        {
            var viewModel = args.DragablzItem.DataContext as TabControlData;
            if (ViewModels.Count() == 1) {
                SetViewModels(); 
            }
            viewModel.ModuleBaseItem.Deactivate();
            ViewModels.Remove(viewModel); 
        }

        #endregion

    }
}

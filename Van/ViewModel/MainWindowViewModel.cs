using Van.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Van.Helper;
using static Van.Helper.Enums;
using System.Collections.ObjectModel;
using System;
using Van.AbstractClasses;
using Dragablz;
using System.Messaging;
using MaterialDesignThemes.Wpf;
using ITheme = Van.Interfaces.ITheme;
using System.Windows.Input;
using System.Threading.Tasks;

namespace Van.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {

        #region Fields

        public event PropertyChangedEventHandler PropertyChanged = delegate { }; 

        public List<ITheme> Themes { get; private set; }

        public List<ITheme> DarkLightThemes { get; private set; } 

        public ObservableCollection<TabControlViewModel> ViewModels { get; set; } = new ObservableCollection<TabControlViewModel>();

        private IEnumerable<ModuleBase> modules { get; set; }

        #endregion

        public MainWindowViewModel()
        {

            isMessagePanelContent = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(1200)); 

            modules = StaticReflectionHelper.GetAllInstancesOf<ModuleBase>().Where(x=>x.IsActive);

            var leftMenu = modules.Where(x => x.modelClass == ModelBaseClasses.LeftMenu).OrderBy(x=>x.Num);
            LeftMenuNodes = GetTreeViewItems(leftMenu);

            var rightMenu = modules.Where(x => x.modelClass == ModelBaseClasses.RightMenu).OrderBy(x => x.Num);
            RightMenuNodes = GetTreeViewItems(rightMenu);
             
            SetViewModels(); 

            var themes = StaticReflectionHelper.GetAllInstancesOf<ITheme>().ToList();

            Themes = themes.Where(x => x.ThemeClass == ThemeBaseClasses.GeneralTheme).OrderBy(m => m.Num).ToList();
            SelectedTheme = this.Themes.FirstOrDefault();
             
            DarkLightThemes = themes.Where(x => x.ThemeClass == ThemeBaseClasses.GlobalTheme).OrderBy(m => m.Num).ToList();
            SelectedThemeDarkOrLight = this.DarkLightThemes.FirstOrDefault(); 

        }

        public void SetViewModels()
        {
            var mainmenu = modules.Where(x => x.ID == Types.ViewData.MainMenuView).FirstOrDefault();

            mainmenu = mainmenu ?? modules.FirstOrDefault();

            var tabControlViewModel = new TabControlViewModel { Name = mainmenu.Name, ViewContent = mainmenu.UserInterface, ID = mainmenu.ID, ModuleBaseItem = mainmenu };
             
            ViewModels.Add(tabControlViewModel); 

            SelectedViewModel = tabControlViewModel;
        }

        #region Выбранный таблКонтрол

        private TabControlViewModel _selectedViewModel;

        public TabControlViewModel SelectedViewModel
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

        #region Деревья в левом и правом меню

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

        public ObservableCollection<Node> GetNodes(Guid moduleID) {
            var nodes = new ObservableCollection<Node>();

            foreach (var module in modules) {
                if (module.ParentID != null && module.ParentID == moduleID)
                {
                    var node = new Node();
                    node.ID = module.ID;
                    node.Name = module.Name;
                    node.ParentName = modules.Where(x=>x.ID == moduleID).FirstOrDefault().Name; 
                    node.View = module;
                    node.Nodes = GetNodes(module.ID);

                    nodes.Add(node);
                }
            }

            return nodes;
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

        #region Выбор обычной темы

        private ITheme _SelectedTheme;
        public ITheme SelectedTheme
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

        #region Выбор глобальной темы

        private ITheme _SelectedThemeDarkOrLight;
        public ITheme SelectedThemeDarkOrLight
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

        #region Вызов настроек

        private RelayCommand setSettingsView;
        public RelayCommand SetSettingsView
        {
            get
            {
                return setSettingsView ??
                    (setSettingsView = new RelayCommand(obj =>
                    {
                        SetSettings();
                    }));
            }
        }

        private void SetSettings()
        {
            var settings = modules.Where(x => x.modelClass == Enums.ModelBaseClasses.Settings).FirstOrDefault();
            AddItemInTabControl(settings.Name, settings.UserInterface, settings.ID, settings);
        }

        #endregion

        #region Добавление в ТабКонтрол

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
                            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                AddItemInTabControl(Node.Name, Node.View.UserInterface, Node.ID, Node.View);
                                Node.Selected = false;
                            }));  
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
                
                var TabControlViewModel = new TabControlViewModel() { Name = name, ViewContent = userInterface, ID = id, ModuleBaseItem = moduleBase };
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

        #region Удаление вкладки

        public ItemActionCallback ClosingTabItemHandler
        {
            get { return ClosingTabItemHandlerImpl; }
        }
        
        private void ClosingTabItemHandlerImpl(ItemActionCallbackArgs<TabablzControl> args)
        {
            var viewModel = args.DragablzItem.DataContext as TabControlViewModel;
            if (ViewModels.Count() == 1) {
                SetViewModels(); 
            }
            viewModel.ModuleBaseItem.Deactivate();
            ViewModels.Remove(viewModel); 
        }

        #endregion

    }
}

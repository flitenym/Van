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

namespace Van.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {

        #region Fields

        public event PropertyChangedEventHandler PropertyChanged = delegate { }; 

        public List<ITheme> Themes { get; private set; }

        public List<ITheme> DarkLightThemes { get; private set; }

        public IMainWindowView MainWindowView { get; set; }

        private TabControlViewModel _selectedViewModel;

        public ObservableCollection<TabControlViewModel> ViewModels { get; } = new ObservableCollection<TabControlViewModel>();

        #endregion

        public MainWindowViewModel()
        {
            isMessagePanelContent = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(1200)); 

            var modules = StaticReflectionHelper.CreateAllInstancesOf<IModule>();

            var leftMenu = modules.Where(x => x.modelClass == ModelBaseClasses.LeftMenu).OrderBy(x=>x.Num);
            LeftMenuNodes = GetTreeViewItems(leftMenu);

            var rightMenu = modules.Where(x => x.modelClass == ModelBaseClasses.RightMenu).OrderBy(x => x.Num);
            RightMenuNodes = GetTreeViewItems(rightMenu);


            var moduleViews = StaticReflectionHelper.CreateAllInstancesOf<ModuleBase>();
            SetViewModels(moduleViews);


            var themes = StaticReflectionHelper.CreateAllInstancesOf<ITheme>().ToList();

            Themes = themes.Where(x => x.ThemeClass == ThemeBaseClasses.GeneralTheme).OrderBy(m => m.Num).ToList();
            SelectedTheme = this.Themes.FirstOrDefault();
             
            DarkLightThemes = themes.Where(x => x.ThemeClass == ThemeBaseClasses.GlobalTheme).OrderBy(m => m.Num).ToList();
            SelectedThemeDarkOrLight = this.DarkLightThemes.FirstOrDefault(); 
        }

        public void SetViewModels(IEnumerable<ModuleBase> modules)
        { 
            ViewModels.Add(new TabControlViewModel { Name = modules.FirstOrDefault().Name, ViewContent = modules.FirstOrDefault().UserInterface, ID = modules.FirstOrDefault().ID }); 

            _selectedViewModel = ViewModels.FirstOrDefault();
        }

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

        public ObservableCollection<Node> GetTreeViewItems(IEnumerable<IModule> modules) {

            var nodes = new ObservableCollection<Node>();

            foreach (var module in modules) {
                if (module.ParentID == null)
                {
                    var node = new Node();
                    node.ID = module.ID;
                    node.Name = module.Name;
                    node.ParentName = string.Empty;
                    node.View = module;
                    node.Nodes = GetNodes(modules, module.ID);

                    nodes.Add(node);
                }
            }

            return nodes;
        }

        public ObservableCollection<Node> GetNodes(IEnumerable<IModule> modules, Guid moduleID) {
            var nodes = new ObservableCollection<Node>();

            foreach (var module in modules) {
                if (module.ParentID != null && module.ParentID == moduleID)
                {
                    var node = new Node();
                    node.ID = module.ID;
                    node.Name = module.Name;
                    node.ParentName = modules.Where(x=>x.ID == moduleID).FirstOrDefault().Name;
                    node.View = module;
                    node.Nodes = GetNodes(modules, module.ID);

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
                _SelectedTheme = null;
                if (value == _SelectedTheme) return;
                if (_SelectedTheme != null) _SelectedTheme.Deactivate();
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
                _SelectedThemeDarkOrLight = null;
                if (value == _SelectedThemeDarkOrLight) return;
                if (_SelectedThemeDarkOrLight != null) _SelectedThemeDarkOrLight.Deactivate();
                _SelectedThemeDarkOrLight = value;
                _SelectedThemeDarkOrLight.SelectTheme();
            }
        }

        #endregion

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
            var modules = StaticReflectionHelper.CreateAllInstancesOf<IModule>().ToList();
            var settings = modules.Where(x => x.modelClass == Enums.ModelBaseClasses.Settings).FirstOrDefault();
            AddItemInTabControl(settings);
        }

        #region Добавление в ТабКонтрол

        private RelayCommand setSelectedTreeViewItem;
        public RelayCommand SetSelectedTreeViewItem
        {
            get
            {
                return setSelectedTreeViewItem ??
                    (setSelectedTreeViewItem = new RelayCommand(obj =>
                    {
                        AddItemInTabControl((Node)obj); 
                    }));
            }
        }

        private void AddItemInTabControl(Node node)
        {
            var existingViewModel = ViewModels.Where(x => x.ID == node.ID).FirstOrDefault();
            int? index = null;
            if (existingViewModel != null)
            {
                index = ViewModels.IndexOf(existingViewModel);
            }

            if (index == null)
            {
                ViewModels.Add(new TabControlViewModel { Name = node.View.Name, ViewContent = node.View.UserInterface, ID = node.ID });
            }
            else
            {
                Helper.Helper.Message($"Уже используется, он находится на {index + 1} месте");
            }
        }

        private void AddItemInTabControl(IModule module)
        {
            var existingViewModel = ViewModels.Where(x => x.ID == module.ID).FirstOrDefault();
            int? index = null;
            if (existingViewModel != null)
            {
                index = ViewModels.IndexOf(existingViewModel);
            }

            if (index == null)
            {
                ViewModels.Add(new TabControlViewModel { Name = module.Name, ViewContent = module.UserInterface, ID = module.ID });
            }
            else
            {
                Helper.Helper.Message($"Уже используется, он находится на {index + 1} месте");
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
            if (ViewModels.Count() == 1) { args.Cancel(); return; }

            ViewModels.Remove(viewModel); 
        }

        #endregion

    }
}

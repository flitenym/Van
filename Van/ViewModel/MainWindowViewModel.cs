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

        #endregion

        public MainWindowViewModel()
        { 
            var modules = StaticReflectionHelper.CreateAllInstancesOf<IModule>();

            Nodes = GetTreeViewItems(modules);


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
            _viewModels.Add(new TabControlViewModel { Name = modules.FirstOrDefault().Name, ViewContent = modules.FirstOrDefault().UserInterface, ID = modules.FirstOrDefault().ID }); 

            _selectedViewModel = _viewModels.FirstOrDefault();
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

        private readonly ObservableCollection<TabControlViewModel> _viewModels = new ObservableCollection<TabControlViewModel>();

        public ObservableCollection<TabControlViewModel> ViewModels => _viewModels;

        #region Дерево в левом меню

        private ObservableCollection<Node> nodeData = new ObservableCollection<Node>();
        public ObservableCollection<Node> Nodes
        {
            get { return nodeData; }
            set
            {
                nodeData = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Nodes)));
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

        public void SnackBar()
        {
            MainWindowViewModel win = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            win.MainWindowView = (IMainWindowView)Application.Current.MainWindow;
            if (MainWindowView == null) return;

            MainWindowView.SnackBar();
        }

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

        public string isMessagePanelContent = string.Empty;

        public string IsMessagePanelContent
        {
            get
            {
                return isMessagePanelContent;
            }
            set
            {
                isMessagePanelContent = value;
                SnackBar();
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

        //private RelayCommand setSettingsView;
        //public RelayCommand SetSettingsView
        //{
        //    get
        //    {
        //        return setSettingsView ??
        //            (setSettingsView = new RelayCommand(obj =>
        //            {
        //                SetSettings();
        //            }));
        //    }
        //}

        //private void SetSettings()
        //{ 
        //    var modules = StaticReflectionHelper.CreateAllInstancesOf<IModule>().ToList();
        //    var settings = modules.Where(x => x.modelClass == Enums.ModelBaseClasses.Settings).FirstOrDefault(); 
        //}

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
            var existingViewModel = _viewModels.Where(x => x.ID == node.ID).FirstOrDefault();
            int? index = null;
            if (existingViewModel != null)
            {
                index = _viewModels.IndexOf(existingViewModel);
            }

            if (index == null)
            {
                _viewModels.Add(new TabControlViewModel { Name = node.View.Name, ViewContent = node.View.UserInterface, ID = node.ID });
            }
            else
            {
                Helper.Helper.Message($"Уже используется, он находится на {index + 1} месте");
            }
        }

        public ItemActionCallback ClosingTabItemHandler
        {
            get { return ClosingTabItemHandlerImpl; }
        }
        
        private void ClosingTabItemHandlerImpl(ItemActionCallbackArgs<TabablzControl> args)
        {
            var viewModel = args.DragablzItem.DataContext as TabControlViewModel;
            if (_viewModels.Count() == 1) { args.Cancel(); return; }

            _viewModels.Remove(viewModel); 
        }


    }
}

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

namespace Van.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {

        #region Fields

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public event PropertyChangedEventHandler ThemeChanged = delegate { };

        public List<ITheme> Themes { get; private set; }

        public List<ITheme> DarkLightThemes { get; private set; }

        public IMainWindowView MainWindowView { get; set; }

        #endregion

        public MainWindowViewModel()
        { 
            var modules = StaticReflectionHelper.CreateAllInstancesOf<IModule>();

            Nodes = GetTreeViewItems(modules);

            var themes = StaticReflectionHelper.CreateAllInstancesOf<ITheme>().ToList();


            Themes = themes.Where(x => x.ThemeClass == ThemeBaseClasses.GeneralTheme).OrderBy(m => m.Num).ToList();
            SelectedTheme = this.Themes.FirstOrDefault();
             
            DarkLightThemes = themes.Where(x => x.ThemeClass == ThemeBaseClasses.GlobalTheme).OrderBy(m => m.Num).ToList();
            SelectedThemeDarkOrLight = this.DarkLightThemes.FirstOrDefault();
        }

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
                    node.Name = module.Name;
                    node.ParentName = modules.Where(x=>x.ID == moduleID).FirstOrDefault().Name;
                    node.View = module;
                    node.Nodes = GetNodes(modules, module.ID);

                    nodes.Add(node);
                }
            }

            return nodes;
        }


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
        }

    }
}

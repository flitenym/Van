using Van.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Van.Helper;
using static Van.Helper.Enums;

namespace Van.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public event PropertyChangedEventHandler ThemeChanged = delegate { };

        public MainWindowViewModel()
        {
            List<IModule> modules = new List<IModule>(); //лист где все страницы

            modules = StaticReflectionHelper.CreateAllInstancesOf<IModule>().ToList();

            //this.SelectedModule = modules.Where(x => x.modelClass == ModelBaseClasses.Main).FirstOrDefault();

            List<ITheme> themes = new List<ITheme>(); //лист где все темы

            themes = StaticReflectionHelper.CreateAllInstancesOf<ITheme>().ToList();


            Themes = themes.Where(x => x.ThemeClass == ThemeBaseClasses.GeneralTheme).OrderBy(m => m.Num).ToList();
            SelectedTheme = this.Themes.FirstOrDefault();
             
            DarkLightThemes = themes.Where(x => x.ThemeClass == ThemeBaseClasses.GlobalTheme).OrderBy(m => m.Num).ToList();
            SelectedThemeDarkOrLight = this.DarkLightThemes.FirstOrDefault();
        } 
        public List<ITheme> Themes { get; private set; } //лист где все темы без темного и светлого
        public List<ITheme> DarkLightThemes { get; private set; }  

        public IMainWindowView MainWindowView { get; set; }

        public void SnackBar()
        {
            MainWindowViewModel win = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            win.MainWindowView = (IMainWindowView)Application.Current.MainWindow;
            if (MainWindowView == null) return;

            MainWindowView.SnackBar();
        }

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

        private IModule _SelectedModule;

        public IModule SelectedModule
        {
            get { return _SelectedModule; }
            set
            {
                _SelectedModule = null;
                if (value == _SelectedModule) return;
                if (_SelectedModule != null) _SelectedModule.Deactivate();
                _SelectedModule = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(_SelectedModule)));
                PropertyChanged(this, new PropertyChangedEventArgs("UserInterface"));
            }
        }

        public UserControl UserInterface
        {
            get
            {
                if (SelectedModule == null) return null;
                return SelectedModule.UserInterface;
            }
        }

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
            this.SelectedModule = settings;
        }

    }
}

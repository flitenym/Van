using Van.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Van.Helper;
using System.Runtime.CompilerServices;
using static Van.Helper.Helper;
using System;

namespace Van.ViewModel
{
    class SettingsViewModel : INotifyPropertyChanged
    { 
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }


        private RelayCommand themeAcceptor;
        public RelayCommand ThemeAcceptor
        {
            get
            {
                return themeAcceptor ??
                  (themeAcceptor = new RelayCommand(obj =>
                  {
                      if (obj != null && obj is string ThemeName)
                      {
                          AcceptTheme(ThemeName);
                      }
                  }));
            }
        }

        public void AcceptTheme(string ThemeName) {
            MainWindowViewModel win = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            var themes = StaticReflectionHelper.CreateAllInstancesOf<ITheme>().ToList();
            var selectedTheme = themes.Where(x => x.Name == ThemeName).FirstOrDefault();

            if (win.SelectedTheme.UriPath != selectedTheme.UriPath)
            {
                win.SelectedTheme = selectedTheme;
                Message("Тема изменена");
            }
            else {
                Message("Тема уже применена");
            }
        }

        private RelayCommand globalThemeAcceptor;
        public RelayCommand GlobalThemeAcceptor
        {
            get
            {
                return globalThemeAcceptor ??
                  (globalThemeAcceptor = new RelayCommand(obj =>
                  {
                      if (obj != null && obj is string ThemeName)
                      {
                          AcceptGlobalTheme(ThemeName);
                      }
                  }));
            }
        }

        public void AcceptGlobalTheme(string ThemeName)
        {
            MainWindowViewModel win = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            var themes = StaticReflectionHelper.CreateAllInstancesOf<ITheme>().ToList();
            var selectedTheme = themes.Where(x => x.Name == ThemeName).FirstOrDefault();

            if (win.SelectedThemeDarkOrLight.UriPath != selectedTheme.UriPath)
            {
                win.SelectedThemeDarkOrLight = selectedTheme;
                Message("Тема изменена");
            }
            else
            {
                Message("Тема уже применена");
            }
        }



    }
}

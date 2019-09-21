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

namespace Van.ViewModel
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        public SettingsViewModel()
        {
            
        }

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
                      AcceptTheme();
                  }));
            }
        }

        public void AcceptTheme() {
            Message("Тема изменена");
        }
    }
}

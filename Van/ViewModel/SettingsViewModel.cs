using Van.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Van.ViewModel
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
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
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(_SelectedTheme)));
            }
        }
    }
}

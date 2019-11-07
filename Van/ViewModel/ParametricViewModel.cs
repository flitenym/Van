using System.ComponentModel;

namespace Van.ViewModel
{
    class ParametricViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}

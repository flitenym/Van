using System.ComponentModel;

namespace Van.ViewModel.Methods
{
    class ParametricViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}

using System.ComponentModel;

namespace Van.ViewModel.Methods
{
    class NonparametricViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}

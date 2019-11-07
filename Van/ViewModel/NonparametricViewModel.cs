using System.ComponentModel;

namespace Van.ViewModel
{
    class NonparametricViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}

using System.ComponentModel;

namespace Van.ViewModel
{
    class ExcludingMortalityViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}

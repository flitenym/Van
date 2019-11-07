using System.ComponentModel;

namespace Van.ViewModel
{
    class SemiParametricViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}

using System.ComponentModel;

namespace Van.Core.ViewModel
{
    class SemiParametricViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}

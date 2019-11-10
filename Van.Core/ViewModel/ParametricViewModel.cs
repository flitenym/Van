using System.ComponentModel;

namespace Van.Core.ViewModel
{
    class ParametricViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}

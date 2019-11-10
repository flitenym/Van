using System.ComponentModel;

namespace Van.Core.ViewModel
{
    class NonparametricViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}

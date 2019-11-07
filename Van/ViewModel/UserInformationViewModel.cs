using System.ComponentModel;

namespace Van.ViewModel
{
    class UserInformationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}

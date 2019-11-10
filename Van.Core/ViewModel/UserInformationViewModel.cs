using System.ComponentModel;

namespace Van.Core.ViewModel
{
    class UserInformationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}

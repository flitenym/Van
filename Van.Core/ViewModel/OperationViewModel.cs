using System.ComponentModel;
using Van.Core.Helper;
using System.Collections.ObjectModel;

namespace Van.Core.ViewModel
{
    class OperationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        

        private ObservableCollection<OperationData> operationsData = new ObservableCollection<OperationData>() { new OperationData("название", "текст") };

        public ObservableCollection<OperationData> OperationsData
        {
            get { return operationsData; }
            set
            {
                operationsData = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(OperationsData)));
            }
        } 

    }
}

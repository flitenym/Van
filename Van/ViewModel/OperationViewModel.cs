using Van.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Van.Helper;
using System.Runtime.CompilerServices;
using static Van.Helper.Helper;
using System;
using System.Collections.ObjectModel;

namespace Van.ViewModel
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

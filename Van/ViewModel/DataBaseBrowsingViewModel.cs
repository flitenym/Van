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
using System.Data;
using System.Reflection;
using Van.DataBase.Model;

namespace Van.ViewModel
{
    class DataBaseBrowsingViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        } 

        private DataTable tableData = new DataTable();

        public DataTable TableData
        {
            get { return tableData; }
            set
            {
                if (value == null || value.DataSet == null) return;
                tableData = value; 
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(TableData)));
            }
        }

        public DataBaseBrowsingViewModel() { 
            var x = DataBase.SQLExecutor.Get<OldModel>("select * from Old");
            tableData = ToDataTable(x);
        }  


    }
}

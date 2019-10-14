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
using System.Collections.ObjectModel;
using Van.DataBase;

namespace Van.ViewModel
{
    class DataBaseBrowsingViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private DataTable tableData;

        public DataTable TableData
        {
            get { return tableData; }
            set
            {
                if (value == null) return;
                tableData = value; 
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(TableData)));
            }
        }

        private ObservableCollection<string> databaseModels = new ObservableCollection<string>();

        public ObservableCollection<string> DatabaseModels
        {
            get { return databaseModels; }
            set
            {
                databaseModels = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(DatabaseModels)));
            }
        }

        public DataBaseBrowsingViewModel() {
            TableData = new DataTable();
            DatabaseModels = new ObservableCollection<string>(GetNamesWithMyAttribute());
            SelectedModel = databaseModels.FirstOrDefault();
        } 

        public static IEnumerable<string> GetNamesWithMyAttribute()
        {
            var assembly = Assembly.GetExecutingAssembly();

            foreach (Type type in assembly.GetTypes())
            {
                if (Attribute.IsDefined(type, typeof(DatabaseAttribute)))
                    yield return type.Name;
            }
        } 

        private string selectedModel;
        public string SelectedModel
        {
            get { return selectedModel; }
            set
            {
                selectedModel = value;
                TableData = DataBase.Helper.GetDataByName(selectedModel);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedModel))); 
            }
        }



    }
}

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
                TableData = DataBase.Helper.GetData(selectedModel);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedModel))); 
            }
        }

        private RelayCommand deleteRowCommand;
        public RelayCommand DeleteRowCommand
        {
            get
            {
                return deleteRowCommand ??
                  (deleteRowCommand = new RelayCommand(obj =>
                  {
                      if (obj != null && ((DataRowView)obj).Row is DataRow selectedItem)
                      {
                          DeleteRows(selectedItem);
                      }
                  }));
            }
        }

        public void DeleteRows(DataRow selectedItems)
        {
            var index = tableData.Rows.IndexOf(selectedItems);

            string queryConditions = string.Empty;

            for (int i = 0; i < selectedItems.Table.Columns.Count; i++) { 
                string columnName = selectedItems.Table.Columns[i].ColumnName;
                if (columnName.ToLower().Trim() == "id") {
                    var rowValue = tableData.Rows[index].ItemArray[i];
                    queryConditions = $"{columnName} = {rowValue}";
                }
            }



            if (!string.IsNullOrEmpty(queryConditions))
            {
                DataBase.Helper.DeleteData(selectedModel, queryConditions);
                tableData.Rows.Remove(selectedItems);
            } 
        }

        private RelayCommand insertRowCommand;
        public RelayCommand InsertRowCommand
        {
            get
            {
                return insertRowCommand ??
                  (insertRowCommand = new RelayCommand(obj =>
                  {
                      InsertRows(); 
                  }));
            }
        }

        public void InsertRows()
        {   
            DataBase.Helper.InsertData(selectedModel);
            var newRow = tableData.NewRow(); 
        }


    }
}

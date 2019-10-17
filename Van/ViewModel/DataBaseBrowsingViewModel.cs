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
using System.Collections.ObjectModel;
using Van.DataBase;
using Dapper;

namespace Van.ViewModel
{
    class DataBaseBrowsingViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { }; 

        public DataBaseBrowsingViewModel()
        { 
            TableData = new DataTable();
            DatabaseModels = new ObservableCollection<ModelClass>(StaticReflectionHelper.GetAllInstancesOf<ModelClass>());
            SelectedModel = DatabaseModels.FirstOrDefault();
        }

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

        private ObservableCollection<ModelClass> databaseModels = new ObservableCollection<ModelClass>();

        public ObservableCollection<ModelClass> DatabaseModels
        {
            get { return databaseModels; }
            set
            {
                databaseModels = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(DatabaseModels)));
            }
        } 

        private ModelClass selectedModel;

        private string SelectedModelName => SelectedModel.GetType().Name;

        public ModelClass SelectedModel
        {
            get { return selectedModel; }
            set
            {
                selectedModel = value;
                TableData = SQLExecutor.SelectExecutor(SelectedModelName);
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
            var index = TableData.Rows.IndexOf(selectedItems);
            int ID = -1;
            for (int i = 0; i < selectedItems.Table.Columns.Count; i++) { 
                string columnName = selectedItems.Table.Columns[i].ColumnName;
                if (columnName.ToLower().Trim() == "id") {
                    ID = (int?)TableData.Rows[index].ItemArray[i] ?? -1;
                    break;
                }
            }

            if (ID != -1)
            {
                SQLExecutor.DeleteExecutor(SelectedModelName, ID);
                TableData.Rows.Remove(selectedItems);
                Message("Удаление успешно");
            }
            else {
                Message("Удаление произошло неудачно");
            } 
        }

        private RelayCommand insertRowCommand;
        public RelayCommand InsertRowCommand
        {
            get
            {
                return insertRowCommand ??
                  (insertRowCommand = new RelayCommand(x =>
                  { 
                        InsertRows(); 
                  }));
            }
        }

        public void InsertRows()
        {
            var newRow = TableData.NewRow();
            var ID = SQLExecutor.InsertExecutor(SelectedModelName, SelectedModel); 

            if (ID != -1)
            {
                newRow["ID"] = ID; 
                TableData.Rows.Add(newRow);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(TableData)));
                Message("Добавление успешно");
            }
            else
            {
                Message("Добавление произошло неудачно");
            }
        }

    }
}

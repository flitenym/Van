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
using System.Collections;

namespace Van.ViewModel
{
    class DataBaseBrowsingViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { }; 

        public DataBaseBrowsingViewModel()
        { 
            TableData = new DataTable();
            var models = StaticReflectionHelper.GetAllInstancesOf<ModelClass>().ToList();
            models.ForEach(x => x.Title = GetModelTitleAttribute(x));
            DatabaseModels = new ObservableCollection<ModelClass>(models);
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

        private string GetModelTitleAttribute(object model) {
            var customAttributes = (ModelTitleAttribute[])model.GetType().GetCustomAttributes(typeof(ModelTitleAttribute), true);
            if (customAttributes.Length > 0)
            {
                var myAttribute = customAttributes[0];
                return myAttribute.TableTitle; 
            }
            return string.Empty;
        }

        public ModelClass SelectedModel
        {
            get { return selectedModel; }
            set
            {
                selectedModel = value;
                TableData = SQLExecutor.SelectExecutor(SelectedModelName);
                TableData.AcceptChanges();
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
                      if (obj != null)
                      {
                          IList selectedItemsList = (IList)obj;
                          var selectedItemsCollection = selectedItemsList.Cast<DataRowView>(); 
                          DeleteRows(selectedItemsCollection.ToList());
                      }
                  }));
            }
        }

        public void DeleteRows(IList<DataRowView> selectedItems)
        {
            if (selectedItems.Count() == 0) {
                Message("Нет выделенных строк");
                return;
            }
            foreach (var selectedItem in selectedItems) {
                var index = TableData.Rows.IndexOf(selectedItem.Row);
                int ID = -1;
                for (int i = 0; i < selectedItem.Row.Table.Columns.Count; i++)
                {
                    string columnName = selectedItem.Row.Table.Columns[i].ColumnName;
                    if (columnName.ToLower().Trim() == "id")
                    {
                        ID = (int?)TableData.Rows[index].ItemArray[i] ?? -1;
                        break;
                    }
                }
                 
                SQLExecutor.DeleteExecutor(SelectedModelName, ID); 
            }

            foreach (var selectedItem in selectedItems) {
                TableData.Rows.Remove(selectedItem.Row);
            }

            Message("Удаление успешно"); 
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
                TableData.AcceptChanges();
                Message("Добавление успешно");
            }
            else
            {
                Message("Добавление произошло неудачно");
            }
        }

        private RelayCommand updateRowCommand;
        public RelayCommand UpdateRowCommand
        {
            get
            {
                return updateRowCommand ??
                  (updateRowCommand = new RelayCommand(x =>
                  {
                      UpdateRows();
                  }));
            }
        }

        public void UpdateRows()
        {
            var tableData = TableData.GetChanges();

            if (tableData == null || tableData.Rows.Count == 0 ) {
                Message("Изменения не найдены");
                return;
            }

            foreach (DataRow row in tableData.Rows) { 
                if (int.TryParse(row["ID"].ToString(), out int ID)){
                    SQLExecutor.UpdateExecutor(SelectedModelName, row, ID); 
                }
            }
            TableData.AcceptChanges();
            Message("Изменения сохранены");
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Van.DataBase;
using Van.Helper;
using static Van.Helper.Helper;

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
            models.ForEach(x => x.CanInsert = GetModelCanInsertAttribute(x));
            models.ForEach(x => x.CanDelete = GetModelCanDeleteAttribute(x));
            models.ForEach(x => x.CanUpdate = GetModelCanUpdateAttribute(x));

            DatabaseModels = new ObservableCollection<ModelClass>(models);
            SelectedModel = DatabaseModels.FirstOrDefault();
        }

        /// <summary>
        /// Взять название из атрибута чтобы отобразить корректно для пользователя
        /// </summary>
        private string GetModelTitleAttribute(object model)
        {
            var customAttributes = (ModelClassAttribute[])model.GetType().GetCustomAttributes(typeof(ModelClassAttribute), true);
            if (customAttributes.Length > 0)
            {
                var myAttribute = customAttributes[0];
                if (myAttribute.TableTitle != null)
                    return myAttribute.TableTitle;
            }
            return model.GetType().Name;
        }
         
        private bool GetModelCanInsertAttribute(object model)
        {
            var customAttributes = (ModelClassAttribute[])model.GetType().GetCustomAttributes(typeof(ModelClassAttribute), true);
            if (customAttributes.Length > 0)
            {
                var myAttribute = customAttributes[0];
                return myAttribute.CanInsert;
            }
            return true;
        }

        private bool GetModelCanUpdateAttribute(object model)
        {
            var customAttributes = (ModelClassAttribute[])model.GetType().GetCustomAttributes(typeof(ModelClassAttribute), true);
            if (customAttributes.Length > 0)
            {
                var myAttribute = customAttributes[0];
                return myAttribute.CanUpdate;
            }
            return true;
        }

        private bool GetModelCanDeleteAttribute(object model)
        {
            var customAttributes = (ModelClassAttribute[])model.GetType().GetCustomAttributes(typeof(ModelClassAttribute), true);
            if (customAttributes.Length > 0)
            {
                var myAttribute = customAttributes[0];
                return myAttribute.CanDelete;
            }
            return true;
        }

        #region Отображение данных в таблице

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

        #endregion

        #region Все модели которые необходимо отобразить

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

        #endregion

        /// <summary>
        /// Название выбранной таблицы
        /// </summary>
        private string SelectedModelName => SelectedModel.GetType().Name;

        #region Выбранная модель (таблица)

        private ModelClass selectedModel;
        public ModelClass SelectedModel
        {
            get { return selectedModel; }
            set
            {
                selectedModel = value;  
                Task.Factory.StartNew(() =>
                    Select()
                );
            }
        }

        #endregion

        #region Обновление данных из БД, по сути Select

        private RelayCommand refreshCommand;

        public RelayCommand RefreshCommand
        {
            get
            {
                return refreshCommand ??
                  (refreshCommand = new RelayCommand(x =>
                  {
                      Task.Factory.StartNew(() =>
                          Select()
                      );
                  }));
            }
        }

        private void Select()
        {
            Loading(true);
            TableData = SQLExecutor.SelectExecutor(SelectedModelName);
            TableData.AcceptChanges();
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedModel)));
            Loading(false);
        }

        #endregion

        #region Удаление выделенных строк

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
                          var selectedItemsCollection = ((IList)obj).Cast<DataRowView>();
                          DeleteRows(selectedItemsCollection.ToList());
                      }
                  }, CanDelete));
            }
        }

        private bool CanDelete(object x)
        {
            return SelectedModel.CanDelete;
        }

        public void DeleteRows(IList<DataRowView> selectedItems)
        { 
            Loading(true);
            if (selectedItems.Count() == 0) {
                Message("Нет выделенных строк");
                Loading(false);
                return;
            }
            List<int> IDs = new List<int>();

            foreach (var selectedItem in selectedItems) { 
                for (int i = 0; i < selectedItem.Row.Table.Columns.Count; i++)
                {
                    string columnName = selectedItem.Row.Table.Columns[i].ColumnName;
                    if (columnName.ToLower().Trim() == "id")
                    {
                        IDs.Add((int?)selectedItem.Row.ItemArray[i] ?? -1);
                        break;
                    }
                } 
            }

            IDs.RemoveAll(x => x == -1); 
            SQLExecutor.DeleteExecutor(SelectedModelName, IDs);
             

            foreach (var selectedItem in selectedItems) {
                TableData.Rows.Remove(selectedItem.Row);
            }
            TableData.AcceptChanges();
            Message("Удаление успешно");
            Loading(false);

        }

        #endregion

        #region Добавление новой строки

        private RelayCommand insertRowCommand;
        public RelayCommand InsertRowCommand
        {
            get
            {
                return insertRowCommand ??
                  (insertRowCommand = new RelayCommand(x =>
                  { 
                        InsertRows(); 
                  }, CanInsert));
            }
        }

        private bool CanInsert(object x) {
            return SelectedModel.CanInsert;
        }

        public void InsertRows()
        {
            var changes = TableData.GetChanges();

            if (changes != null && changes.Rows.Count != 0)
            {
                foreach (DataRow row in changes.Rows)
                {
                    if (int.TryParse(row["ID"].ToString(), out int IDChangeRow))
                    {
                        SQLExecutor.UpdateExecutor(SelectedModelName, row, IDChangeRow);
                    }
                }
                TableData.AcceptChanges();
                Message("Внесенные изменения сохранены");
            }

            var newRow = TableData.NewRow();
            var ID = SQLExecutor.InsertExecutor(SelectedModelName, SelectedModel); 

            if (ID != -1)
            {
                newRow["ID"] = ID; 
                TableData.Rows.Add(newRow);
                TableData.AcceptChanges();
                Message("Добавление новой строки успешно");
            }
            else
            {
                Message("Добавление произошло неудачно");
            }
        }

        #endregion

        #region Применение изменений

        private RelayCommand updateRowCommand;
        public RelayCommand UpdateRowCommand
        {
            get
            {
                return updateRowCommand ??
                  (updateRowCommand = new RelayCommand(x =>
                  {
                      Task.Factory.StartNew(() =>
                          UpdateRows()
                      );
                  }, CanUpdate));
            }
        }

        private bool CanUpdate(object x)
        {
            return SelectedModel.CanUpdate;
        }

        public void UpdateRows()
        {
            var tableData = TableData.GetChanges();

            if (tableData == null || tableData.Rows.Count == 0 ) {
                Message("Изменения не найдены");
                return;
            }
            Loading(true);
            foreach (DataRow row in tableData.Rows) { 
                if (int.TryParse(row["ID"].ToString(), out int ID)){
                    SQLExecutor.UpdateExecutor(SelectedModelName, row, ID); 
                }
            } 
            TableData.AcceptChanges();
            Message("Изменения сохранены");
            Loading(false);
        }

        #endregion

    }
}

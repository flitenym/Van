using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Van.AbstractClasses;
using Van.Helper;
using Van.Helper.Attributes;
using Van.LocalDataBase;
using Van.Windows.View;
using Van.Windows.ViewModel;
using IronXL;
using Microsoft.Win32;
using static Van.Helper.HelperMethods;
using Van.Helper.StaticInfo;

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
            models.ForEach(x => x.CanLoad = GetModelCanLoadAttribute(x));

            DatabaseModelsData = new ObservableCollection<ModelClass>(models);

            FilterCollection = new CollectionViewSource();
            FilterCollection.Source = DatabaseModelsData;
            FilterCollection.Filter += FilterCollection_Filter;

            SelectedModel = DatabaseModelsData.FirstOrDefault();
        }

        #region Fields

        #region Отображение данных в таблице

        private DataTable tableData;

        public DataTable TableData
        {
            get { return tableData; }
            set
            {
                if (value == null) return;
                tableData?.Clear();
                tableData = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(TableData)));
            }
        }

        #endregion

        #region Видимость информации

        private Visibility infoVisibility = Visibility.Visible;
        public Visibility InfoVisibility
        {
            get { return infoVisibility; }
            set
            {
                infoVisibility = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(InfoVisibility)));
            }
        }

        #endregion

        #region Видимость поисковой строки

        private Visibility searchVisibility = Visibility.Hidden;
        public Visibility SearchVisibility
        {
            get { return searchVisibility; }
            set
            {
                searchVisibility = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SearchVisibility)));
            }
        }

        #endregion

        #region Текст в поисковой строке

        private string searchText = string.Empty;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                this.FilterCollection.View.Refresh();
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SearchText)));
            }
        }

        #endregion

        #region Фильтрация по тексту

        public ICollectionView DatabaseModels
        {
            get
            {
                return this.FilterCollection.View;
            }
        }

        private readonly CollectionViewSource FilterCollection;

        void FilterCollection_Filter(object sender, FilterEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                e.Accepted = true;
                return;
            }

            ModelClass usr = e.Item as ModelClass;
            if (usr.Title.ToUpper().Contains(SearchText.ToUpper()))
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = false;
            }
        }

        #endregion

        #region Все модели которые необходимо отобразить

        private ObservableCollection<ModelClass> databaseModels = new ObservableCollection<ModelClass>();

        public ObservableCollection<ModelClass> DatabaseModelsData
        {
            get { return databaseModels; }
            set
            {
                databaseModels = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(DatabaseModelsData)));
            }
        }

        #endregion

        #region Выбранная модель (таблица)

        private Type SelectedModelType => SelectedModel.GetType();

        private string SelectedModelName => SelectedModelType.Name;

        private ModelClass selectedModel;
        public ModelClass SelectedModel
        {
            get { return selectedModel; }
            set
            {
                selectedModel = value;
                if (value == null)
                {
                    TableData = new DataTable();
                }
                else
                {
                    Select();
                }
            }
        }

        #endregion

        #region Используется для поднятия кнопок

        private string hideButtonToolTip = string.Empty;

        public string HideButtonToolTip
        {
            get
            {
                return IsClicked ? "Опустить кнопки" : "Поднять кнопки";
            }
            set
            {
                hideButtonToolTip = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(HideButtonToolTip)));
            }
        }

        private bool isClicked = false;

        public bool IsClicked
        {
            get
            {
                return isClicked;
            }
            set
            {
                isClicked = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsClicked)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(HideButtonToolTip)));
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Заполнить данными

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

        private bool GetModelCanLoadAttribute(object model)
        {
            var customAttributes = (ModelClassAttribute[])model.GetType().GetCustomAttributes(typeof(ModelClassAttribute), true);
            if (customAttributes.Length > 0)
            {
                var myAttribute = customAttributes[0];
                return myAttribute.CanLoad;
            }
            return true;
        }

        #endregion

        #endregion

        #region Команда для загрузки из Excel

        private RelayCommand loadCommand;

        public RelayCommand LoadCommand
        {
            get
            {
                return loadCommand ??
                  (loadCommand = new RelayCommand(x =>
                  {
                      OpenFileDialog openFileDialog = new OpenFileDialog();
                      openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm;*.csv";
                      if (openFileDialog.ShowDialog() == true)
                      {
                          string filePath = openFileDialog.FileName;
                          LoadFromExcel(filePath);
                      }
                  }, CanLoad));
            }
        }

        private bool CanLoad(object x)
        {
            if (SelectedModel == null) return false;
            return true;
        }

        private void LoadFromExcel(string fileName)
        { 
            WorkBook workbook = WorkBook.Load(fileName);
            var mainWindow = new LoadFromExcelWindowView();
            var vm = new LoadFromExcelWindowViewModel(workbook, SelectedModel, SelectedModelType);
            mainWindow.DataContext = vm; 
            if (mainWindow.ShowDialog() == true)
            {
                Select();
            }
        }

        #endregion

        #region Команда для обновления данных из БД, по сути Select

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
                  }, CanRefresh));
            }
        }

        private bool CanRefresh(object x)
        {
            if (SelectedModel == null) return false;
            return true;
        }

        private void Select()
        {
            Loading(true);
            TableData = SQLExecutor.SelectExecutor(SelectedModelType, SelectedModelName, SettingsDictionary.round);
            TableData.AcceptChanges();
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedModel)));
            Loading(false);
        }

        #endregion

        #region Команда для удаления выделенных строк

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
            if (SelectedModel == null) return false;
            return SelectedModel.CanDelete;
        }

        public void DeleteRows(IList<DataRowView> selectedItems)
        {
            Loading(true);
            if (selectedItems.Count() == 0)
            {
                Message("Нет выделенных строк");
                Loading(false);
                return;
            }
            List<int> IDs = new List<int>();

            foreach (var selectedItem in selectedItems)
            {
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


            foreach (var selectedItem in selectedItems)
            {
                TableData.Rows.Remove(selectedItem.Row);
            }
            TableData.AcceptChanges();
            Message("Удаление успешно");
            Loading(false);

        }

        #endregion

        #region Команда для добавления новой строки

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

        private bool CanInsert(object x)
        {
            if (SelectedModel == null) return false;
            return SelectedModel.CanInsert;
        }

        public void InsertRows()
        {
            var changes = TableData.GetChanges();

            if (changes != null && changes.Rows.Count != 0)
            { 
                for (int i =0; i<changes.Rows.Count; i++)
                {
                    if (int.TryParse(changes.Rows[i]["ID"].ToString(), out int IDChangeRow))
                    {
                        SQLExecutor.UpdateExecutor(SelectedModel, SelectedModelType, changes.Rows[i], IDChangeRow);
                    }
                }
                TableData.AcceptChanges();
                Message("Внесенные изменения сохранены");
            }

            var newRow = TableData.NewRow();
            var ID = SQLExecutor.InsertExecutor(SelectedModel, SelectedModel);

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

        #region Команда для применения изменений

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
            if (SelectedModel == null) return false;
            return SelectedModel.CanUpdate;
        }

        public void UpdateRows()
        {
            var tableData = TableData.GetChanges();

            if (tableData == null || tableData.Rows.Count == 0)
            {
                Message("Изменения не найдены");
                return;
            }
            Loading(true);
            for (int i = 0; i < tableData.Rows.Count; i++)
            {
                if (int.TryParse(tableData.Rows[i]["ID"].ToString(), out int ID))
                {
                    SQLExecutor.UpdateExecutor(SelectedModel, SelectedModelType, tableData.Rows[i], ID);
                }
            }
            TableData.AcceptChanges();
            Message("Изменения сохранены");
            Loading(false);
        }

        #endregion

        #region Команда для поиска

        private RelayCommand searchCommand;
        public RelayCommand SearchCommand
        {
            get
            {
                return searchCommand ??
                  (searchCommand = new RelayCommand(x =>
                  {
                      SearchVisibility = SearchVisibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
                      InfoVisibility = SearchVisibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
                      SearchText = string.Empty;
                  }));
            }
        }

        #endregion

        #region Команда для поиска

        private RelayCommand replaceCommand;
        public RelayCommand ReplaceCommand
        {
            get
            {
                return replaceCommand ??
                  (replaceCommand = new RelayCommand(x =>
                  {
                      IsClicked = !IsClicked;
                  }));
            }
        }

        #endregion

        #region Команда для поиска

        private RelayCommand searchCloseCommand;
        public RelayCommand SearchCloseCommand
        {
            get
            {
                return searchCloseCommand ??
                  (searchCloseCommand = new RelayCommand(x =>
                  {
                      SearchText = string.Empty;
                  }));
            }
        }

        #endregion

    }
}

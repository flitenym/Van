﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using SharedLibrary.AbstractClasses;
using SharedLibrary.Commands;
using SharedLibrary.Helper;
using SharedLibrary.Helper.Attributes;
using SharedLibrary.LocalDataBase;
using SharedLibrary.View;
using SharedLibrary.ViewModel;
using Microsoft.Win32;
using static SharedLibrary.Helper.HelperMethods;
using System.Windows.Controls;
using System.IO;
using ExcelDataReader;

namespace SharedLibrary.ViewModel
{
    public class DataBaseBrowsingViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public DataBaseBrowsingViewModel()
        {
            LoadDataBase();
        }

        public void LoadDataBase()
        {
            TableData = new DataTable();

            var models = GetAllInstancesOf<ModelClass>(new List<ModelClass>()).ToList();
            models.ForEach(x => x.Title = GetModelTitleAttribute(x));
            models.ForEach(x => x.SetCanInsert(GetModelCanInsertAttribute(x)));
            models.ForEach(x => x.SetCanDelete(GetModelCanDeleteAttribute(x)));
            models.ForEach(x => x.SetCanUpdate(GetModelCanUpdateAttribute(x)));
            models.ForEach(x => x.SetCanLoad(GetModelCanLoadAttribute(x)));
            models.ForEach(x => x.SetIsVisible(GetModelIsVisibleAttribute(x)));

            DatabaseModelsData = new ObservableCollection<ModelClass>(models.Where(x => x.GetIsVisible() == true));

            FilterCollection = new CollectionViewSource
            {
                Source = DatabaseModelsData
            };
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

        private CollectionViewSource FilterCollection;

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

        private Type SelectedModelType => SelectedModel?.GetType();

        private string SelectedModelName => SelectedModelType?.Name;

        private ModelClass selectedModel;
        public ModelClass SelectedModel
        {
            get { return selectedModel; }
            set
            { 
                selectedModel = value;
                RefreshCommand.Execute(null);
            }
        }

        #endregion

        #region Используется для поднятия кнопок

        private string hideButtonToolTip = string.Empty;

        public string HideButtonToolTip
        {
            get
            {
                hideButtonToolTip = IsClicked ? "Опустить кнопки" : "Поднять кнопки";
                return hideButtonToolTip;
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

        private bool GetModelIsVisibleAttribute(object model)
        {
            var customAttributes = (ModelClassAttribute[])model.GetType().GetCustomAttributes(typeof(ModelClassAttribute), true);
            if (customAttributes.Length > 0)
            {
                var myAttribute = customAttributes[0];
                return myAttribute.IsVisible;
            }
            return true;
        }

        #endregion

        #endregion

        #region Команда для загрузки из Excel

        private RelayCommand loadCommand;

        public RelayCommand LoadCommand => loadCommand ?? (loadCommand = new RelayCommand(x => Load(), (o) => CanLoad()));

        private void Load()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Excel Files|*.xls;*.xlsx;*.xlsm;*.csv"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                try
                {
                    var loadFromExcelWindow = new LoadFromExcelWindowView();

                    var vm = new LoadFromExcelWindowViewModel(fileName, SelectedModel, SelectedModelType);

                    loadFromExcelWindow.DataContext = vm;
                    if (loadFromExcelWindow.ShowDialog() == true)
                    {
                        Task.Factory.StartNew(() =>
                            Select()
                        );
                    }
                }
                catch (Exception ex)
                {
                    Task.Factory.StartNew(() =>
                        Message($"{ex.Message}")
                    );
                }
            }
        }

        private bool CanLoad()
        {
            if (SelectedModel == null) return false;
            return SelectedModel.GetCanLoad();
        }

        #endregion

        #region Команда для обновления данных из БД, по сути Select

        private AsyncCommand refreshCommand;

        public AsyncCommand RefreshCommand => refreshCommand ?? (refreshCommand = new AsyncCommand(x => Select(), (o) => CanRefresh()));

        private bool CanRefresh()
        {
            if (SelectedModel == null) return false;
            return true;
        }

        private async Task Select()
        {
            if (SelectedModelType == null || SelectedModelName == null)
            {
                TableData = new DataTable();
            }
            else
            {
                TableData = await SQLExecutor.SelectExecutorAsync(SelectedModelType, SelectedModelName);
            } 

            TableData.AcceptChanges();
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedModel))); 
        }

        #endregion

        #region Команда для удаления выделенных строк

        private AsyncCommand deleteRowCommand;

        public AsyncCommand DeleteRowCommand => deleteRowCommand ?? (deleteRowCommand = new AsyncCommand(obj => DeleteRows(obj), x => CanDelete()));

        private bool CanDelete()
        {
            if (SelectedModel == null) return false;
            return SelectedModel.GetCanDelete();
        }

        public async Task DeleteRows(object obj)
        {
            List<DataRowView> selectedItems = new List<DataRowView>();

            if (obj != null)
            {
                selectedItems = ((IList)obj).Cast<DataRowView>().ToList();
            }
            else return;

            
            if (selectedItems.Count() == 0)
            {
                await Message("Нет выделенных строк");
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

            await SQLExecutor.DeleteExecutor(SelectedModelName, IDs);


            foreach (var selectedItem in selectedItems)
            {
                TableData.Rows.Remove(selectedItem.Row);
            }
            TableData.AcceptChanges();
            await Message("Удаление успешно");
        }

        #endregion

        #region Команда для добавления новой строки

        private AsyncCommand insertRowCommand;
        public AsyncCommand InsertRowCommand => insertRowCommand ?? (insertRowCommand = new AsyncCommand(x => InsertRows(), y => CanInsert()));

        private bool CanInsert()
        {
            if (SelectedModel == null) return false;
            return SelectedModel.GetCanInsert();
        }

        public async Task InsertRows()
        {
            var changes = TableData.GetChanges();

            if (changes != null && changes.Rows.Count != 0)
            { 
                for (int i =0; i<changes.Rows.Count; i++)
                {
                    if (int.TryParse(changes.Rows[i]["ID"].ToString(), out int IDChangeRow))
                    {
                        await SQLExecutor.UpdateExecutorAsync(SelectedModel, SelectedModelType, changes.Rows[i], IDChangeRow);
                    }
                }
                TableData.AcceptChanges();
                await Message("Внесенные изменения сохранены");
            }

            var newRow = TableData.NewRow();
            var ID = await SQLExecutor.InsertExecutorAsync(SelectedModel, SelectedModel); 
            if (ID != -1)
            {
                newRow["ID"] = ID;
                TableData.Rows.Add(newRow);
                TableData.AcceptChanges();
                await Message("Добавление новой строки успешно");
            }
            else
            {
                await Message("Добавление произошло неудачно");
            }
        }

        #endregion

        #region Команда для применения изменений

        private AsyncCommand updateRowCommand;
        public AsyncCommand UpdateRowCommand => updateRowCommand ?? (updateRowCommand = new AsyncCommand(x => UpdateRowsAsync(), y => CanUpdate()));

        private bool CanUpdate()
        {
            if (SelectedModel == null) return false;
            return SelectedModel.GetCanUpdate();
        }

        public async Task UpdateRowsAsync()
        {
            var tableData = TableData.GetChanges();

            if (tableData == null || tableData.Rows.Count == 0)
            {
                await Message("Изменения не найдены");
                return;
            }
            for (int i = 0; i < tableData.Rows.Count; i++)
            {
                if (int.TryParse(tableData.Rows[i]["ID"].ToString(), out int ID))
                {
                    await SQLExecutor.UpdateExecutorAsync(SelectedModel, SelectedModelType, tableData.Rows[i], ID);
                }
            }
            TableData.AcceptChanges();
            await Message("Изменения сохранены");
        }

        #endregion

        #region Команда для поиска

        private RelayCommand searchCommand;
        public RelayCommand SearchCommand => searchCommand ?? (searchCommand = new RelayCommand(x => SearchFunction()));

        public void SearchFunction()
        {
            SearchVisibility = SearchVisibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
            InfoVisibility = InfoVisibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
            SearchText = string.Empty;
        }

        #endregion

        #region Команда для закрытия

        private RelayCommand searchCloseCommand;
        public RelayCommand SearchCloseCommand => searchCloseCommand ?? (searchCloseCommand = new RelayCommand(x => SearchTextEmpty()));

        public void SearchTextEmpty()
        {
            SearchText = string.Empty; 
        }

        #endregion

        #region Команда для перемещения панели

        private RelayCommand replaceCommand;
        public RelayCommand ReplaceCommand => replaceCommand ?? (replaceCommand = new RelayCommand(x => ReplaceCommandIsClicked()));

        public void ReplaceCommandIsClicked()
        {
            IsClicked = !IsClicked;
        }

        #endregion

        #region Команда для перемещения панели

        private RelayCommand scrollItemCommand;
        public RelayCommand ScrollItemCommand => scrollItemCommand ?? (scrollItemCommand = new RelayCommand(obj => Scroll(obj)));

        public void Scroll(object obj)
        {
            if (obj is DataGrid dataGrid)
            {
                var item = dataGrid.Items[dataGrid.Items.Count - 1];
                dataGrid.ScrollIntoView(item);
                dataGrid.SelectedItem = item;
                dataGrid.UpdateLayout();
            }
        }

        #endregion
    }
}

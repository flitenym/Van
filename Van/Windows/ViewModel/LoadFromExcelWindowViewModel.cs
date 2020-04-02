using Van.AbstractClasses;
using Van.Commands;
using Van.Helper;
using Van.LocalDataBase;
using IronXL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows; 

namespace Van.Windows.ViewModel
{
    class LoadFromExcelWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public LoadFromExcelWindowViewModel(WorkBook workBook, ModelClass modelClassItem, Type type)
        {
            this.workBook = workBook;
            this.type = type;
            this.modelClassItem = modelClassItem;
            WorkSheets = workBook.WorkSheets;
            WorkSheet = WorkSheets.First();
        }

        #region Fields

        public WorkBook workBook;
        public Type type;
        public ModelClass modelClassItem;

        /// <summary>
        /// Инструкция по загрузке
        /// </summary>
        public string Instruction =>
$@"При загрузке из Excel следует придерживаться правил:
1. Загрузка будет работать только над объектом, который выбран в странице ""{Helper.StaticInfo.Types.ViewData.DataBaseBrowsing.Name}"".
2. Необходимо следовать изначальной последовательности колонок.
3. В случае если у вас для первой строки в Excel используются названия колонок, то следует снять флаг.";

        #region Игнорирование первой строки в листе Excel

        private bool ignoreFirstRow = true;
        public bool IgnoreFirstRow
        {
            get { return ignoreFirstRow; }
            set
            {
                ignoreFirstRow = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(IgnoreFirstRow)));
            }
        }

        #endregion

        #region Выбранный лист

        private WorkSheet workSheet;
        public WorkSheet WorkSheet
        {
            get { return workSheet; }
            set
            {
                workSheet = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(WorkSheet)));
            }
        }

        #endregion

        #region Все доступные листы в Excel

        private WorksheetsCollection workSheets;
        public WorksheetsCollection WorkSheets
        {
            get { return workSheets; }
            set
            {
                workSheets = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(WorkSheets)));
            }
        }

        #endregion

        #endregion

        #region Команда для старта загрузки

        private AsyncCommand startCommand;
        public AsyncCommand StartCommand => startCommand ?? (startCommand = new AsyncCommand(obj => StartLoadingAsync(obj as Window)));

        public async Task StartLoadingAsync(Window window)
        {
            await LoadAsync();

            window.DialogResult = true;
        }

        public async Task LoadAsync()
        {
            try
            {
                var dataTable = WorkSheet.ToDataTable(ignoreFirstRow);
                List<object> listObj = new List<object>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    listObj.Add(dataTable.Rows[i].ToObjectLoad(type));
                }
                if (listObj.Count > 0)
                {
                    await HelperMethods.Message($"Найдено {listObj.Count} строк, выполняется загрузка в БД");
                    for (int i = 0; i < listObj.Count; i++)
                    {
                        await SQLExecutor.InsertExecutorAsync(modelClassItem, listObj[i]);
                    }
                }
                else
                {
                    await HelperMethods.Message($"Данные не найдены");
                }
            }
            catch (Exception ex)
            {
                await HelperMethods.Message($"{ex.Message}");
            }
        }

        #endregion

        #region Команда для отмены загрузки

        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand => cancelCommand ?? (cancelCommand = new RelayCommand(obj => CancelFunction(obj)));

        public void CancelFunction(object obj)
        {
            (obj as Window).DialogResult = false;
        }

        #endregion

    }
}

using ExcelDataReader;
using Microsoft.Win32;
using SharedLibrary.AbstractClasses;
using SharedLibrary.Commands;
using SharedLibrary.Helper;
using SharedLibrary.LocalDataBase;
using SharedLibrary.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SharedLibrary.ViewModel
{
    public class LoadFromTXTWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public LoadFromTXTWindowViewModel(ModelClass modelClassItem, Type type)
        {
            this.type = type;
            this.modelClassItem = modelClassItem;
        }

        #region Fields

        public Type type;
        public ModelClass modelClassItem;

        #region Выбранный год

        private string year;
        public string Year
        {
            get { return year; }
            set
            {
                year = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Year)));
            }
        }

        #endregion

        #region Все доступные года

        private List<string> years = new List<string>();
        public List<string> Years
        {
            get { return years; }
            set
            {
                years = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Years)));
            }
        }

        #endregion

        #region Выбранный регион

        private string reg;
        public string Reg
        {
            get { return reg; }
            set
            {
                reg = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Reg)));
            }
        }

        #endregion

        #region Все доступные регион

        private List<string> regs = new List<string>();
        public List<string> Regs
        {
            get { return regs; }
            set
            {
                regs = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Regs)));
            }
        }

        #endregion

        #region Выбранная группа

        private string group;
        public string Group
        {
            get { return group; }
            set
            {
                group = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Group)));
            }
        }

        #endregion

        #region Все доступные группы

        private List<string> groups = new List<string>();
        public List<string> Groups
        {
            get { return groups; }
            set
            {
                groups = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Groups)));
            }
        }

        #endregion

        #region Выбранный пол

        private string sex;
        public string Sex
        {
            get { return sex; }
            set
            {
                sex = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Sex)));
            }
        }

        #endregion

        #region Все доступные пола

        private List<string> sexs = new List<string>();
        public List<string> Sexs
        {
            get { return sexs; }
            set
            {
                sexs = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Sexs)));
            }
        }

        #endregion

        #endregion

        #region Команда для численности населения

        public List<string> CountData = new List<string>();

        private RelayCommand getCountCommand;
        public RelayCommand GetCountCommand => getCountCommand ?? (getCountCommand = new RelayCommand(x => GetCount()));

        public void GetCount()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "TXT Files|*.txt"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string filepath = openFileDialog.FileName;

                if (File.Exists(filepath))
                {
                    CountData = new List<string>();
                    using (StreamReader sr = new StreamReader(filepath))
                    {
                        while (sr.Peek() >= 0)
                        {
                            CountData.Add(sr.ReadLine());
                        }
                    }
                }
            }
        }

        #endregion

        #region Команда для коэф

        public List<string> CoefficientData = new List<string>();

        private RelayCommand getCoefficientCommand;
        public RelayCommand GetCoefficientCommand => getCoefficientCommand ?? (getCoefficientCommand = new RelayCommand(x => GetCoefficient()));

        public void GetCoefficient()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "TXT Files|*.txt"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                string filepath = openFileDialog.FileName;

                if (File.Exists(filepath))
                {
                    CoefficientData = new List<string>();
                    using (StreamReader sr = new StreamReader(filepath))
                    {
                        while (sr.Peek() >= 0)
                        {
                            CoefficientData.Add(sr.ReadLine());
                        }
                    }
                }
            }
        }

        #endregion

        #region Команда для коэф

        public DataTable resultDataTable = new DataTable();

        private RelayCommand showDataCommand;
        public RelayCommand ShowDataCommand => showDataCommand ?? (showDataCommand = new RelayCommand(x => ShowData()));

        public void ShowData()
        {
            var showDataTable = new ShowDataTableView();
            var vm = new ShowDataTableViewModel(resultDataTable);
            showDataTable.DataContext = vm;

            showDataTable.ShowDialog();
        }

        #endregion

        #region Команда для старта загрузки

        private AsyncCommand startCommand;
        public AsyncCommand StartCommand => startCommand ?? (startCommand = new AsyncCommand(obj => StartLoadingAsync(obj as System.Windows.Window), y => CanStart()));

        public async Task StartLoadingAsync(System.Windows.Window window)
        {
            await LoadAsync();

            window.DialogResult = true;
        }

        public bool CanStart()
        {
            if (CountData.Any() && CoefficientData.Any() && CountData.Count == CoefficientData.Count)
                return true;

            return false;
        }

        public async Task LoadAsync()
        {
            //try
            //{
            //    string result;

            //    DataTable dataTable = new DataTable();

            //    for (int i = 0; i < result.Tables.Count; i++)
            //    {
            //        if (result.Tables[i].TableName == workSheet)
            //        {
            //            dataTable = result.Tables[i];
            //            break;
            //        }
            //    }

            //    List<object> listObj = new List<object>();
            //    for (int i = 0; i < dataTable.Rows.Count; i++)
            //    {
            //        listObj.Add(dataTable.Rows[i].ToObjectLoad(type));
            //    }
            //    if (listObj.Count > 0)
            //    {
            //        await HelperMethods.Message($"Найдено {listObj.Count} строк, выполняется загрузка в БД");
            //        for (int i = 0; i < listObj.Count; i++)
            //        {
            //            await SQLExecutor.InsertExecutorAsync(modelClassItem, listObj[i]);
            //        }
            //    }
            //    else
            //    {
            //        await HelperMethods.Message($"Данные не найдены");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    await HelperMethods.Message($"{ex.Message}");
            //}
        } 

        #endregion

        #region Команда для отмены загрузки

        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand => cancelCommand ?? (cancelCommand = new RelayCommand(obj => CancelFunction(obj)));

        public void CancelFunction(object obj)
        {
            (obj as System.Windows.Window).DialogResult = false;
        }

        #endregion

    }
}

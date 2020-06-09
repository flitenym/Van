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
using System.Windows;
using System.Windows.Media.Converters;

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
                if (years != null)
                    Year = years.FirstOrDefault();
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
                if (regs != null)
                    Reg = regs.FirstOrDefault();
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
                if (groups != null)
                    Group = groups.FirstOrDefault();
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
                if (sexs != null)
                    Sex = sexs.FirstOrDefault();
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Sexs)));
            }
        }

        #endregion

        #endregion

        #region Команда для численности населения

        public List<string> CountData = new List<string>();

        private RelayCommand getCountCommand;
        public RelayCommand GetCountCommand => getCountCommand ?? (getCountCommand = new RelayCommand(x => GetData(CountData, false)));

        #endregion

        #region Команда для коэф

        public List<string> CoefficientData = new List<string>();

        private RelayCommand getCoefficientCommand;
        public RelayCommand GetCoefficientCommand => getCoefficientCommand ?? (getCoefficientCommand = new RelayCommand(x => GetData(CoefficientData, true)));

        #endregion

        public void GetData(List<string> dataList, bool isCoef)
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
                    dataList = new List<string>();

                    using (StreamReader sr = new StreamReader(filepath))
                    {
                        while (sr.Peek() >= 0)
                        {
                            dataList.Add(sr.ReadLine());
                        }
                    }

                    if (isCoef) CoefficientData = dataList; else CountData = dataList;

                    var yearFromCoef = new List<string>();
                    var regFromCoef = new List<string>();
                    var groupFromCoef = new List<string>();
                    var sexFromCoef = new List<string>();
                    for (int i = 1; i < dataList.Count; i++)
                    {
                        var splits = dataList[i].Split(',');
                        if (splits.Length > 3)
                        {
                            yearFromCoef.Add(splits[0].Trim());
                            regFromCoef.Add(splits[1].Trim());
                            groupFromCoef.Add(splits[2].Trim());
                            sexFromCoef.Add(splits[3].Trim());
                        }
                    }

                    var newYears = yearFromCoef.Distinct().ToList();
                    var newRegs = regFromCoef.Distinct().ToList();
                    var newGroups = groupFromCoef.Distinct().ToList();
                    var newSexs = sexFromCoef.Distinct().ToList();

                    if (newYears.Except(Years).Any())
                        Years = newYears.OrderBy(x=>x).ToList();

                    if (newRegs.Except(Regs).Any())
                        Regs = newRegs.OrderBy(x => x).ToList();

                    if (newGroups.Except(Groups).Any())
                        Groups = newGroups.OrderBy(x => x).ToList();

                    if (newSexs.Except(Sexs).Any())
                        Sexs = newSexs.OrderBy(x => x).ToList();
                }
            }

            StartCommand.RaiseCanExecuteChanged();
            ShowDataCommand.RaiseCanExecuteChanged();
        }

        #region Команда для просмотра

        private RelayCommand showDataCommand;
        public RelayCommand ShowDataCommand => showDataCommand ?? (showDataCommand = new RelayCommand(x => ShowData(), y => CanStart()));

        public void ShowData()
        {
            DataTable dataTable = null;

            Task.Factory.StartNew(async () => { dataTable = await GetDataTable(); }).Wait();


            if (dataTable != null)
            {
                var showDataTable = new ShowDataTableView();
                var vm = new ShowDataTableViewModel(dataTable);
                showDataTable.DataContext = vm;
                showDataTable.ShowDialog();
            }
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
            if (CountData.Any() && CoefficientData.Any() && CountData.Count == CoefficientData.Count
                    && !string.IsNullOrEmpty(Year) && !string.IsNullOrEmpty(Reg) && !string.IsNullOrEmpty(Group) && !string.IsNullOrEmpty(Sex))
                return true;

            return false;
        }

        public async Task LoadAsync()
        {
            var dataTable = await GetDataTable();

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

        #endregion

        #region Команда для отмены загрузки

        private RelayCommand cancelCommand;
        public RelayCommand CancelCommand => cancelCommand ?? (cancelCommand = new RelayCommand(obj => CancelFunction(obj)));

        public void CancelFunction(object obj)
        {
            (obj as System.Windows.Window).DialogResult = false;
        }

        #endregion

        public async Task<DataTable> GetDataTable()
        {
            string resultCoef = string.Empty;
            for (int i = 1; i < CoefficientData.Count; i++)
            {
                var splits = CoefficientData[i].Split(',');
                if (splits[0].Trim() == Year && splits[1].Trim() == Reg && splits[2].Trim() == Group && splits[3].Trim() == Sex)
                {
                    resultCoef = CoefficientData[i];
                    break;
                }
            }

            if (string.IsNullOrEmpty(resultCoef))
            {
                await HelperMethods.Message("Не нашлось в файле коэффициентов данных по указанным");
                return null;
            }

            string resultCount = string.Empty;
            for (int i = 1; i < CountData.Count; i++)
            {
                var splits = CountData[i].Split(',');
                if (splits[0].Trim() == Year && splits[1].Trim() == Reg && splits[2].Trim() == Group && splits[3].Trim() == Sex)
                {
                    resultCount = CountData[i];
                    break;
                }
            }

            if (string.IsNullOrEmpty(resultCount))
            {
                await HelperMethods.Message("Не нашлось в файле населения данных по указанным");
                return null;
            }

            List<double> countData = new List<double>();
            List<double> coefData = new List<double>();


            var splitsCount = resultCount.Split(',');

            for (int i = 4; i < splitsCount.Length; i++)
            {
                if (double.TryParse(splitsCount[i].Trim(), out double val))
                {
                    countData.Add(val);
                }
            }

            var splitsCoef = resultCoef.Split(',');

            for (int i = 4; i < splitsCoef.Length; i++)
            {
                if (double.TryParse(splitsCoef[i].Trim(), out double val))
                {
                    coefData.Add(val);
                }
            }

            List<double> resultSurvivorsData = new List<double>();
            List<double> resultDeadData = new List<double>();

            for (int i = 0; i < Math.Min(countData.Count(), coefData.Count()); i++)
            {
                resultDeadData.Add(countData[i] * coefData[i] / 1000.0); 
            }

            resultSurvivorsData.Add(resultDeadData.Sum());

            for (int i = 1; i < resultDeadData.Count(); i++)
            {
                resultSurvivorsData.Add(resultSurvivorsData.Last() - resultDeadData[i]);
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("AgeX");
            dt.Columns.Add("NumberOfSurvivors");
            dt.Columns.Add("NumberOfDead");

            for (int i = 0; i < resultDeadData.Count(); i++)
            {
                DataRow dr = dt.NewRow();
                dr["AgeX"] = i;
                dr["NumberOfSurvivors"] = Convert.ToInt32(resultSurvivorsData[i]);
                dr["NumberOfDead"] = Convert.ToInt32(resultDeadData[i]);
                dt.Rows.Add(dr);
            }

            return dt;
        }

    }
}

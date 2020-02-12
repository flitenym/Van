using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Van.Helper;
using System;
using Van.Methods;
using System.Threading.Tasks;
using LiveCharts;
using Van.DataBase.Models;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Windows.Media;
using LiveCharts.Wpf;
using System.Windows;
using Dapper;
using System.Threading;
using Van.LocalDataBase;
using Van.Helper.HelperClasses;
using System.Collections.ObjectModel;
using Van.Helper.StaticInfo;

namespace Van.ViewModel.Methods
{
    class TestViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public TestViewModel()
        {
            HelperMethods.Loading(true);

            Thread thread = new Thread(LoadTables);
            thread.Start();

            HelperMethods.Loading(false);
        }

        public List<double> ageValues = new List<double>();

        public List<double> standartValues = new List<double>();

        public List<int> t = new List<int>();

        public List<int> delta = new List<int>();

        double r => delta.Where(x => x == 1).Count();

        public Random random = new Random();

        public bool HaveNullProbability = true;

        public double epsilon = 0.01; 

        public List<MortalityTable> currentMortalityTables = new List<MortalityTable>();

        public List<SurvivalFunction> currentSurvivalFunctions = new List<SurvivalFunction>();

        public List<Density> currentDensitys = new List<Density>(); 

        public List<LifeTimes> currentLifeTimes = new List<LifeTimes>();

        /// <summary>
        /// Акаики
        /// </summary>
        public QualityAssessmentOfModels Acaici = new QualityAssessmentOfModels();

        /// <summary>
        /// Расстояние от табличного первый метод
        /// </summary>
        public QualityAssessmentOfModels DistanceFirstMethod = new QualityAssessmentOfModels();

        /// <summary>
        /// Расстояние от табличного второй метод
        /// </summary>
        public QualityAssessmentOfModels DistanceSecondMethod = new QualityAssessmentOfModels();
        
        public void LoadTables()
        { 
            Task.Factory.StartNew(() =>
            {
                SelectMortality();
                SelectSurvivalFunction();
                SelectDensity();
                SelectLifeTimesFunction();
                SelectQualityAssessmentOfModels();

                RefreshChartsDivides();
            });
        }

        #region Диапазон

        private ObservableCollection<RangeData> rangeDataList = new ObservableCollection<RangeData>();

        public ObservableCollection<RangeData> RangeDataList
        {
            get { return rangeDataList; }
            set
            {
                rangeDataList = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(RangeDataList)));
            }
        }

        private RangeData firstAgeX = new RangeData();

        public RangeData FirstAgeX
        {
            get { return firstAgeX; }
            set
            {
                firstAgeX = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(FirstAgeX)));
            }
        }

        private RangeData secondAgeX = new RangeData();

        public RangeData SecondAgeX
        {
            get { return secondAgeX; }
            set
            {
                secondAgeX = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SecondAgeX)));
            }
        }

        #endregion

        #region Таблица оценка качесва моделей

        private DataTable qualityAssessmentOfModelsTableData;

        public DataTable QualityAssessmentOfModelsTableData
        {
            get { return qualityAssessmentOfModelsTableData; }
            set
            {
                if (value == null) return;
                qualityAssessmentOfModelsTableData?.Clear();
                qualityAssessmentOfModelsTableData?.Columns.Clear();
                qualityAssessmentOfModelsTableData?.Rows.Clear();

                qualityAssessmentOfModelsTableData = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(QualityAssessmentOfModelsTableData)));
            }
        }

        private void SelectQualityAssessmentOfModels()
        {
            QualityAssessmentOfModelsTableData = SQLExecutor.SelectExecutor(typeof(QualityAssessmentOfModels), nameof(QualityAssessmentOfModels), SettingsDictionary.round);
            QualityAssessmentOfModelsTableData.AcceptChanges(); 
        }

        #endregion

        #region Таблица смертности

        private DataTable mortalityTableData;

        public DataTable MortalityTableData
        {
            get { return mortalityTableData; }
            set
            {
                if (value == null) return;
                mortalityTableData?.Clear();
                mortalityTableData?.Columns.Clear();
                mortalityTableData?.Rows.Clear();

                mortalityTableData = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(MortalityTableData)));
            }
        }

        private void SelectMortality(bool needTableRefresh = true)
        {
            if (needTableRefresh)
            {
                MortalityTableData = SQLExecutor.SelectExecutor(typeof(MortalityTable), nameof(MortalityTable), SettingsDictionary.round);
                MortalityTableData.AcceptChanges();
            }

            using (var slc = new SQLiteConnection(SQLExecutor.LoadConnectionString))
            {
                slc.Open();
                currentMortalityTables = slc.Query<MortalityTable>($"SELECT * FROM {nameof(MortalityTable)}").ToList();
            }

            NValue = currentMortalityTables.Select(x => x.NumberOfSurvivors).Max().Value;
            if (currentMortalityTables.Where(x => x.Probability == null).Any())
                HaveNullProbability = true;
            else HaveNullProbability = false;


            ageValues = new List<double>();

            var temp = new ObservableCollection<RangeData>(); 

            for (int i = 0; i < currentMortalityTables.Count; i++)
            {
                ageValues.Add(getTValue(currentMortalityTables[i]?.AgeX));

                temp.Add(new RangeData() { ID = (currentMortalityTables[i]?.ID).Value, AgeX = (int)getTValue(currentMortalityTables[i]?.AgeX) });
            }

            RangeDataList = temp;

            //FirstAgeX = RangeDataList.First();
            //SecondAgeX = RangeDataList.First();
        }

        #endregion

        #region Таблица s(t)

        private DataTable survivalFunctionTable;

        public DataTable SurvivalFunctionTable
        {
            get { return survivalFunctionTable; }
            set
            {
                if (value == null) return;
                survivalFunctionTable?.Clear();
                survivalFunctionTable?.Columns.Clear();
                survivalFunctionTable?.Rows.Clear();

                survivalFunctionTable = value; 

                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SurvivalFunctionTable)));
            }
        }

        private void SelectSurvivalFunction(bool needTableRefresh = true)
        {
            if (needTableRefresh)
            {
                SurvivalFunctionTable = SQLExecutor.SelectExecutor(typeof(SurvivalFunction), nameof(SurvivalFunction), SettingsDictionary.round);
                SurvivalFunctionTable.AcceptChanges();
            }

            using (var slc = new SQLiteConnection(SQLExecutor.LoadConnectionString))
            {
                slc.Open();
                currentSurvivalFunctions = slc.Query<SurvivalFunction>($"SELECT * FROM {nameof(SurvivalFunction)}").ToList();
            }

            if (!currentSurvivalFunctions.Where(x => x.Standart == null).Any())
            {
                RefreshChartsSurvivalFunctions();
            }

            standartValues.Clear();
            standartValues = currentSurvivalFunctions.Select(x => x.Standart ?? 0).ToList();
        }

        #endregion

        #region Таблица f(t)

        private DataTable densityTable;

        public DataTable DensityTable
        {
            get { return densityTable; }
            set
            {
                if (value == null) return;
                densityTable?.Clear();
                densityTable?.Columns.Clear();
                densityTable?.Rows.Clear();

                densityTable = value;

                PropertyChanged(this, new PropertyChangedEventArgs(nameof(DensityTable)));
            }
        }

        private void SelectDensity(bool needTableRefresh = true)
        {
            if (needTableRefresh)
            {
                DensityTable = SQLExecutor.SelectExecutor(typeof(Density), nameof(Density), SettingsDictionary.round);
                DensityTable.AcceptChanges();
            }

            using (var slc = new SQLiteConnection(SQLExecutor.LoadConnectionString))
            {
                slc.Open();
                currentDensitys = slc.Query<Density>($"SELECT * FROM {nameof(Density)}").ToList();
            }

            if (!currentDensitys.Where(x => x.Standart == null).Any())
            {
                RefreshChartsDensitys();
            }
        }

        #endregion

        #region Таблица t

        private DataTable lifeTimesTable;

        public DataTable LifeTimesTable
        {
            get { return lifeTimesTable; }
            set
            {
                if (value == null) return;
                lifeTimesTable?.Clear();
                lifeTimesTable?.Columns.Clear();
                lifeTimesTable?.Rows.Clear();
                lifeTimesTable = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(LifeTimesTable)));
            }
        }

        private void SelectLifeTimesFunction(bool needTableRefresh = true)
        {
            if (needTableRefresh)
            {
                LifeTimesTable = SQLExecutor.SelectExecutor(typeof(LifeTimes), nameof(LifeTimes), SettingsDictionary.round);
                LifeTimesTable.AcceptChanges();
            }


            using (var slc = new SQLiteConnection(SQLExecutor.LoadConnectionString))
            {
                slc.Open();
                currentLifeTimes = slc.Query<LifeTimes>($"SELECT * FROM {nameof(LifeTimes)}").ToList();
            }

            t = currentLifeTimes.Select(x => x.LifeTime.Value).ToList();
            delta = currentLifeTimes.Select(x => x.Censor.Value).ToList();
        }

        #endregion

        #region n значение

        private int nValue;

        public int NValue
        {
            get { return nValue; }
            set
            {
                nValue = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(NValue)));
            }
        }

        #endregion

        #region Комманда для вычисления T

        private RelayCommand calculateTCommand;
        public RelayCommand CalculateTCommand
        {
            get
            {
                return calculateTCommand ??
                  (calculateTCommand = new RelayCommand(x =>
                  {
                      Task.Factory.StartNew(() =>
                          CalculateT()
                      );
                  }, CanCalculateT));
            }
        }

        private bool CanCalculateT(object x)
        {
            return !HaveNullProbability;
        }

        public void CalculateT()
        {
            HelperMethods.Loading(true);
            t = new List<int>(); 

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            GenerateT();
            
            RewriteLifeTimesTable();
            
            SelectLifeTimesFunction();

            HelperMethods.Message($"t вычислено. Время: {stopwatch.Elapsed.TotalSeconds.ToString()}");
            stopwatch.Stop();

            HelperMethods.Loading(false);
        }

        public void GenerateT()
        {
            var minimalProb = currentMortalityTables.Select(x => x.Probability).Min().Value;
            var maxsurv = currentMortalityTables.Select(x => x.NumberOfSurvivors).Max().Value;
            minimalProb = minimalProb / 2; 

            for (int i = 0; i < NValue; i++)
            {
                int randomNumber = random.Next(0, maxsurv + 1);
                double z = (double)randomNumber / maxsurv;
                z -= minimalProb;

                double sumProbubility = 0;

                for (int j = 0; j < currentMortalityTables.Count(); j++)
                {
                    if (z < currentMortalityTables[j].Probability)
                    {
                        t.Add((int)getTValue(currentMortalityTables[j].AgeX));
                        break;
                    }
                    else if (j != 0 && z >= sumProbubility && z < sumProbubility + currentMortalityTables[j].Probability.Value)
                    {
                        t.Add((int)getTValue(currentMortalityTables[j].AgeX));
                        break;
                    }
                    sumProbubility += currentMortalityTables[j].Probability.Value;
                }
            }
        }

        public void RewriteLifeTimesTable()
        {

            currentLifeTimes = new List<LifeTimes>();
            delta = new List<int>();

            for (int i = 0; i < t.Count(); i++)
            {
                currentLifeTimes.Add(new LifeTimes() { LifeTime = t[i], Censor = 1 });
                delta.Add(1);
            }

            using (var cn = new SQLiteConnection(SQLExecutor.LoadConnectionString))
            {
                cn.Open();
                using (var transaction = cn.BeginTransaction())
                {
                    using (var cmd = cn.CreateCommand())
                    {
                        cmd.CommandText = $"DELETE FROM LifeTimes";
                        cmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                cn.Close();
            }


            using (var cn = new SQLiteConnection(SQLExecutor.LoadConnectionString))
            {
                cn.Open();
                using (var transaction = cn.BeginTransaction())
                {
                    using (var cmd = cn.CreateCommand())
                    {
                        cmd.CommandText = $"INSERT INTO LifeTimes(LifeTime, Censor) VALUES(@LifeTime, @Censor);";
                        for (int i = 0; i < currentLifeTimes.Count(); i++)
                        {
                            cmd.Parameters.AddWithValue("@LifeTime", currentLifeTimes[i].LifeTime);
                            cmd.Parameters.AddWithValue("@Censor", currentLifeTimes[i].Censor);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
                cn.Close();
            }
        }

        /// <summary>
        /// В AgeX может быть лишние значение такие как + и т.д., поэтому спарсим только числа
        /// </summary>
        public double getTValue(string ageX) {
            // в AgeX может быть лишние значение такие как + и т.д., поэтому спарсим только числа
            if (double.TryParse(string.Join("", ageX.Where(c => char.IsDigit(c))), out double value))
            { 
                return value;
            }
            return 0;
        }

        #endregion 

        #region Комманда для вычисления числа умерших d(x)    

        private RelayCommand calculateDataCommand;
        public RelayCommand CalculateDataCommand
        {
            get
            {
                return calculateDataCommand ??
                  (calculateDataCommand = new RelayCommand(x =>
                  {
                      Task.Factory.StartNew(() =>
                            CalculateMortality()
                      );
                  }));
            }
        }

        public void CalculateMortality()
        {
            HelperMethods.Loading(true); 
            
            //обновим таблицу через БД
            SelectMortality(false);

            if (currentMortalityTables.Count() < 2)
            {
                SelectMortality();
                HelperMethods.Message("Слишком мало данных");
                HelperMethods.Loading(false);
                return;
            }

            CalculateNumberOfDead();

            CalculateProbability();

            //Обновим в БД данные исходя из текущего списка
            UpdateMortality(); 
            //обновим таблицу через БД
            SelectMortality();

            HelperMethods.Message("Вычисление прошло успешно");
            HelperMethods.Loading(false);
        }

        private void CalculateNumberOfDead() {
            int mortalityTableCount = currentMortalityTables.Count() - 1;

            for (int i = 0; i < mortalityTableCount; i++)
            {
                currentMortalityTables[i].NumberOfDead = currentMortalityTables[i].NumberOfSurvivors - currentMortalityTables[i + 1].NumberOfSurvivors;
            }
            currentMortalityTables[mortalityTableCount].NumberOfDead = currentMortalityTables[mortalityTableCount].NumberOfSurvivors;
        }

        private void CalculateProbability()
        {
            for (int i = 0; i < currentMortalityTables.Count(); i++)
            {
                currentMortalityTables[i].Probability = (double?)currentMortalityTables[i].NumberOfDead / (double)currentMortalityTables.First().NumberOfSurvivors;
            }
        }

        #endregion

        #region Комманда для обновления таблицы (используется после вычислений)

        private RelayCommand updateMortalityCommand;
        public RelayCommand UpdateMortalityCommand
        {
            get
            {
                return updateMortalityCommand ??
                  (updateMortalityCommand = new RelayCommand(x =>
                  {
                      Task.Factory.StartNew(() =>
                          UpdateMortalityTable()
                      );
                  }));
            }
        }

        public void UpdateMortalityTable()
        {
            HelperMethods.Loading(true);

            //обновим таблицу через БД
            SelectMortality();

            HelperMethods.Message("Таблица обновлена");
            HelperMethods.Loading(false);
        }


        private void UpdateMortality()
        {
            for (int i = 0; i < currentMortalityTables.Count(); i++)
            {
                SQLExecutor.UpdateExecutor(currentMortalityTables[i], currentMortalityTables[i], currentMortalityTables[i].ID);
            }
        }

        #endregion

        #region Комманда для методов

        private RelayCommand calculateMethodsCommand;
        public RelayCommand CalculateMethodsCommand
        {
            get
            {
                return calculateMethodsCommand ??
                  (calculateMethodsCommand = new RelayCommand(x =>
                  {
                      Task.Factory.StartNew(() =>
                          CalculateMethodsAsync()
                      );
                  }, CanCalculateMethods));
            }
        }

        private bool CanCalculateMethods(object x)
        {
            return t.Any() && currentSurvivalFunctions.Any() && currentSurvivalFunctions.Count == currentMortalityTables.Count;
        }

        private void UpdateST()
        {
            for (int i = 0; i < currentSurvivalFunctions.Count(); i++) 
            {
                SQLExecutor.UpdateExecutor(currentSurvivalFunctions[i], currentSurvivalFunctions[i], currentSurvivalFunctions[i].ID);
            }
        }

        private void UpdateD()
        {
            for (int i = 0; i < currentDensitys.Count(); i++)
            {
                SQLExecutor.UpdateExecutor(currentDensitys[i], currentDensitys[i], currentDensitys[i].ID);
            }
        }

        public async Task CalculateMethodsAsync()
        {
            HelperMethods.Loading(true); 

            var stopwatch = new Stopwatch();
            stopwatch.Start();
             
            Acaici = new QualityAssessmentOfModels() { Quality = "Акаики"};
             
            DistanceFirstMethod = new QualityAssessmentOfModels() { Quality = "Первая метрика" };
            DistanceSecondMethod = new QualityAssessmentOfModels() { Quality = "Вторая метрика" };

            CalculateSTStandart();


            var taskWeibull = new Task(CalculateSTWeibull);
            var taskRelay = new Task(CalculateSTRelay);
            var taskGompertz = new Task(CalculateSTGompertz);
            var taskExponential = new Task(CalculateSTExponential);

            taskWeibull.Start();
            taskRelay.Start();
            taskGompertz.Start();
            taskExponential.Start();

            await Task.WhenAll(
                taskWeibull,
                taskRelay,
                taskGompertz,
                taskExponential);


            var taskST = new Task(UpdateAndSelectST);
            var taskD = new Task(UpdateAndSelectD);

            taskST.Start();
            taskD.Start();

            await Task.WhenAll(
                taskST,
                taskD);

            RefreshChartsDivides();

            //--------Обновление оценки качества
            QualityUpdate();


            HelperMethods.Message($"Время: {stopwatch.Elapsed.TotalSeconds.ToString()}");
            stopwatch.Stop();

            HelperMethods.Loading(false);
        }

        public void UpdateAndSelectST() {
            UpdateST();
            SelectSurvivalFunction();
        }

        public void UpdateAndSelectD()
        {
            UpdateD();
            SelectDensity();
        }

        public void CalculateSTStandart() {
            var maxNumberOfSurvivors = currentMortalityTables.Select(x => x.NumberOfSurvivors).Max();

            if (maxNumberOfSurvivors == null) return;

            maxNumberOfSurvivors = maxNumberOfSurvivors.Value;

            for (int i = 0; i < currentMortalityTables.Count; i++) {
                currentSurvivalFunctions[i].Standart = Math.Round(
                    (double)currentMortalityTables[i]?.NumberOfSurvivors / 
                    (double)maxNumberOfSurvivors
                    , SettingsDictionary.round);

                currentDensitys[i].Standart = Math.Round(
                    (double)currentMortalityTables[i]?.NumberOfDead /
                    (double)maxNumberOfSurvivors
                    , SettingsDictionary.round);
            }
        }

        public void CalculateSTWeibull()
        {
            Weibull weibull = new Weibull(standartValues, ageValues, t, delta, SettingsDictionary.round, r, (double)int.MaxValue, epsilon, FirstAgeX, SecondAgeX);

            Acaici.Weibull = weibull.Quality;
            DistanceFirstMethod.Weibull = weibull.DistanceFirstMethod;
            DistanceSecondMethod.Weibull = weibull.DistanceSecondMethod;


            for (int i = 0; i < currentMortalityTables.Count; i++)
            {
                currentSurvivalFunctions[i].Weibull = weibull.SurvivalFunctions[i];
                currentDensitys[i].Weibull = weibull.Densitys[i];
            }
        }

        public void CalculateSTRelay()
        {
            Relay relay = new Relay(standartValues, ageValues, t, delta, SettingsDictionary.round, r, FirstAgeX, SecondAgeX);

            Acaici.Relay = relay.Quality;
            DistanceFirstMethod.Relay = relay.DistanceFirstMethod;
            DistanceSecondMethod.Relay = relay.DistanceSecondMethod;

            for (int i = 0; i < currentMortalityTables.Count; i++)
            {
                currentSurvivalFunctions[i].Relay = relay.SurvivalFunctions[i];
                currentDensitys[i].Relay = relay.Densitys[i];
            }
        }

        public void CalculateSTGompertz()
        {
            Gompertz gompertz = new Gompertz(standartValues, ageValues, t, delta, SettingsDictionary.round, r, (double)int.MaxValue, epsilon, FirstAgeX, SecondAgeX);

            Acaici.Gompertz = gompertz.Quality;
            DistanceFirstMethod.Gompertz = gompertz.DistanceFirstMethod;
            DistanceSecondMethod.Gompertz = gompertz.DistanceSecondMethod;

            for (int i = 0; i < currentMortalityTables.Count; i++)
            {
                currentSurvivalFunctions[i].Gompertz = gompertz.SurvivalFunctions[i];
                currentDensitys[i].Gompertz = gompertz.Densitys[i];
            }
        }

        public void CalculateSTExponential()
        {
            Exponential exponential = new Exponential(standartValues, ageValues, t, delta, SettingsDictionary.round, r, FirstAgeX, SecondAgeX);

            Acaici.Exponential = exponential.Quality;
            DistanceFirstMethod.Exponential = exponential.DistanceFirstMethod;
            DistanceSecondMethod.Exponential = exponential.DistanceSecondMethod;

            for (int i = 0; i < currentMortalityTables.Count; i++)
            {
                currentSurvivalFunctions[i].Exponential = exponential.SurvivalFunctions[i];
                currentDensitys[i].Exponential = exponential.Densitys[i];
            }
        }

        public void QualityUpdate()
        {
            //Удалим значение из таблицы
            using (var slc = new SQLiteConnection(SQLExecutor.LoadConnectionString))
            {
                slc.Open();
                slc.Execute($"DELETE from {nameof(QualityAssessmentOfModels)}");
            }

            SQLExecutor.InsertExecutor(Acaici, Acaici);
            SQLExecutor.InsertExecutor(DistanceFirstMethod, DistanceFirstMethod);
            SQLExecutor.InsertExecutor(DistanceFirstMethod, DistanceSecondMethod);
            
            SelectQualityAssessmentOfModels();
        }

        #endregion

        #region SurvivalFunctions

        public void RefreshChartsSurvivalFunctions() {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                SurvivalFunctionsCollection = new SeriesCollection();
                var strokeThickness = 2;
                var standart = new LineSeries
                {
                    Title = "Табличное",
                    Values = new ChartValues<double>(currentSurvivalFunctions.Select(x => Math.Round(x.Standart.Value, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                }; 
                SurvivalFunctionsCollection.Add(standart);

                var weibull = new LineSeries
                {
                    Title = "Вейбулл",
                    Values = new ChartValues<double>(currentSurvivalFunctions.Select(x => Math.Round(x.Weibull.Value, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                SurvivalFunctionsCollection.Add(weibull);

                var exponential = new LineSeries
                {
                    Title = "Экспоненциальное",
                    Values = new ChartValues<double>(currentSurvivalFunctions.Select(x => Math.Round(x.Exponential.Value, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                SurvivalFunctionsCollection.Add(exponential);

                var gompertz = new LineSeries
                {
                    Title = "Гомпертц",
                    Values = new ChartValues<double>(currentSurvivalFunctions.Select(x => Math.Round(x.Gompertz.Value, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                SurvivalFunctionsCollection.Add(gompertz);

                var relay = new LineSeries
                {
                    Title = "Рэлея",
                    Values = new ChartValues<double>(currentSurvivalFunctions.Select(x => Math.Round(x.Relay.Value, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                SurvivalFunctionsCollection.Add(relay);


                SurvivalFunctionsYFormatter = value => Math.Round(value, 3).ToString();
            }));

        }
         
        private SeriesCollection survivalFunctionsCollection;

        public SeriesCollection SurvivalFunctionsCollection
        {
            get { return survivalFunctionsCollection; }
            set
            {
                survivalFunctionsCollection = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SurvivalFunctionsCollection)));
            }
        }

        private Func<double, string> survivalFunctionsYFormatter;

        public Func<double, string> SurvivalFunctionsYFormatter
        {
            get { return survivalFunctionsYFormatter; }
            set
            {
                survivalFunctionsYFormatter = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SurvivalFunctionsYFormatter)));
            }
        }

        #endregion

        #region Density

        public void RefreshChartsDensitys()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                DensitysCollection = new SeriesCollection();
                var strokeThickness = 2;
                var standart = new LineSeries
                {
                    Title = "Табличное",
                    Values = new ChartValues<double>(currentDensitys.Select(x => Math.Round(x.Standart.Value, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DensitysCollection.Add(standart);

                var weibull = new LineSeries
                {
                    Title = "Вейбулл",
                    Values = new ChartValues<double>(currentDensitys.Select(x => Math.Round(x.Weibull.Value, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DensitysCollection.Add(weibull);

                var exponential = new LineSeries
                {
                    Title = "Экспоненциальное",
                    Values = new ChartValues<double>(currentDensitys.Select(x => Math.Round(x.Exponential.Value, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DensitysCollection.Add(exponential);

                var gompertz = new LineSeries
                {
                    Title = "Гомпертц",
                    Values = new ChartValues<double>(currentDensitys.Select(x => Math.Round(x.Gompertz.Value, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DensitysCollection.Add(gompertz);

                var relay = new LineSeries
                {
                    Title = "Рэлея",
                    Values = new ChartValues<double>(currentDensitys.Select(x => Math.Round(x.Relay.Value, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DensitysCollection.Add(relay);


                DensitysYFormatter = value => Math.Round(value, 3).ToString();
            }));

        }

        private SeriesCollection densitysCollection;

        public SeriesCollection DensitysCollection
        {
            get { return densitysCollection; }
            set
            {
                densitysCollection = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(DensitysCollection)));
            }
        }

        private Func<double, string> densitysYFormatter;

        public Func<double, string> DensitysYFormatter
        {
            get { return densitysYFormatter; }
            set
            {
                densitysYFormatter = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(DensitysYFormatter)));
            }
        }

        #endregion

        #region Density/SurvivalFunctions

        public void RefreshChartsDivides()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (!currentDensitys.Where(x=>x.Weibull != null).Any() || !currentSurvivalFunctions.Where(x => x.Weibull != null).Any()) return;

                List<Density> divides = new List<Density>();

                for (int i = 0; i < currentDensitys.Count(); i++)
                {
                    divides.Add(new Density()
                    {
                        Standart = currentDensitys[i].Standart / (currentSurvivalFunctions[i].Standart == 0 ? 1 : currentSurvivalFunctions[i].Standart),
                        Weibull = currentDensitys[i].Weibull / (currentSurvivalFunctions[i].Weibull == 0 ? 1 : currentSurvivalFunctions[i].Weibull),
                        Exponential = currentDensitys[i].Exponential / (currentSurvivalFunctions[i].Exponential == 0 ? 1 : currentSurvivalFunctions[i].Exponential),
                        Gompertz = currentDensitys[i].Gompertz / (currentSurvivalFunctions[i].Gompertz == 0 ? 1 : currentSurvivalFunctions[i].Gompertz),
                        Relay = currentDensitys[i].Relay / (currentSurvivalFunctions[i].Relay == 0 ? 1 : currentSurvivalFunctions[i].Relay)
                    });
                }

                DividesCollection = new SeriesCollection();
                var strokeThickness = 2;
                var standart = new LineSeries
                {
                    Title = "Табличное",
                    Values = new ChartValues<double>(divides.Select(x => Math.Round(x.Standart.Value, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DividesCollection.Add(standart);

                var weibull = new LineSeries
                {
                    Title = "Вейбулл",
                    Values = new ChartValues<double>(divides.Select(x => Math.Round(x.Weibull.Value, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DividesCollection.Add(weibull);

                var exponential = new LineSeries
                {
                    Title = "Экспоненциальное",
                    Values = new ChartValues<double>(divides.Select(x => Math.Round(x.Exponential.Value, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DividesCollection.Add(exponential);

                var gompertz = new LineSeries
                {
                    Title = "Гомпертц",
                    Values = new ChartValues<double>(divides.Select(x => Math.Round(x.Gompertz.Value, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DividesCollection.Add(gompertz);

                var relay = new LineSeries
                {
                    Title = "Рэлея",
                    Values = new ChartValues<double>(divides.Select(x => Math.Round(x.Relay.Value, SettingsDictionary.round))),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DividesCollection.Add(relay);


                DensitysYFormatter = value => Math.Round(value, 3).ToString();
            }));

        }

        private SeriesCollection dividesCollection;

        public SeriesCollection DividesCollection
        {
            get { return dividesCollection; }
            set
            {

                dividesCollection = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(DividesCollection)));
            }
        }

        private Func<double, string> dividesYFormatter;

        public Func<double, string> DividesYFormatter
        {
            get { return dividesYFormatter; }
            set
            {
                dividesYFormatter = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(DividesYFormatter)));
            }
        }

        #endregion

    }
}

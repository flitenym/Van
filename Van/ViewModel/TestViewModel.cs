using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Van.Helper;
using static Van.Helper.Helper;
using System;
using Van.Methods;
using System.Threading.Tasks;
using LiveCharts;
using Van.DataBase.Models;
using Van.DataBase;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Windows.Media;
using LiveCharts.Wpf;
using System.Windows;
using Dapper;
using System.Threading;

namespace Van.ViewModel
{
    class TestViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public TestViewModel()
        {
            Loading(true);

            Thread thread = new Thread(LoadTables);
            thread.Start();

            Loading(false);
        }

        public List<int> t = new List<int>();

        public List<int> delta = new List<int>();

        double r => delta.Where(x => x == 1).Count();

        public Random random = new Random();

        public bool HaveNullProbability = true;

        public double epsilon = 0.01;

        public int round = 5;

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
            QualityAssessmentOfModelsTableData = SQLExecutor.SelectExecutor(nameof(QualityAssessmentOfModels));
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
                MortalityTableData = SQLExecutor.SelectExecutor(nameof(MortalityTable));
                MortalityTableData.AcceptChanges();
            }

            currentMortalityTables = SQLExecutor.Select<MortalityTable>($"SELECT * FROM {nameof(MortalityTable)}").ToList();

            NValue = currentMortalityTables.Select(x => x.NumberOfSurvivors).Max().Value;
            if (currentMortalityTables.Where(x => x.Probability == null).Any())
                HaveNullProbability = true;
            else HaveNullProbability = false;
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
                SurvivalFunctionTable = SQLExecutor.SelectExecutor(nameof(SurvivalFunction));
                SurvivalFunctionTable.AcceptChanges();
            }

            currentSurvivalFunctions = SQLExecutor.Select<SurvivalFunction>($"SELECT * FROM {nameof(SurvivalFunction)}").ToList();

            if (!currentSurvivalFunctions.Where(x => x.Standart == null).Any())
            {
                RefreshChartsSurvivalFunctions();
            }
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
                DensityTable = SQLExecutor.SelectExecutor(nameof(Density));
                DensityTable.AcceptChanges();
            }

            currentDensitys = SQLExecutor.Select<Density>($"SELECT * FROM {nameof(Density)}").ToList();

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
                LifeTimesTable = SQLExecutor.SelectExecutor(nameof(LifeTimes));
                LifeTimesTable.AcceptChanges();
            }

            currentLifeTimes = SQLExecutor.Select<LifeTimes>($"SELECT * FROM {nameof(LifeTimes)}").ToList();

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
            Loading(true);
            t = new List<int>(); 

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            GenerateT();
            
            RewriteLifeTimesTable();
            
            SelectLifeTimesFunction(); 
             
            Message($"t вычислено. Время: {stopwatch.Elapsed.TotalSeconds.ToString()}");
            stopwatch.Stop();

            Loading(false);
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
            Loading(true); 
            
            //обновим таблицу через БД
            SelectMortality(false);

            if (currentMortalityTables.Count() < 2)
            {
                SelectMortality();
                Message("Слишком мало данных");
                Loading(false);
                return;
            }

            CalculateNumberOfDead();

            CalculateProbability();

            //Обновим в БД данные исходя из текущего списка
            UpdateMortality(); 
            //обновим таблицу через БД
            SelectMortality();

            Message("Вычисление прошло успешно");
            Loading(false);
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
            Loading(true);

            //обновим таблицу через БД
            SelectMortality();

            Message("Таблица обновлена");
            Loading(false);
        }


        private void UpdateMortality()
        {
            for (int i = 0; i < currentMortalityTables.Count(); i++)
            {
                SQLExecutor.UpdateExecutor(nameof(MortalityTable), currentMortalityTables[i], currentMortalityTables[i].ID);
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
                SQLExecutor.UpdateExecutor(nameof(SurvivalFunction), currentSurvivalFunctions[i], currentSurvivalFunctions[i].ID);
            }
        }

        private void UpdateD()
        {
            for (int i = 0; i < currentDensitys.Count(); i++)
            {
                SQLExecutor.UpdateExecutor(nameof(Density), currentDensitys[i], currentDensitys[i].ID);
            }
        }

        public async Task CalculateMethodsAsync()
        {
            Loading(true); 

            var stopwatch = new Stopwatch();
            stopwatch.Start();
             
            Acaici = new QualityAssessmentOfModels() { Quality = "Акаики"};
             
            DistanceFirstMethod = new QualityAssessmentOfModels() { Quality = "Расстояние первый метод" };
            DistanceSecondMethod = new QualityAssessmentOfModels() { Quality = "Расстояние второй метод" };
            
            var taskStandart = new Task(CalculateSTStandart);
            var taskWeibull = new Task(CalculateSTWeibull);
            var taskRelay = new Task(CalculateSTRelay);
            var taskGompertz = new Task(CalculateSTGompertz);
            var taskExponential = new Task(CalculateSTExponential);

            taskStandart.Start();
            taskWeibull.Start();
            taskRelay.Start();
            taskGompertz.Start();
            taskExponential.Start();

            await Task.WhenAll(
                taskStandart, 
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


            Message($"Время: {stopwatch.Elapsed.TotalSeconds.ToString()}");
            stopwatch.Stop();

            Loading(false);
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
                    , round);

                currentDensitys[i].Standart = Math.Round(
                    0.0
                    , round);
            }
        }

        public void CalculateSTWeibull()
        { 
            Weibull weibull = new Weibull(t, delta, r, (double)int.MaxValue, epsilon);

            for (int i = 0; i < currentMortalityTables.Count; i++)
            {
                currentSurvivalFunctions[i].Weibull = Math.Round(
                    Math.Exp(
                    -weibull.lambda * Math.Pow(getTValue(currentMortalityTables[i]?.AgeX), weibull.gamma)
                    )
                    , round);

                currentDensitys[i].Weibull = Math.Round(
                    weibull.lambda * 
                    weibull.gamma * 
                    Math.Pow(getTValue(currentMortalityTables[i]?.AgeX), weibull.gamma - 1) *
                    Math.Exp(
                    -weibull.lambda * Math.Pow(getTValue(currentMortalityTables[i]?.AgeX), weibull.gamma)
                    )
                    , round);
            }

            Acaici.Weibull = GetQuality(weibull.LValue, 2, t.Count());

            DistanceWeibull();
        }

        public void CalculateSTRelay()
        { 
            Relay relay = new Relay(t, delta, r);

            for (int i = 0; i < currentMortalityTables.Count; i++)
            {
                currentSurvivalFunctions[i].Relay = Math.Round(
                    Math.Exp(
                            - Math.Pow(getTValue(currentMortalityTables[i]?.AgeX), 2) /
                            (2 * Math.Pow(relay.lambda, 2))
                        )
                    , round);

                currentDensitys[i].Relay = Math.Round(
                    getTValue(currentMortalityTables[i]?.AgeX) *
                    Math.Exp(
                            -Math.Pow(getTValue(currentMortalityTables[i]?.AgeX), 2) /
                            (2 * Math.Pow(relay.lambda, 2))
                        ) / Math.Pow(relay.lambda, 2)
                    , round);

            }

            Acaici.Relay = GetQuality(relay.LValue, 1, t.Count());

            DistanceRelay();
        }

        public void CalculateSTGompertz()
        {
            Gompertz gompertz = new Gompertz(t, delta, r, (double)int.MaxValue, epsilon);

            for (int i = 0; i < currentMortalityTables.Count; i++)
            {
                currentSurvivalFunctions[i].Gompertz = Math.Round(
                    Math.Exp(
                    gompertz.lambda /
                    gompertz.alpha *
                    (
                        1 -
                        Math.Exp(
                                    gompertz.alpha *
                                    getTValue(currentMortalityTables[i]?.AgeX)
                                )
                    )
                    )
                    , round);

                currentDensitys[i].Gompertz = Math.Round(
                    gompertz.lambda *
                    Math.Exp(
                                    gompertz.alpha *
                                    getTValue(currentMortalityTables[i]?.AgeX)
                                ) *
                    Math.Exp(
                    gompertz.lambda /
                    gompertz.alpha *
                    (
                        1 -
                        Math.Exp(
                                    gompertz.alpha *
                                    getTValue(currentMortalityTables[i]?.AgeX)
                                )
                    )
                    )
                    , round);
            }

            Acaici.Gompertz = GetQuality(gompertz.LValue, 2, t.Count());

            DistanceGompertz();
        }

        public void CalculateSTExponential()
        { 
            Exponential exponential = new Exponential(t, delta, r);

            for (int i = 0; i < currentMortalityTables.Count; i++)
            {
                currentSurvivalFunctions[i].Exponential = Math.Round(
                    Math.Exp(
                                -exponential.lambda *
                                getTValue(currentMortalityTables[i]?.AgeX)
                            )
                    , round);

                currentDensitys[i].Exponential = Math.Round(
                    exponential.lambda *
                    Math.Exp(
                                -exponential.lambda *
                                getTValue(currentMortalityTables[i]?.AgeX)
                            )
                    , round);
            }

            Acaici.Exponential = GetQuality(exponential.LValue, 1, t.Count());

            DistanceExponential();
        }

        public double GetDistanceFirst(double first, double second) {
            return Math.Abs(first - second);
        }

        public double GetDistanceSecond(double first, double second)
        {
            return Math.Pow(first - second, 2);
        }

        public void DistanceWeibull()
        {
            double sumFirst = 0;
            double sumSecond = 0;

            for (int i = 0; i < currentSurvivalFunctions.Count(); i++)
            {
                sumFirst += GetDistanceFirst((double)currentSurvivalFunctions[i].Standart, (double)currentSurvivalFunctions[i].Weibull);
                sumSecond += GetDistanceSecond((double)currentSurvivalFunctions[i].Standart, (double)currentSurvivalFunctions[i].Weibull);
            }

            DistanceFirstMethod.Weibull = sumFirst;
            DistanceSecondMethod.Weibull = Math.Sqrt(sumSecond);
        }

        public void DistanceRelay()
        {
            double sumFirst = 0;
            double sumSecond = 0;

            for (int i = 0; i < currentSurvivalFunctions.Count(); i++)
            {
                sumFirst += GetDistanceFirst((double)currentSurvivalFunctions[i].Standart, (double)currentSurvivalFunctions[i].Relay);
                sumSecond += GetDistanceSecond((double)currentSurvivalFunctions[i].Standart, (double)currentSurvivalFunctions[i].Relay);
            }

            DistanceFirstMethod.Relay = sumFirst;
            DistanceSecondMethod.Relay = Math.Sqrt(sumSecond);
        }

        public void DistanceGompertz()
        {
            double sumFirst = 0;
            double sumSecond = 0;

            for (int i = 0; i < currentSurvivalFunctions.Count(); i++)
            {
                sumFirst += GetDistanceFirst((double)currentSurvivalFunctions[i].Standart, (double)currentSurvivalFunctions[i].Gompertz);
                sumSecond += GetDistanceSecond((double)currentSurvivalFunctions[i].Standart, (double)currentSurvivalFunctions[i].Gompertz);
            }

            DistanceFirstMethod.Gompertz = sumFirst;
            DistanceSecondMethod.Gompertz = Math.Sqrt(sumSecond);
        }

        public void DistanceExponential()
        {
            double sumFirst = 0;
            double sumSecond = 0;

            for (int i = 0; i < currentSurvivalFunctions.Count(); i++)
            {
                sumFirst += GetDistanceFirst((double)currentSurvivalFunctions[i].Standart, (double)currentSurvivalFunctions[i].Exponential);
                sumSecond += GetDistanceSecond((double)currentSurvivalFunctions[i].Standart, (double)currentSurvivalFunctions[i].Exponential);
            }

            DistanceFirstMethod.Exponential = sumFirst;
            DistanceSecondMethod.Exponential = Math.Sqrt(sumSecond);
        } 

        public double GetQuality(double LValue, int k, int n)
        {
            return -2.0 * LValue / n + 2.0 * k / n;
        }

        public void QualityUpdate()
        {
            //Удалим значение из таблицы
            using (var slc = new SQLiteConnection(SQLExecutor.LoadConnectionString))
            {
                slc.Open();
                slc.Execute($"DELETE from {nameof(QualityAssessmentOfModels)}");
            }

            SQLExecutor.InsertExecutor(nameof(QualityAssessmentOfModels), Acaici);
            SQLExecutor.InsertExecutor(nameof(QualityAssessmentOfModels), DistanceFirstMethod);
            SQLExecutor.InsertExecutor(nameof(QualityAssessmentOfModels), DistanceSecondMethod);
            
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
                    Title = "Стандартное",
                    Values = new ChartValues<double>(currentSurvivalFunctions.Select(x => x.Standart.Value)),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                }; 
                SurvivalFunctionsCollection.Add(standart);

                var weibull = new LineSeries
                {
                    Title = "Вейбулл",
                    Values = new ChartValues<double>(currentSurvivalFunctions.Select(x => x.Weibull.Value)),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                SurvivalFunctionsCollection.Add(weibull);

                var exponential = new LineSeries
                {
                    Title = "Экспоненциальное",
                    Values = new ChartValues<double>(currentSurvivalFunctions.Select(x => x.Exponential.Value)),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                SurvivalFunctionsCollection.Add(exponential);

                var gompertz = new LineSeries
                {
                    Title = "Гомпертц",
                    Values = new ChartValues<double>(currentSurvivalFunctions.Select(x => x.Gompertz.Value)),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                SurvivalFunctionsCollection.Add(gompertz);

                var relay = new LineSeries
                {
                    Title = "Рэлея",
                    Values = new ChartValues<double>(currentSurvivalFunctions.Select(x => x.Relay.Value)),
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
                    Title = "Стандартное",
                    Values = new ChartValues<double>(currentDensitys.Select(x => x.Standart.Value)),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DensitysCollection.Add(standart);

                var weibull = new LineSeries
                {
                    Title = "Вейбулл",
                    Values = new ChartValues<double>(currentDensitys.Select(x => x.Weibull.Value)),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DensitysCollection.Add(weibull);

                var exponential = new LineSeries
                {
                    Title = "Экспоненциальное",
                    Values = new ChartValues<double>(currentDensitys.Select(x => x.Exponential.Value)),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DensitysCollection.Add(exponential);

                var gompertz = new LineSeries
                {
                    Title = "Гомпертц",
                    Values = new ChartValues<double>(currentDensitys.Select(x => x.Gompertz.Value)),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DensitysCollection.Add(gompertz);

                var relay = new LineSeries
                {
                    Title = "Рэлея",
                    Values = new ChartValues<double>(currentDensitys.Select(x => x.Relay.Value)),
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
                    Title = "Стандартное",
                    Values = new ChartValues<double>(divides.Select(x => x.Standart.Value)),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DividesCollection.Add(standart);

                var weibull = new LineSeries
                {
                    Title = "Вейбулл",
                    Values = new ChartValues<double>(divides.Select(x => x.Weibull.Value)),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DividesCollection.Add(weibull);

                var exponential = new LineSeries
                {
                    Title = "Экспоненциальное",
                    Values = new ChartValues<double>(divides.Select(x => x.Exponential.Value)),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DividesCollection.Add(exponential);

                var gompertz = new LineSeries
                {
                    Title = "Гомпертц",
                    Values = new ChartValues<double>(divides.Select(x => x.Gompertz.Value)),
                    Fill = Brushes.Transparent,
                    StrokeThickness = strokeThickness,
                    PointGeometry = null
                };
                DividesCollection.Add(gompertz);

                var relay = new LineSeries
                {
                    Title = "Рэлея",
                    Values = new ChartValues<double>(divides.Select(x => x.Relay.Value)),
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

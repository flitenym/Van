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

namespace Van.ViewModel
{
    class TestViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public TestViewModel()
        {
            Loading(true);

            Task.Factory.StartNew(() =>
                SelectMortality()
            );

            Task.Factory.StartNew(() =>
               SelectSurvivalFunction()
            );

            Task.Factory.StartNew(() =>
               SelectLifeTimesFunction()
            ); 

            Loading(false);
        }

        public List<int> t = new List<int>();

        public List<int> delta = new List<int>();

        public Random random = new Random();

        public bool HaveNullProbability = true;

        public double epsilon = 0.01;

        public List<MortalityTable> currentMortalityTables = new List<MortalityTable>();

        public List<SurvivalFunction> currentSurvivalFunctions = new List<SurvivalFunction>();

        public List<LifeTimes> currentLifeTimes = new List<LifeTimes>();


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
                Task.Factory.StartNew(() =>
                   SelectLifeTimesFunction()
                );

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
                RefreshCharts();
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

        #region Weibull

        private void Weibull() { 
            Weibull weibull = new Weibull(t, delta, (double)int.MaxValue, epsilon);
            WeibullValue = weibull.lambda(); 
        } 

        private double weibullValue = 0;

        public double WeibullValue
        {
            get { return weibullValue; }
            set
            {
                weibullValue = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(WeibullValue)));
            }
        }

        #endregion

        #region Exponential

        private void Exponential()
        { 
            Exponential exponential = new Exponential(t, delta);
            ExponentialValue = exponential.lambda(); 
        } 

        private double exponentialValue = 0;

        public double ExponentialValue
        {
            get { return exponentialValue; }
            set
            {
                exponentialValue = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ExponentialValue)));
            }
        }

        #endregion

        #region Relay

        private void Relay()
        { 
            Relay relay = new Relay(t, delta);
            RelayValue = relay.lambda(); 
        } 

        private double relayValue = 0;

        public double RelayValue
        {
            get { return relayValue; }
            set
            {
                relayValue = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(RelayValue)));
            }
        }

        #endregion

        #region Gompertz

        private void Gompertz()
        { 
            Gompertz gompertz = new Gompertz(t, delta, (double)int.MaxValue, epsilon);
            GompertzValue = gompertz.lambda(); 
        } 

        private double gompertzValue = 0;

        public double GompertzValue
        {
            get { return gompertzValue; }
            set
            {
                gompertzValue = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(GompertzValue)));
            }
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
                        t.Add(getTValue(currentMortalityTables[j].AgeX));
                        break;
                    }
                    else if (j != 0 && z >= sumProbubility && z < sumProbubility + currentMortalityTables[j].Probability.Value)
                    {
                        t.Add(getTValue(currentMortalityTables[j].AgeX));
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

            foreach (var tValue in t)
            {
                currentLifeTimes.Add(new LifeTimes() { LifeTime = tValue, Censor = 1 });
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
                        foreach (var currentLifeTime in currentLifeTimes)
                        {
                            cmd.Parameters.AddWithValue("@LifeTime", currentLifeTime.LifeTime);
                            cmd.Parameters.AddWithValue("@Censor", currentLifeTime.Censor);
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
        public int getTValue(string ageX) {
            // в AgeX может быть лишние значение такие как + и т.д., поэтому спарсим только числа
            if (int.TryParse(string.Join("", ageX.Where(c => char.IsDigit(c))), out int value))
            { 
                return value;
            }
            return 0;
        }

        #endregion

        #region Комманда для вычисления параметров

        private RelayCommand calculateCommand;
        public RelayCommand CalculateCommand
        {
            get
            {
                return calculateCommand ??
                  (calculateCommand = new RelayCommand(x =>
                  {
                      Task.Factory.StartNew(() =>
                            CalculateParametrs()
                      );
                  }));
            }
        }

        private void CalculateParametrs()
        {
            Loading(true);
            Gompertz();
            Relay();
            Exponential();
            Weibull();
            Message("Параметры вычислены");
            Loading(false);
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
            foreach (var mortalityTable in currentMortalityTables)
            {
                mortalityTable.Probability = (double?)mortalityTable.NumberOfDead / (double)currentMortalityTables.FirstOrDefault()?.NumberOfSurvivors;
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


        private void UpdateMortality() {
            foreach (var MortalityTable in currentMortalityTables)
            {
                SQLExecutor.UpdateExecutor(nameof(MortalityTable), MortalityTable, MortalityTable.ID);
            }
        }

        #endregion

        #region Комманда для вычисления s(t)

        private RelayCommand calculateSTCommand;
        public RelayCommand CalculateSTCommand
        {
            get
            {
                return calculateSTCommand ??
                  (calculateSTCommand = new RelayCommand(x =>
                  {
                      Task.Factory.StartNew(() =>
                          CalculateST()
                      );
                  }, CanCalculateST));
            }
        }

        private bool CanCalculateST(object x)
        {
            return currentSurvivalFunctions.Any() && currentSurvivalFunctions.Count == currentMortalityTables.Count;
        }

        private void UpdateST()
        {
            foreach (var SurvivalFunction in currentSurvivalFunctions)
            {
                SQLExecutor.UpdateExecutor(nameof(SurvivalFunction), SurvivalFunction, SurvivalFunction.ID);
            }
        }

        public void CalculateST()
        {
            Loading(true); 

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            CalculateSTStandart();

            //Обновим в БД данные исходя из текущего списка
            UpdateST();

            //обновим таблицу через БД
            SelectSurvivalFunction();

            Message($"s(t) вычислено. Время: {stopwatch.Elapsed.TotalSeconds.ToString()}");
            stopwatch.Stop();

            Loading(false);
        }

        public void CalculateSTStandart() {
            var maxNumberOfSurvivors = currentMortalityTables.Select(x => x.NumberOfSurvivors).Max();

            if (maxNumberOfSurvivors == null) return;

            maxNumberOfSurvivors = maxNumberOfSurvivors.Value;

            for (int i = 0; i < currentMortalityTables.Count; i++) {
                currentSurvivalFunctions[i].Standart = (double)currentMortalityTables[i]?.NumberOfSurvivors / (double)maxNumberOfSurvivors;
            }
        }

        #endregion
         
        public void RefreshCharts() {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                SeriesCollection = new SeriesCollection();

                var series = new LineSeries
                {
                    Title = "Стандартное",
                    Values = new ChartValues<double>(currentSurvivalFunctions.Select(x => x.Standart.Value)),
                    Fill = Brushes.Transparent,
                    StrokeThickness = 1,
                    PointGeometry = null //use a null geometry when you have many series
                };

                SeriesCollection.Add(series);

                YFormatter = value => Math.Round(value, 3).ToString();
            }));

        }
         
        private SeriesCollection seriesCollection;

        public SeriesCollection SeriesCollection
        {
            get { return seriesCollection; }
            set
            {
                seriesCollection = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SeriesCollection)));
            }
        }

        private Func<double, string> yFormatter;

        public Func<double, string> YFormatter
        {
            get { return yFormatter; }
            set
            {
                yFormatter = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(YFormatter)));
            }
        }

    }
}

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

            Loading(false);
        }

        public List<double> t = new List<double>() { 2, 2, 3, 4, 8, 4, 7, 9, 2, 4 };

        public List<double> delta = new List<double>() { 0, 1, 0, 1, 1, 1, 1, 0, 1, 0 }; 

        public double epsilon = 0.01;
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public List<MortalityTable> currentMortalityTables = new List<MortalityTable>();

        public List<SurvivalFunction> currentSurvivalFunctions = new List<SurvivalFunction>();

        #region Таблица смертности

        private DataTable mortalityTableData;

        public DataTable MortalityTableData
        {
            get { return mortalityTableData; }
            set
            {
                if (value == null) return;
                mortalityTableData = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(MortalityTableData)));
            }
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

        private void SelectMortality(bool needTableRefresh = true)
        {
            if (needTableRefresh)
            {
                MortalityTableData = SQLExecutor.SelectExecutor(nameof(MortalityTable));
                MortalityTableData.AcceptChanges();
            }

            currentMortalityTables = SQLExecutor.Select<MortalityTable>($"SELECT * FROM {nameof(MortalityTable)}").ToList(); 
        }

        #endregion

        #region Функция выживания s(t)

        private DataTable survivalFunctionTable;

        public DataTable SurvivalFunctionTable
        {
            get { return survivalFunctionTable; }
            set
            {
                if (value == null) return;
                survivalFunctionTable = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SurvivalFunctionTable)));
            }
        }

        #endregion

        private void SelectSurvivalFunction(bool needTableRefresh = true)
        {
            if (needTableRefresh)
            {
                SurvivalFunctionTable = SQLExecutor.SelectExecutor(nameof(SurvivalFunction));
                SurvivalFunctionTable.AcceptChanges();
            }

            currentSurvivalFunctions = SQLExecutor.Select<SurvivalFunction>($"SELECT * FROM {nameof(SurvivalFunction)}").ToList();
        }

    }
}

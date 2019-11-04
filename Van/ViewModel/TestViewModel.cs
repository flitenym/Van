using Van.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Van.Helper;
using System.Runtime.CompilerServices;
using static Van.Helper.Helper;
using System;
using Van.Methods;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media;
using System.Collections.ObjectModel;
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
                    Select()
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

        #region Таблица смертности

        private DataTable mortalityTable;

        public DataTable MortalityTable
        {
            get { return mortalityTable; }
            set
            {
                if (value == null) return;
                mortalityTable = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(MortalityTable)));
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
                            CalculateData()
                      );
                  }));
            }
        }

        public void CalculateData()
        {
            Loading(true); 
            
            //обновим таблицу через БД
            Select(false);

            if (currentMortalityTables.Count() < 2)
            {
                Select();
                Message("Слишком мало данных");
                Loading(false);
                return;
            }

            CalculateNumberOfDead();

            CalculateProbability();

            //Обновим в БД данные исходя из текущего списка
            Update(); 
            //обновим таблицу через БД
            Select();

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
                          UpdateTable()
                      );
                  }));
            }
        }

        public void UpdateTable()
        {
            Loading(true);

            //обновим таблицу через БД
            Select();

            Message("Таблица обновлена");
            Loading(false);
        }


        private void Update() {
            foreach (var MortalityTable in currentMortalityTables)
            {
                SQLExecutor.UpdateExecutor(nameof(MortalityTable), MortalityTable, MortalityTable.ID);
            }
        }

        private void Select(bool needTableRefresh = true)
        {
            if (needTableRefresh)
            {
                MortalityTable = SQLExecutor.SelectExecutor(nameof(MortalityTable));
                MortalityTable.AcceptChanges();
            }

            currentMortalityTables = SQLExecutor.Select<MortalityTable>($"SELECT * FROM {nameof(MortalityTable)}").ToList(); 
        }

        #endregion

    }
}

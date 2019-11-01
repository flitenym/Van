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

        public List<double> t = new List<double>() { 2, 2, 3, 4, 8, 4, 7, 9, 2, 4 };
        public List<double> delta = new List<double>() { 0, 1, 0, 1, 1, 1, 1, 0, 1, 0 }; 
        public double epsilon = 0.01;

        #region Weibull

        public void Weibull() { 
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

        public void Exponential()
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

        public void Relay()
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

        public void Gompertz()
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

        public void CalculateParametrs()
        {
            Loading(true);
            Gompertz();
            Relay();
            Exponential();
            Weibull();
            Loading(false);
        }

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

        private RelayCommand updateMortalityCommand;
        public RelayCommand UpdateMortalityCommand
        {
            get
            {
                return updateMortalityCommand ??
                  (updateMortalityCommand = new RelayCommand(x =>
                  {
                      SelectTable();
                  }));
            }
        } 

        public TestViewModel()
        {
            SelectTable();
        }

        private void SelectTable() {
            Task.Factory.StartNew(() =>
                    Select()
            );
        }

        private void Select()
        {
            Loading(true);
            MortalityTable = SQLExecutor.SelectExecutor(nameof(MortalityTable));
            MortalityTable.AcceptChanges();
            Loading(false);
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

    }
}

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
            Loading(true); 
            Weibull weibull = new Weibull(t, delta, (double)int.MaxValue, epsilon);
            WeibullValue = weibull.lambda();
            Loading(false);
        }

        private RelayCommand calculateWeibullCommand;
        public RelayCommand CalculateWeibullCommand
        {
            get
            {
                return calculateWeibullCommand ??
                  (calculateWeibullCommand = new RelayCommand(x =>
                  {
                        Task.Factory.StartNew(() =>
                              Weibull()
                        ); 
                  }));
            }
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
            Loading(true); 
            Exponential exponential = new Exponential(t, delta);
            ExponentialValue = exponential.lambda();
            Loading(false);
        }

        private RelayCommand calculateExponentialCommand;
        public RelayCommand CalculateExponentialCommand
        {
            get
            {
                return calculateExponentialCommand ??
                  (calculateExponentialCommand = new RelayCommand(x =>
                  {
                      Task.Factory.StartNew(() =>
                            Exponential()
                      );
                  }));
            }
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
            Loading(true);
            
            Relay relay = new Relay(t, delta);
            RelayValue = relay.lambda();
            Loading(false);
        }

        private RelayCommand calculateRelayCommand;
        public RelayCommand CalculateRelayCommand
        {
            get
            {
                return calculateRelayCommand ??
                  (calculateRelayCommand = new RelayCommand(x =>
                  {
                      Task.Factory.StartNew(() =>
                            Relay()
                      );
                  }));
            }
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
            Loading(true);  
            Gompertz gompertz = new Gompertz(t, delta, (double)int.MaxValue, epsilon);
            GompertzValue = gompertz.lambda();
            Loading(false);
        }

        private RelayCommand calculateGompertzCommand;
        public RelayCommand CalculateGompertzCommand
        {
            get
            {
                return calculateGompertzCommand ??
                  (calculateGompertzCommand = new RelayCommand(x =>
                  {
                      Task.Factory.StartNew(() =>
                            Gompertz()
                      );
                  }));
            }
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

    }
}

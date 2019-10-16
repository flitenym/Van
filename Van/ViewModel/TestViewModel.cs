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

namespace Van.ViewModel
{
    class TestViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void VeibullaDistribution()
        {
            double r = 5.0;
            double gamma = 5.0;

            var t = new List<double>();
            var delta = new List<double>();

            int n = t.Count();

            double first = r / gamma;

            double second = 0.0;

            for (int i = 0; i < n; i++)
            {
                second += Math.Log(t[i]) * delta[i];
            }

            double third = 0.0;

            for (int i = 0; i < n; i++)
            {
                third += Math.Pow(t[i], gamma);
            }

            double fourth = 0.0;

            for (int i = 0; i < n; i++)
            {
                fourth += Math.Pow(t[i], gamma) * Math.Log(t[i]);
            }

            double five = (r / third) * fourth;

            double six = first + second - five;
        }

        public double ExpDistribution()
        {
            double r = 5.0; 

            var t = new List<double>();

            return  r / t.Sum(); 
        }




    } 
}

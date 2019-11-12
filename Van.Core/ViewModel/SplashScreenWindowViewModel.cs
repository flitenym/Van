﻿using System.ComponentModel;
using System.Reflection;

namespace Van.Core.ViewModel
{
    class SplashScreenWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public string AppVersion => "Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString();  
    }
}
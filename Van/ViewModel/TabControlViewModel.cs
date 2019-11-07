﻿using System.ComponentModel;
using System;
using Van.AbstractClasses;

namespace Van.ViewModel
{
    class TabControlViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public string Name { get; set; }

        public Guid ID { get; set; }

        public object ViewContent { get; set; }

        public ModuleBase ModuleBaseItem { get; set; }


        private bool isSelected = false; 
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }
    }
}

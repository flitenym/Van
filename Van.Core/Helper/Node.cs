using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Van.Core.AbstractClasses;

namespace Van.Core.Helper
{
    public class Node : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public string Name { get; set; }
        public Guid ID { get; set; }
        public ObservableCollection<Node> Nodes { get; set; }
        public string ParentName { get; set; }
        public ModuleBase View { get; set; }

        private bool selected = false;
        public bool Selected
        {
            get { return selected; }
            set
            { 
                selected = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Selected)));
            }
        }

        private bool expanded = false;
        public bool Expanded
        {
            get { return expanded; }
            set
            {
                expanded = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Expanded)));
            }
        }

    }
}

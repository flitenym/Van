using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Van.Interfaces;

namespace Van.Helper
{
    public class Node
    {
        public string Name { get; set; }
        public Guid ID { get; set; }
        public ObservableCollection<Node> Nodes { get; set; }
        public string ParentName { get; set; }
        public IModule View { get; set; }
    }
}

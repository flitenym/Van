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
    public class OperationData
    {
        public OperationData(string name, string title) {
            this.Name = name;
            this.Title = title;
        }
        public string Name { get; set; }
        public string Title { get; set; } 
    }
}

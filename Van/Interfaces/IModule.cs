using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using static Van.Helper.Enums;

namespace Van.Interfaces
{
    public interface IModule
    {
        string Name { get; }
        int Num { get; }
        Guid ID { get; }
        Guid? ParentID { get; }
        ModelBaseClasses modelClass { get; } 
        UserControl UserInterface { get; }
        void Deactivate();
    }
}

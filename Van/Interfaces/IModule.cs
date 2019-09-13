using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Van.Interfaces
{
    public interface IModule
    {
        string Name { get; }
        int Num { get; }
        int IdType { get; }
        UserControl UserInterface { get; }
        void Deactivate();
    }
}

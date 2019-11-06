using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Van.Interfaces;
using Van.ViewModel;
using static Van.Helper.Helper;


namespace Van.View
{ 
    public partial class DistributionView : UserControl, IDisposable
    {
        public DistributionView()
        {
            InitializeComponent();
        }

        public void Dispose()
        {

        }
    }
}

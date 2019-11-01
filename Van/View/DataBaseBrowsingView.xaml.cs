using System.Data;
using System.Windows;
using System.Windows.Controls;


namespace Van.View
{
    public partial class DataBaseBrowsingView : UserControl
    {
        public DataBaseBrowsingView()
        {
            InitializeComponent();
            DataGrid.AutoGeneratingColumn += Helper.Helper.DataGrid_AutoGeneratingColumn;
        }

        

    }
}

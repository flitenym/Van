using System.Windows.Controls;
using static Van.Helper.Helper;


namespace Van.View
{
    public partial class TestView : UserControl
    {
        public TestView()
        {
            InitializeComponent();
            MortalityTableDataGrid.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
            SurvivalFunctionTableDataGrid.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
        } 
    }
}

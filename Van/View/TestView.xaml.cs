using System;
using System.Windows.Controls;
using static Van.Helper.Helper;


namespace Van.View
{
    public partial class TestView : UserControl, IDisposable
    {
        public TestView()
        {
            InitializeComponent();
            MortalityTableDataGrid.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
            SurvivalFunctionTableDataGrid.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
            LifeTimesTableDataGrid.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
        }

        public void Dispose()
        {
            MortalityTableDataGrid.AutoGeneratingColumn -= DataGrid_AutoGeneratingColumn;
            SurvivalFunctionTableDataGrid.AutoGeneratingColumn -= DataGrid_AutoGeneratingColumn;
            LifeTimesTableDataGrid.AutoGeneratingColumn -= DataGrid_AutoGeneratingColumn;
        }
    }
}

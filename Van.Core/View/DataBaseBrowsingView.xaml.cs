using System;
using System.Windows.Controls;

namespace Van.Core.View
{
    /// <summary>
    /// Логика взаимодействия для DataBaseBrowsingView.xaml
    /// </summary>
    public partial class DataBaseBrowsingView : UserControl, IDisposable
    {
        public DataBaseBrowsingView()
        {
            InitializeComponent();
            DataGrid.AutoGeneratingColumn += Helper.Helper.DataGrid_AutoGeneratingColumn;
        }

        public void Dispose()
        {
            DataGrid.AutoGeneratingColumn -= Helper.Helper.DataGrid_AutoGeneratingColumn;
        }
    }
}

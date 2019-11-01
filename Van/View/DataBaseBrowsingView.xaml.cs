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
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        { 
            if (e.PropertyName == "ID" || e.PropertyName == "Title")
            {
                e.Column.Visibility = Visibility.Collapsed;
            }

            var dGrid = (sender as DataGrid);
            if (dGrid == null) return;
            var view = dGrid.ItemsSource as DataView;
            if (view == null) return;
            var table = view.Table;
            e.Column.Header = table.Columns[e.Column.Header as string].Caption;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Van.ViewModel;

namespace Van.Helper
{
    public static class Helper
    {
        public static async void Loading(bool isLoading)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
             {
                 MainWindowViewModel win = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
                 win.IsLoadingPanelVisible = isLoading;
             }));
        }

        /// <summary>
        /// Отправить сообщение через SnackBar
        /// </summary>
        /// <param name="content">Сообщение</param>
        /// <param name="isNoDuplicateConsider">Если true и будет дубликаты сообщений, то они каждый все равно вызовет новое уведомление, если false то выйдет повторное сообщение 1 раз</param>
        public static async void Message(string content, bool isNoDuplicateConsider = false)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                MainWindowViewModel win = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
                Task.Factory.StartNew(() => win.IsMessagePanelContent.Enqueue(
                content,
                "OK",
                param => Trace.WriteLine("Actioned: " + param),
                null,
                false,
                isNoDuplicateConsider)
            );
            }));
        }

        /// <summary>
        /// Логирование
        /// </summary>
        public static async void LogMessage(string name, string title)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                OperationViewModel operationViewModel = (OperationViewModel)Activator.CreateInstance(StaticReflectionHelper.GetClassByName(nameof(OperationViewModel)));
                operationViewModel.OperationsData = new ObservableCollection<OperationData>() { new OperationData(name, title) }; 
            }));
        }

        public static void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
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

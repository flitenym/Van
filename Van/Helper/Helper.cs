using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

    }
}

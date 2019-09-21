using CustomControls;
using System.Diagnostics;
using System.Windows;
using Van.Interfaces;
using Van.ViewModel;

namespace Van.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView : WindowControl, IMainWindowView
    {
        public MainWindowView()
        {
            InitializeComponent();
        }

        public void SnackBar()
        {
            MainWindowViewModel win = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            Snackbar.MessageQueue.Enqueue(
                win.IsMessagePanelContent,
                "OK",
                param => Trace.WriteLine("Actioned: " + param),
                win.IsMessagePanelContent);
        } 
    }
}

using CustomControls; 
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Van.Helper;
using Van.Interfaces;
using Van.Model;
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

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel win = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            var modules = StaticReflectionHelper.CreateAllInstancesOf<IModule>().ToList();
            var settings = modules.Where(x => x.IdType == 0).FirstOrDefault();
            win.SelectedModule = settings;
        }
    }
}

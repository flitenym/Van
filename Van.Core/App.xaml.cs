using Van.Core.View;
using System.Threading.Tasks;
using System.Windows;
using Van.Core.ViewModel;
using Van.Core.Windows;

namespace Van.Core
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var splashScreen = new SplashScreenWindow();
            this.MainWindow = splashScreen;
            splashScreen.Show();

            Task.Factory.StartNew(() =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var mainWindow = new MainWindowView();
                    var vm = new MainWindowViewModel();
                    mainWindow.DataContext = vm;
                    this.MainWindow = mainWindow;
                    mainWindow.Show();
                    splashScreen.Close();
                    mainWindow.Closing += (s, args) =>
                    {
                        if (vm.SelectedTheme != null)
                            vm.SelectedTheme.Deactivate();

                        if (vm.SelectedThemeDarkOrLight != null)
                            vm.SelectedThemeDarkOrLight.Deactivate();

                        if (vm.SelectedViewModel != null)
                            vm.SelectedViewModel.ModuleBaseItem.Deactivate();
                    };
                });
            });
        }
    }
}

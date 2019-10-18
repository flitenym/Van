using Van.View; 
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Van.Interfaces;
using Van.Helper;
using Van.ViewModel;
using Van.Windows;

namespace Van
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
                this.Dispatcher.Invoke(async () =>
                { 
                    await Task.Delay(TimeSpan.FromSeconds(1.5));  
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
                    }; 
                });
            }); 
        }
    }
}

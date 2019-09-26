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

namespace Van
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = new MainWindowView();

            var vm = new MainWindowViewModel();

            mainWindow.DataContext = vm;

            mainWindow.Closing += (s, args) =>
            { 
                if (vm.SelectedTheme != null)
                    vm.SelectedTheme.Deactivate();
            };

            mainWindow.Show();
        }
    }
}

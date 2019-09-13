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

            List<IModule> modules = new List<IModule>(); //лист где все страницы

            modules = StaticReflectionHelper.CreateAllInstancesOf<IModule>().ToList();

            List<ITheme> themes = new List<ITheme>(); //лист где все темы

            themes = StaticReflectionHelper.CreateAllInstancesOf<ITheme>().ToList();

            var vm = new MainWindowViewModel(modules, themes);

            mainWindow.DataContext = vm;

            mainWindow.Closing += (s, args) =>
            {
                if (vm.SelectedModule != null)
                    vm.SelectedModule.Deactivate();

                if (vm.SelectedTheme != null)
                    vm.SelectedTheme.Deactivate();
            };

            mainWindow.Show();
        }
    }
}

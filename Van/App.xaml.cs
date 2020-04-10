using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using Van.AbstractClasses;
using Van.Helper;
using Van.Helper.StaticInfo;
using Van.ViewModel;
using Van.ViewModel.Provider;
using Van.Windows.View;
using Van.Windows.ViewModel;

namespace Van
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Task.Factory.StartNew(() => HelperMethods.Message($"Глобальная ошибка: {e.Exception.Message}"));

            var infoVM = SharedProvider.GetFromDictionaryByKey(nameof(InfoViewModel)) as InfoViewModel ?? new InfoViewModel();
            infoVM.UpdateStackTrace(e.Exception.StackTrace);
            e.Handled = true;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var splashScreen = new SplashScreenWindowView();
            this.MainWindow = splashScreen;
            splashScreen.Show();

            Task.Factory.StartNew(() =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    //Все вьюшки
                    SharedProvider.SetToSingleton(
                        InfoKeys.ModulesKey,
                        HelperMethods.GetAllInstancesOf<ModuleBase>().Where(x => x.IsActive).ToList());

                    //Все темы
                    SharedProvider.SetToSingleton(
                        InfoKeys.ThemesKey,
                        HelperMethods.GetAllInstancesOf<ThemeBase>().ToList());

                    //Основная ViewModel
                    var vm = new MainWindowViewModel();
                    SharedProvider.SetToSingleton(nameof(MainWindowViewModel), vm);

                    var infovm = new InfoViewModel();
                    SharedProvider.SetToSingleton(nameof(InfoViewModel), infovm);

                    var mainWindow = new MainWindowView();

                    mainWindow.DataContext = vm;
                    this.MainWindow = mainWindow;
                    mainWindow.Show();
                    splashScreen.Close();
                    mainWindow.Closing += async (s, args) =>
                    {
                        try
                        {
                            if (vm.SelectedTheme != null)
                                vm.SelectedTheme.Deactivate();

                            if (vm.SelectedThemeDarkOrLight != null)
                                vm.SelectedThemeDarkOrLight.Deactivate();

                            if (vm.SelectedViewModel != null)
                                vm.SelectedViewModel.ModuleBaseItem.Deactivate();

                            this.Dispatcher.UnhandledException -= OnDispatcherUnhandledException;
                        }
                        catch (Exception ex)
                        {
                            await Helper.HelperMethods.Message($"{ex.Message}");
                        }
                    };
                });
            });
        }
    }
}
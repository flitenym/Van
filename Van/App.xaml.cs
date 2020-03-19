using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Van.AbstractClasses;
using Van.Helper;
using Van.Helper.StaticInfo;
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
                    SharedProvider.SetToSingletonAsync(
                        InfoKeys.ModulesKey,
                        HelperMethods.GetAllInstancesOf<ModuleBase>().Where(x => x.IsActive).ToList());

                    //Все темы
                    SharedProvider.SetToSingletonAsync(
                        InfoKeys.ThemesKey,
                        HelperMethods.GetAllInstancesOf<ThemeBase>().ToList());

                    //Основная ViewModel
                    var vm = new MainWindowViewModel();
                    SharedProvider.SetToSingletonAsync(nameof(MainWindowViewModel), vm);

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
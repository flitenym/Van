using System.Windows;
using System.Windows.Input;
using Van.Windows.ViewModel;

namespace Van.Windows.View
{
    /// <summary>
    /// Логика взаимодействия для SplashScreen.xaml
    /// </summary>
    public partial class SplashScreenWindowView : Window
    {
        public SplashScreenWindowView()
        {
            InitializeComponent(); 
            this.DataContext = new SplashScreenWindowViewModel();
        } 

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}

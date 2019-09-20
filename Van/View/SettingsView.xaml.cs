using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Van.Interfaces;
using Van.ViewModel;
using static Van.Helper.Helper;


namespace Van.View
{ 
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel win = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            Button bn = (Button)sender;

            Type TestType = Type.GetType("Freon.Model." + bn.Uid.ToString(), false, true);

            //если класс не найден
            if (TestType != null)
            {
                System.Reflection.ConstructorInfo ci = TestType.GetConstructor(new Type[] { });

                //вызываем конструтор
                object Obj = ci.Invoke(new object[] { });

                if (win.SelectedTheme != (ITheme)Obj)
                {
                    win.SelectedTheme = (ITheme)Obj;
                    Message($"Тема изменена на {((ITheme)Obj).Name}");
                }
            }
        }

    }
}

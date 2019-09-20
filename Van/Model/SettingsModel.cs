using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Van.AbstractClasses;
using Van.View;
using Van.ViewModel;

namespace Van.Model
{
    class SettingsModel : ModuleBase
    {
        public override string Name
        {
            get { return "Настройки"; }
        }

        public override int Num
        {
            get { return 0; }
        }

        public override int IdType
        {
            get { return 0; }
        }

        protected override UserControl CreateViewAndViewModel()
        {
            return new SettingsView() { DataContext = new SettingsViewModel() };
        }

    }
}

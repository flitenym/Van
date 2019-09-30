using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Van.AbstractClasses;
using Van.Helper;
using Van.View;
using Van.ViewModel;
using static Van.Helper.Enums;

namespace Van.Model
{
    class SettingsModel : ModuleBase
    {
        public override string Name => Types.ViewData.SettingsName;

        public override int Num => Types.ViewData.SettingsNum;

        public override Guid ID => Types.ViewData.SettingsView;
        public override Guid? ParentID => null;

        public override ModelBaseClasses modelClass => ModelBaseClasses.Settings; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new SettingsView() { DataContext = new SettingsViewModel() };
        }

    }
}

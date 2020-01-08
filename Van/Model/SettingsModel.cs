using System;
using System.Windows.Controls;
using Van.AbstractClasses;
using Van.View;
using Van.ViewModel;
using Van.Helper.StaticInfo;
using static Van.Helper.StaticInfo.Enums;

namespace Van.Model
{
    class SettingsModel : ModuleBase
    {
        public override string Name => Types.ViewData.SettingsName;

        public override int Num => Types.ViewData.SettingsNum;

        public override bool IsActive => true;

        public override Guid ID => Types.ViewData.SettingsView;

        public override Guid? ParentID => null;

        public override ModelBaseClasses modelClass => ModelBaseClasses.Settings; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new SettingsView() { DataContext = new SettingsViewModel() };
        }

    }
}

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
        public override string Name => Types.ViewData.Settings.Name;

        public override int Num => Types.ViewData.Settings.Num;

        public override bool IsActive => Types.ViewData.Settings.IsActive;

        public override Guid ID => Types.ViewData.Settings.View;

        public override ModelBaseClasses modelClass => Types.ViewData.Settings.ModelClass;

        public override Guid? ParentID => null;

        protected override UserControl CreateViewAndViewModel()
        {
            return new SettingsView() { DataContext = new SettingsViewModel() };
        }

    }
}

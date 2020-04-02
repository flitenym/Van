using System;
using System.Windows.Controls;
using Van.AbstractClasses;
using Van.View;
using Van.ViewModel;
using Van.Helper.StaticInfo;
using static Van.Helper.StaticInfo.Enums;

namespace Van.Model
{
    class MainMenuModel : ModuleBase
    {
        public override string Name => Types.ViewData.MainMenu.Name;

        public override int Num => Types.ViewData.MainMenu.Num;

        public override bool IsActive => Types.ViewData.MainMenu.IsActive;

        public override Guid ID => Types.ViewData.MainMenu.View;

        public override ModelBaseClasses modelClass => Types.ViewData.MainMenu.ModelClass;

        public override Guid? ParentID => null;

        protected override UserControl CreateViewAndViewModel()
        {
            return new MainMenuView() { DataContext = new MainMenuViewModel() };
        }

    }
}

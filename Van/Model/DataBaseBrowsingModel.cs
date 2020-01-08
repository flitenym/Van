using System;
using System.Windows.Controls;
using Van.AbstractClasses;
using Van.View;
using Van.ViewModel;
using Van.Helper.StaticInfo;
using static Van.Helper.StaticInfo.Enums;

namespace Van.Model
{
    class DataBaseBrowsingModel : ModuleBase
    {
        public override string Name => Types.ViewData.DataBaseBrowsingName;

        public override int Num => Types.ViewData.DataBaseBrowsingNum;

        public override bool IsActive => true;

        public override Guid ID => Types.ViewData.DataBaseBrowsingView;

        public override Guid? ParentID => null;

        public override ModelBaseClasses modelClass => ModelBaseClasses.RightMenu;

        protected override UserControl CreateViewAndViewModel()
        {
            return new DataBaseBrowsingView() { DataContext = new DataBaseBrowsingViewModel() };
        }

    }
}

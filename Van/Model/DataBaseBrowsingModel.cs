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
        public override string Name => Types.ViewData.DataBaseBrowsing.Name;

        public override int Num => Types.ViewData.DataBaseBrowsing.Num;

        public override bool IsActive => Types.ViewData.DataBaseBrowsing.IsActive;

        public override Guid ID => Types.ViewData.DataBaseBrowsing.View;

        public override ModelBaseClasses modelClass => Types.ViewData.DataBaseBrowsing.ModelClass;

        public override Guid? ParentID => null;

        protected override UserControl CreateViewAndViewModel()
        {
            return new DataBaseBrowsingView() { DataContext = new DataBaseBrowsingViewModel() };
        }

    }
}

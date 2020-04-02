using System;
using System.Windows.Controls;
using Van.AbstractClasses;
using Van.Helper.StaticInfo;
using Van.View.Methods;
using Van.ViewModel.Methods;
using static Van.Helper.StaticInfo.Enums;

namespace Van.Model.Methods
{
    class DistributionModel : ModuleBase
    {
        public override string Name => Types.ViewData.Distribution.Name;

        public override int Num => Types.ViewData.Distribution.Num;

        public override bool IsActive => Types.ViewData.Distribution.IsActive;

        public override Guid ID => Types.ViewData.Distribution.View;

        public override ModelBaseClasses modelClass => Types.ViewData.Distribution.ModelClass;

        public override Guid? ParentID => Types.ViewData.Parametric.View;

        protected override UserControl CreateViewAndViewModel()
        {
            return new DistributionView() { DataContext = new DistributionViewModel() };
        }

    }
}

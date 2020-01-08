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
        public override string Name => Types.ViewData.DistributionName;

        public override int Num => Types.ViewData.DistributionNum;

        public override bool IsActive => true;

        public override Guid ID => Types.ViewData.DistributionView;

        public override Guid? ParentID => Types.ViewData.ParametricView;

        public override ModelBaseClasses modelClass => ModelBaseClasses.LeftMenu; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new DistributionView() { DataContext = new DistributionViewModel() };
        }

    }
}

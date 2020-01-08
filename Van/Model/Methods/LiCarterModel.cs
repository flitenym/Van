using System;
using System.Windows.Controls;
using Van.AbstractClasses;
using Van.Helper.StaticInfo;
using Van.View.Methods;
using Van.ViewModel.Methods;
using static Van.Helper.StaticInfo.Enums;

namespace Van.Model.Methods
{
    class LiCarterModel : ModuleBase
    {
        public override string Name => Types.ViewData.LiCarterName;

        public override int Num => Types.ViewData.LiCarterNum;

        public override bool IsActive => true;

        public override Guid ID => Types.ViewData.LiCarterView;

        public override Guid? ParentID => Types.ViewData.ParametricView;

        public override ModelBaseClasses modelClass => ModelBaseClasses.LeftMenu; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new LiCarterView() { DataContext = new LiCarterViewModel() };
        }

    }
}

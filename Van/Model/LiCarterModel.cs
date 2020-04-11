using SharedLibrary.AbstractClasses;
using System;
using System.Windows.Controls;
using Van.Helper.StaticInfo;
using Van.View.Methods;
using Van.ViewModel.Methods;
using static SharedLibrary.Helper.StaticInfo.Enums;

namespace Van.Model.Methods
{
    class LiCarterModel : ModuleBase
    {
        public override string Name => Types.ViewData.LiCarter.Name;

        public override int Num => Types.ViewData.LiCarter.Num;

        public override bool IsActive => Types.ViewData.LiCarter.IsActive;

        public override bool IsNeedToDeactivate => Types.ViewData.LiCarter.IsNeedToDeactivate;

        public override Guid ID => Types.ViewData.LiCarter.View;

        public override ModelBaseClasses modelClass => Types.ViewData.LiCarter.ModelClass;

        public override Guid? ParentID => Types.ViewData.Parametric.View;

        protected override UserControl CreateViewAndViewModel()
        {
            return new LiCarterView() { DataContext = new LiCarterViewModel() };
        }

    }
}

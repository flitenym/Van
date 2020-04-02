using System;
using System.Windows.Controls;
using Van.AbstractClasses;
using Van.Helper.StaticInfo;
using Van.View.Methods;
using Van.ViewModel.Methods;
using static Van.Helper.StaticInfo.Enums;

namespace Van.Model.Methods
{
    class SemiParametricModel : ModuleBase
    {
        public override string Name => Types.ViewData.SemiParametric.Name;

        public override int Num => Types.ViewData.SemiParametric.Num;

        public override bool IsActive => Types.ViewData.SemiParametric.IsActive;

        public override Guid ID => Types.ViewData.SemiParametric.View;

        public override ModelBaseClasses modelClass => Types.ViewData.SemiParametric.ModelClass;

        public override Guid? ParentID => Types.ViewData.SurvivalAnalysisMethods.View;

        protected override UserControl CreateViewAndViewModel()
        {
            return new SemiParametricView() { DataContext = new SemiParametricViewModel() };
        }

    }
}

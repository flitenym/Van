using SharedLibrary.AbstractClasses;
using System;
using System.Windows.Controls;
using Van.Helper.StaticInfo;
using Van.View.Methods;
using Van.ViewModel.Methods;
using static SharedLibrary.Helper.StaticInfo.Enums;

namespace Van.Model.Methods
{
    class ParametricModel : ModuleBase
    {
        public override string Name => Types.ViewData.Parametric.Name;

        public override int Num => Types.ViewData.Parametric.Num;

        public override bool IsActive => Types.ViewData.Parametric.IsActive;

        public override bool IsNeedToDeactivate => Types.ViewData.Parametric.IsNeedToDeactivate;

        public override Guid ID => Types.ViewData.Parametric.View;

        public override ModelBaseClasses modelClass => Types.ViewData.Parametric.ModelClass;

        public override Guid? ParentID => Types.ViewData.SurvivalAnalysisMethods.View;

        protected override UserControl CreateViewAndViewModel()
        {
            return new ParametricView() { DataContext = new ParametricViewModel() };
        }

    }
}

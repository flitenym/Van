using System;
using System.Windows.Controls;
using Van.AbstractClasses;
using Van.Helper.StaticInfo;
using Van.View.Methods;
using Van.ViewModel.Methods;
using static Van.Helper.StaticInfo.Enums;

namespace Van.Model.Methods
{
    class NonparametricModel : ModuleBase
    {
        public override string Name => Types.ViewData.Nonparametric.Name;

        public override int Num => Types.ViewData.Nonparametric.Num;

        public override bool IsActive => Types.ViewData.Nonparametric.IsActive;

        public override bool IsNeedToDeactivate => Types.ViewData.Nonparametric.IsNeedToDeactivate;

        public override Guid ID => Types.ViewData.Nonparametric.View;

        public override ModelBaseClasses modelClass => Types.ViewData.Nonparametric.ModelClass;

        public override Guid? ParentID => Types.ViewData.SurvivalAnalysisMethods.View;

        protected override UserControl CreateViewAndViewModel()
        {
            return new NonparametricView() { DataContext = new NonparametricViewModel() };
        }

    }
}

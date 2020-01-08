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
        public override string Name => Types.ViewData.NonparametricName;

        public override int Num => Types.ViewData.NonparametricNum;

        public override bool IsActive => true;

        public override Guid ID => Types.ViewData.NonparametricView;

        public override Guid? ParentID => Types.ViewData.SurvivalAnalysisMethodsView;

        public override ModelBaseClasses modelClass => ModelBaseClasses.LeftMenu; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new NonparametricView() { DataContext = new NonparametricViewModel() };
        }

    }
}

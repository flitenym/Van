using System;
using System.Windows.Controls;
using Van.AbstractClasses;
using Van.Helper.StaticInfo;
using Van.View.Methods;
using Van.ViewModel.Methods;
using static Van.Helper.StaticInfo.Enums;

namespace Van.Model.Methods
{
    class SurvivalAnalysisMethodsModel : ModuleBase
    {
        public override string Name => Types.ViewData.SurvivalAnalysisMethodsName;

        public override int Num => Types.ViewData.SurvivalAnalysisMethodsNum;

        public override bool IsActive => true;

        public override Guid ID => Types.ViewData.SurvivalAnalysisMethodsView;

        public override Guid? ParentID => null;

        public override ModelBaseClasses modelClass => ModelBaseClasses.LeftMenu; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new SurvivalAnalysisMethodsView() { DataContext = new SurvivalAnalysisMethodsViewModel() };
        }

    }
}

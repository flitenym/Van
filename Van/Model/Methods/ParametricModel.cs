using System;
using System.Windows.Controls;
using Van.AbstractClasses;
using Van.Helper.StaticInfo;
using Van.View.Methods;
using Van.ViewModel.Methods;
using static Van.Helper.StaticInfo.Enums;

namespace Van.Model.Methods
{
    class ParametricModel : ModuleBase
    {
        public override string Name => Types.ViewData.ParametricName;

        public override int Num => Types.ViewData.ParametricNum;

        public override bool IsActive => true;

        public override Guid ID => Types.ViewData.ParametricView;

        public override Guid? ParentID => Types.ViewData.SurvivalAnalysisMethodsView;

        public override ModelBaseClasses modelClass => ModelBaseClasses.LeftMenu; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new ParametricView() { DataContext = new ParametricViewModel() };
        }

    }
}

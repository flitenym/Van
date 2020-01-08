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
        public override string Name => Types.ViewData.SemiParametricName;

        public override int Num => Types.ViewData.SemiParametricNum;

        public override bool IsActive => true;

        public override Guid ID => Types.ViewData.SemiParametricView;

        public override Guid? ParentID => Types.ViewData.SurvivalAnalysisMethodsView;

        public override ModelBaseClasses modelClass => ModelBaseClasses.LeftMenu; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new SemiParametricView() { DataContext = new SemiParametricViewModel() };
        }

    }
}

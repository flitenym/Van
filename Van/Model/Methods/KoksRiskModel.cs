using System;
using System.Windows.Controls;
using Van.AbstractClasses;
using Van.Helper.StaticInfo;
using Van.View.Methods;
using Van.ViewModel.Methods;
using static Van.Helper.StaticInfo.Enums;

namespace Van.Model.Methods
{
    class KoksRiskModel : ModuleBase
    {
        public override string Name => Types.ViewData.KoksRiskName;

        public override int Num => Types.ViewData.KoksRiskNum;

        public override bool IsActive => true;

        public override Guid ID => Types.ViewData.KoksRiskView;

        public override Guid? ParentID => Types.ViewData.SemiParametricView;

        public override ModelBaseClasses modelClass => ModelBaseClasses.LeftMenu; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new KoksRiskView() { DataContext = new KoksRiskViewModel() };
        }

    }
}

using System;
using System.Windows.Controls;
using Van.AbstractClasses;
using Van.Helper.StaticInfo;
using Van.View.Methods;
using Van.ViewModel.Methods;
using static Van.Helper.StaticInfo.Enums;

namespace Van.Model.Methods
{
    class KaplanMeierModel : ModuleBase
    {
        public override string Name => Types.ViewData.KaplanMeierName;

        public override int Num => Types.ViewData.KaplanMeierNum;

        public override bool IsActive => true;

        public override Guid ID => Types.ViewData.KaplanMeierView;

        public override Guid? ParentID => Types.ViewData.NonparametricView;

        public override ModelBaseClasses modelClass => ModelBaseClasses.LeftMenu; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new KaplanMeierView() { DataContext = new KaplanMeierViewModel() };
        }

    }
}

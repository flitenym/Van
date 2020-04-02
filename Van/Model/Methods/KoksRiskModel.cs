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
        public override string Name => Types.ViewData.KoksRisk.Name;

        public override int Num => Types.ViewData.KoksRisk.Num;

        public override bool IsActive => Types.ViewData.KoksRisk.IsActive;

        public override Guid ID => Types.ViewData.KoksRisk.View;

        public override ModelBaseClasses modelClass => Types.ViewData.KoksRisk.ModelClass;

        public override Guid? ParentID => Types.ViewData.SemiParametric.View;

        protected override UserControl CreateViewAndViewModel()
        {
            return new KoksRiskView() { DataContext = new KoksRiskViewModel() };
        }

    }
}

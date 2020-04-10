using System;
using System.Windows.Controls;
using Van.AbstractClasses;
using Van.Helper.StaticInfo;
using Van.View.Methods;
using Van.ViewModel.Methods;
using static Van.Helper.StaticInfo.Enums;

namespace Van.Model.Methods
{
    class ExcludingMortalityModel : ModuleBase
    {
        public override string Name => Types.ViewData.ExcludingMortality.Name;

        public override int Num => Types.ViewData.ExcludingMortality.Num;

        public override bool IsActive => Types.ViewData.ExcludingMortality.IsActive;

        public override bool IsNeedToDeactivate => Types.ViewData.ExcludingMortality.IsNeedToDeactivate;

        public override Guid ID => Types.ViewData.ExcludingMortality.View;

        public override ModelBaseClasses modelClass => Types.ViewData.ExcludingMortality.ModelClass;

        public override Guid? ParentID => Types.ViewData.MortalityTable.View;

        protected override UserControl CreateViewAndViewModel()
        {
            return new ExcludingMortalityView() { DataContext = new ExcludingMortalityViewModel() };
        }

    }
}

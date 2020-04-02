using System;
using System.Windows.Controls;
using Van.AbstractClasses;
using Van.Helper.StaticInfo;
using Van.View.Methods;
using Van.ViewModel.Methods;
using static Van.Helper.StaticInfo.Enums;

namespace Van.Model.Methods
{
    class IncludingMortalityModel : ModuleBase
    {
        public override string Name => Types.ViewData.IncludingMortality.Name;

        public override int Num => Types.ViewData.IncludingMortality.Num;

        public override bool IsActive => Types.ViewData.IncludingMortality.IsActive;

        public override Guid ID => Types.ViewData.IncludingMortality.View;

        public override ModelBaseClasses modelClass => Types.ViewData.IncludingMortality.ModelClass;

        public override Guid? ParentID => Types.ViewData.MortalityTable.View;

        protected override UserControl CreateViewAndViewModel()
        {
            return new IncludingMortalityView() { DataContext = new IncludingMortalityViewModel() };
        }

    }
}

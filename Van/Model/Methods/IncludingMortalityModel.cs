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
        public override string Name => Types.ViewData.IncludingMortalityName;

        public override int Num => Types.ViewData.IncludingMortalityNum;

        public override bool IsActive => true;

        public override Guid ID => Types.ViewData.IncludingMortalityView;

        public override Guid? ParentID => Types.ViewData.MortalityTableView;

        public override ModelBaseClasses modelClass => ModelBaseClasses.LeftMenu; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new IncludingMortalityView() { DataContext = new IncludingMortalityViewModel() };
        }

    }
}

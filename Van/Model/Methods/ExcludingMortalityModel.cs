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
        public override string Name => Types.ViewData.ExcludingMortalityName;

        public override int Num => Types.ViewData.ExcludingMortalityNum;

        public override bool IsActive => true;

        public override Guid ID => Types.ViewData.ExcludingMortalityView;

        public override Guid? ParentID => Types.ViewData.MortalityTableView;

        public override ModelBaseClasses modelClass => ModelBaseClasses.LeftMenu; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new ExcludingMortalityView() { DataContext = new ExcludingMortalityViewModel() };
        }

    }
}

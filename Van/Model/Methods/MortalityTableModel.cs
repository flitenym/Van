using System;
using System.Windows.Controls;
using Van.AbstractClasses;
using Van.Helper.StaticInfo;
using Van.View.Methods;
using Van.ViewModel.Methods;
using static Van.Helper.StaticInfo.Enums;

namespace Van.Model.Methods
{
    class MortalityTableModel : ModuleBase
    {
        public override string Name => Types.ViewData.MortalityTableName;

        public override int Num => Types.ViewData.MortalityTableNum;

        public override bool IsActive => true;

        public override Guid ID => Types.ViewData.MortalityTableView;

        public override Guid? ParentID => Types.ViewData.NonparametricView;

        public override ModelBaseClasses modelClass => ModelBaseClasses.LeftMenu; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new MortalityTableView() { DataContext = new MortalityTableViewModel() };
        }

    }
}

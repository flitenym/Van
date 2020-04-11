using SharedLibrary.AbstractClasses;
using System;
using System.Windows.Controls;
using Van.Helper.StaticInfo;
using Van.View.Methods;
using Van.ViewModel.Methods;
using static SharedLibrary.Helper.StaticInfo.Enums;

namespace Van.Model.Methods
{
    class MortalityTableModel : ModuleBase
    {
        public override string Name => Types.ViewData.MortalityTable.Name;

        public override int Num => Types.ViewData.MortalityTable.Num;

        public override bool IsActive => Types.ViewData.MortalityTable.IsActive;

        public override bool IsNeedToDeactivate => Types.ViewData.MortalityTable.IsNeedToDeactivate;

        public override Guid ID => Types.ViewData.MortalityTable.View;

        public override ModelBaseClasses modelClass => Types.ViewData.MortalityTable.ModelClass;

        public override Guid? ParentID => Types.ViewData.Nonparametric.View;

        protected override UserControl CreateViewAndViewModel()
        {
            return new MortalityTableView() { DataContext = new MortalityTableViewModel() };
        }

    }
}

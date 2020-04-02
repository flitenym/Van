﻿using System;
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
        public override string Name => Types.ViewData.MortalityTable.Name;

        public override int Num => Types.ViewData.MortalityTable.Num;

        public override bool IsActive => Types.ViewData.MortalityTable.IsActive;

        public override Guid ID => Types.ViewData.MortalityTable.View;

        public override ModelBaseClasses modelClass => Types.ViewData.MortalityTable.ModelClass;

        public override Guid? ParentID => Types.ViewData.Nonparametric.View;

        protected override UserControl CreateViewAndViewModel()
        {
            return new MortalityTableView() { DataContext = new MortalityTableViewModel() };
        }

    }
}

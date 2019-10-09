﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Van.AbstractClasses;
using Van.Helper;
using Van.View;
using Van.ViewModel;
using static Van.Helper.Enums;

namespace Van.Model
{
    class IncludingMortalityModel : ModuleBase
    {
        public override string Name => Types.ViewData.IncludingMortalityName;

        public override int Num => Types.ViewData.IncludingMortalityNum;

        public override Guid ID => Types.ViewData.IncludingMortalityView;

        public override Guid? ParentID => Types.ViewData.MortalityTableView;

        public override ModelBaseClasses modelClass => ModelBaseClasses.LeftMenu; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new IncludingMortalityView() { DataContext = new IncludingMortalityViewModel() };
        }

    }
}
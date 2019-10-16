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
    class KoksRiskModel : ModuleBase
    {
        public override string Name => Types.ViewData.KoksRiskName;

        public override int Num => Types.ViewData.KoksRiskNum;

        public override bool IsActive => true;

        public override Guid ID => Types.ViewData.KoksRiskView;

        public override Guid? ParentID => Types.ViewData.SemiParametricView;

        public override ModelBaseClasses modelClass => ModelBaseClasses.LeftMenu; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new KoksRiskView() { DataContext = new KoksRiskViewModel() };
        }

    }
}

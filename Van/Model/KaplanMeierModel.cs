﻿using SharedLibrary.AbstractClasses;
using System;
using System.Windows.Controls;
using Van.Helper.StaticInfo;
using Van.View.Methods;
using Van.ViewModel.Methods;
using static SharedLibrary.Helper.StaticInfo.Enums;

namespace Van.Model.Methods
{
    class KaplanMeierModel : ModuleBase
    {
        public override string Name => Types.ViewData.KaplanMeier.Name;

        public override int Num => Types.ViewData.KaplanMeier.Num;

        public override bool IsActive => Types.ViewData.KaplanMeier.IsActive;

        public override bool IsNeedToDeactivate => Types.ViewData.KaplanMeier.IsNeedToDeactivate;

        public override Guid ID => Types.ViewData.KaplanMeier.View;

        public override ModelBaseClasses modelClass => Types.ViewData.KaplanMeier.ModelClass;

        public override Guid? ParentID => Types.ViewData.Nonparametric.View;

        protected override UserControl CreateViewAndViewModel()
        {
            return new KaplanMeierView() { DataContext = new KaplanMeierViewModel() };
        }

    }
}

﻿using SharedLibrary.AbstractClasses;
using System;
using System.Windows.Controls;
using Van.Helper.StaticInfo;
using Van.View.Methods;
using Van.ViewModel.Methods;
using static SharedLibrary.Helper.StaticInfo.Enums;

namespace Van.Model.Methods
{
    class SurvivalAnalysisMethodsModel : ModuleBase
    {
        public override string Name => Types.ViewData.SurvivalAnalysisMethods.Name;

        public override int Num => Types.ViewData.SurvivalAnalysisMethods.Num;

        public override bool IsActive => Types.ViewData.SurvivalAnalysisMethods.IsActive;

        public override bool IsNeedToDeactivate => Types.ViewData.SurvivalAnalysisMethods.IsNeedToDeactivate;

        public override Guid ID => Types.ViewData.SurvivalAnalysisMethods.View;

        public override ModelBaseClasses modelClass => Types.ViewData.SurvivalAnalysisMethods.ModelClass;

        public override Guid? ParentID => null;

        protected override UserControl CreateViewAndViewModel()
        {
            return new SurvivalAnalysisMethodsView() { DataContext = new SurvivalAnalysisMethodsViewModel() };
        }

    }
}

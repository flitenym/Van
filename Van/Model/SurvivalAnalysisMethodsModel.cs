using System;
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
    class SurvivalAnalysisMethodsModel : ModuleBase
    {
        public override string Name => Types.ViewData.SurvivalAnalysisMethodsName;

        public override int Num => Types.ViewData.SurvivalAnalysisMethodsNum;

        public override bool IsActive => true;

        public override Guid ID => Types.ViewData.SurvivalAnalysisMethodsView;

        public override Guid? ParentID => null;

        public override ModelBaseClasses modelClass => ModelBaseClasses.LeftMenu; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new SurvivalAnalysisMethodsView() { DataContext = new SurvivalAnalysisMethodsViewModel() };
        }

    }
}

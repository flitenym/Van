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
    class NonparametricModel : ModuleBase
    {
        public override string Name => Types.ViewData.NonparametricName;

        public override int Num => Types.ViewData.NonparametricNum;

        public override bool IsActive => true;

        public override Guid ID => Types.ViewData.NonparametricView;

        public override Guid? ParentID => Types.ViewData.SurvivalAnalysisMethodsView;

        public override ModelBaseClasses modelClass => ModelBaseClasses.LeftMenu; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new NonparametricView() { DataContext = new NonparametricViewModel() };
        }

    }
}

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
    class ParametricModel : ModuleBase
    {
        public override string Name => Types.ViewData.ParametricName;

        public override int Num => Types.ViewData.ParametricNum;

        public override bool IsActive => true;

        public override Guid ID => Types.ViewData.ParametricView;

        public override Guid? ParentID => Types.ViewData.SurvivalAnalysisMethodsView;

        public override ModelBaseClasses modelClass => ModelBaseClasses.LeftMenu; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new ParametricView() { DataContext = new ParametricViewModel() };
        }

    }
}

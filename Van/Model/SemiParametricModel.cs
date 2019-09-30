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
    class SemiParametricModel : ModuleBase
    {
        public override string Name => Types.ViewData.SemiParametricName;

        public override int Num => Types.ViewData.SemiParametricNum;

        public override Guid ID => Types.ViewData.SemiParametricView;

        public override Guid? ParentID => Types.ViewData.SurvivalAnalysisMethodsView;

        public override ModelBaseClasses modelClass => ModelBaseClasses.LeftMenu; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new SemiParametricView() { DataContext = new SemiParametricViewModel() };
        }

    }
}

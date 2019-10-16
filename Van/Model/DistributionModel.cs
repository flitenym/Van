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
    class DistributionModel : ModuleBase
    {
        public override string Name => Types.ViewData.DistributionName;

        public override int Num => Types.ViewData.DistributionNum;

        public override bool IsActive => true;

        public override Guid ID => Types.ViewData.DistributionView;

        public override Guid? ParentID => Types.ViewData.ParametricView;

        public override ModelBaseClasses modelClass => ModelBaseClasses.LeftMenu; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new DistributionView() { DataContext = new DistributionViewModel() };
        }

    }
}

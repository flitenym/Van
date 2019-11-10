using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Van.Core.AbstractClasses;
using Van.Core.Helper;
using Van.Core.View;
using Van.Core.ViewModel;
using static Van.Core.Helper.Enums;

namespace Van.Core.Model
{
    class LiCarterModel : ModuleBase
    {
        public override string Name => Types.ViewData.LiCarterName;

        public override int Num => Types.ViewData.LiCarterNum;

        public override bool IsActive => true;

        public override Guid ID => Types.ViewData.LiCarterView;

        public override Guid? ParentID => Types.ViewData.ParametricView;

        public override ModelBaseClasses modelClass => ModelBaseClasses.LeftMenu; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new LiCarterView() { DataContext = new LiCarterViewModel() };
        }

    }
}

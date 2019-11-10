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
    class KaplanMeierModel : ModuleBase
    {
        public override string Name => Types.ViewData.KaplanMeierName;

        public override int Num => Types.ViewData.KaplanMeierNum;

        public override bool IsActive => true;

        public override Guid ID => Types.ViewData.KaplanMeierView;

        public override Guid? ParentID => Types.ViewData.NonparametricView;

        public override ModelBaseClasses modelClass => ModelBaseClasses.LeftMenu; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new KaplanMeierView() { DataContext = new KaplanMeierViewModel() };
        }

    }
}

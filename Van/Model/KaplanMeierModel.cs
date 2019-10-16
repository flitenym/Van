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

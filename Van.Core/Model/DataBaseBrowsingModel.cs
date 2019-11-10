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
    class DataBaseBrowsingModel : ModuleBase
    {
        public override string Name => Types.ViewData.DataBaseBrowsingName;

        public override int Num => Types.ViewData.DataBaseBrowsingNum;

        public override bool IsActive => true;

        public override Guid ID => Types.ViewData.DataBaseBrowsingView;

        public override Guid? ParentID => null;

        public override ModelBaseClasses modelClass => ModelBaseClasses.RightMenu;

        protected override UserControl CreateViewAndViewModel()
        {
            return new DataBaseBrowsingView() { DataContext = new DataBaseBrowsingViewModel() };
        }

    }
}

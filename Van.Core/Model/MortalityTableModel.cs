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
    class MortalityTableModel : ModuleBase
    {
        public override string Name => Types.ViewData.MortalityTableName;

        public override int Num => Types.ViewData.MortalityTableNum;

        public override bool IsActive => true;

        public override Guid ID => Types.ViewData.MortalityTableView;

        public override Guid? ParentID => Types.ViewData.NonparametricView;

        public override ModelBaseClasses modelClass => ModelBaseClasses.LeftMenu; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new MortalityTableView() { DataContext = new MortalityTableViewModel() };
        }

    }
}

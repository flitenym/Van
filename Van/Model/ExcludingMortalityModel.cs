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
    class ExcludingMortalityModel : ModuleBase
    {
        public override string Name => Types.ViewData.ExcludingMortalityName;

        public override int Num => Types.ViewData.ExcludingMortalityNum;

        public override bool IsActive => true;

        public override Guid ID => Types.ViewData.ExcludingMortalityView;

        public override Guid? ParentID => Types.ViewData.MortalityTableView;

        public override ModelBaseClasses modelClass => ModelBaseClasses.LeftMenu; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new ExcludingMortalityView() { DataContext = new ExcludingMortalityViewModel() };
        }

    }
}

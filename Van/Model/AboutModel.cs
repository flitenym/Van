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
    class AboutModel : ModuleBase
    {
        public override string Name => Types.ViewData.AboutName;

        public override int Num => Types.ViewData.AboutNum;

        public override bool IsActive => true;

        public override Guid ID => Types.ViewData.AboutView;

        public override Guid? ParentID => null;

        public override ModelBaseClasses modelClass => ModelBaseClasses.RightMenu; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new AboutView() { DataContext = new AboutViewModel() };
        }

    }
}

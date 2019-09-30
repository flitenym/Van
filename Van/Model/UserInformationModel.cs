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
    class UserInformationModel : ModuleBase
    {
        public override string Name => Types.ViewData.UserInformationName;

        public override int Num => Types.ViewData.UserInformationNum;

        public override Guid ID => Types.ViewData.UserInformationView;

        public override Guid? ParentID => null;

        public override ModelBaseClasses modelClass => ModelBaseClasses.RightMenu; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new UserInformationView() { DataContext = new UserInformationViewModel() };
        }

    }
}

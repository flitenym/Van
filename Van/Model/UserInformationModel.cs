using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Van.AbstractClasses;
using Van.View;
using Van.ViewModel;
using static Van.Helper.Enums;

namespace Van.Model
{
    class UserInformationModel : ModuleBase
    {
        public override string Name => "Пользователь";

        public override int Num => 2;

        public override ModelBaseClasses modelClass => ModelBaseClasses.LeftMenu;

        protected override UserControl CreateViewAndViewModel()
        {
            return new UserInformationView() { DataContext = new UserInformationViewModel() };
        }

    }
}

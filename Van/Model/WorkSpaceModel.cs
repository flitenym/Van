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
    class WorkSpaceModel : ModuleBase
    {
        public override string Name => "Рабочее место";

        public override int Num => 1;

        public override ModelBaseClasses modelClass => ModelBaseClasses.Main;

        protected override UserControl CreateViewAndViewModel()
        {
            return new WorkSpaceView() { DataContext = new WorkSpaceViewModel() };
        }

    }
}

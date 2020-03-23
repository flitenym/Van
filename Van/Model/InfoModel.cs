using System;
using System.Windows.Controls;
using Van.AbstractClasses;
using Van.View;
using Van.ViewModel;
using Van.Helper.StaticInfo;
using static Van.Helper.StaticInfo.Enums;

namespace Van.Model
{
    class InfoModel : ModuleBase
    {
        public override string Name => Types.ViewData.InfoName;

        public override int Num => Types.ViewData.InfoNum;

        public override bool IsActive => true;

        public override Guid ID => Types.ViewData.InfoView;

        public override Guid? ParentID => null;

        public override ModelBaseClasses modelClass => ModelBaseClasses.RightMenu; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new InfoView() { DataContext = new InfoViewModel() };
        }

    }
}

using System;
using System.Windows.Controls;
using Van.AbstractClasses;
using Van.View;
using Van.ViewModel;
using Van.Helper.StaticInfo;
using static Van.Helper.StaticInfo.Enums;

namespace Van.Model
{
    class ConnectionDataBaseModel : ModuleBase
    {
        public override string Name => Types.ViewData.ConnectionDataBaseName;

        public override int Num => Types.ViewData.ConnectionDataBaseNum;

        public override bool IsActive => true;

        public override Guid ID => Types.ViewData.ConnectionDataBaseView;

        public override Guid? ParentID => null;

        public override ModelBaseClasses modelClass => ModelBaseClasses.RightMenu; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new ConnectionDataBaseView() { DataContext = new ConnectionDataBaseViewModel() };
        }

    }
}

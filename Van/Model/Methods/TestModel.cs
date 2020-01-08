using System;
using System.Windows.Controls;
using Van.AbstractClasses;
using Van.Helper.StaticInfo;
using Van.View.Methods;
using Van.ViewModel.Methods;
using static Van.Helper.StaticInfo.Enums;

namespace Van.Model.Methods
{
    class TestModel : ModuleBase
    {
        public override string Name => Types.ViewData.TestName;

        public override int Num => Types.ViewData.TestNum;

        public override bool IsActive => true;

        public override Guid ID => Types.ViewData.TestView;

        public override Guid? ParentID => null;

        public override ModelBaseClasses modelClass => ModelBaseClasses.LeftMenu; 

        protected override UserControl CreateViewAndViewModel()
        {
            return new TestView() { DataContext = new TestViewModel() };
        }

    }
}

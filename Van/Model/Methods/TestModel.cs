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
        public override string Name => Types.ViewData.Test.Name;

        public override int Num => Types.ViewData.Test.Num;

        public override bool IsActive => Types.ViewData.Test.IsActive;

        public override bool IsNeedToDeactivate => Types.ViewData.Test.IsNeedToDeactivate;

        public override Guid ID => Types.ViewData.Test.View;

        public override ModelBaseClasses modelClass => Types.ViewData.Test.ModelClass;

        public override Guid? ParentID => null;

        protected override UserControl CreateViewAndViewModel()
        {
            return new TestView() { DataContext = new TestViewModel() };
        }

    }
}

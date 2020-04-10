using System;
using System.Windows.Controls;
using Van.AbstractClasses;
using Van.View;
using Van.ViewModel;
using Van.Helper.StaticInfo;
using static Van.Helper.StaticInfo.Enums;
using Van.ViewModel.Provider;

namespace Van.Model
{
    class InfoModel : ModuleBase
    {
        public override string Name => Types.ViewData.Info.Name;

        public override int Num => Types.ViewData.Info.Num;

        public override bool IsActive => Types.ViewData.Info.IsActive;

        public override bool IsNeedToDeactivate => Types.ViewData.Info.IsNeedToDeactivate;

        public override Guid ID => Types.ViewData.Info.View;

        public override ModelBaseClasses modelClass => Types.ViewData.Info.ModelClass;

        public override Guid? ParentID => null;

        protected override UserControl CreateViewAndViewModel()
        {
            if (IsNeedToDeactivate == false)
            {
                return new InfoView()
                {
                    DataContext = SharedProvider.GetFromDictionaryByKey(nameof(InfoViewModel)) ?? new InfoViewModel()
                };
            }

            return new InfoView()
            {
                DataContext = new InfoViewModel()
            };
        }
    }
}
﻿using System;
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
        public override string Name => Types.ViewData.ConnectionDataBase.Name;

        public override int Num => Types.ViewData.ConnectionDataBase.Num;

        public override bool IsActive => Types.ViewData.ConnectionDataBase.IsActive;

        public override Guid ID => Types.ViewData.ConnectionDataBase.View;

        public override ModelBaseClasses modelClass => Types.ViewData.ConnectionDataBase.ModelClass;

        public override Guid? ParentID => null;

        protected override UserControl CreateViewAndViewModel()
        {
            return new ConnectionDataBaseView() { DataContext = new ConnectionDataBaseViewModel() };
        }

    }
}

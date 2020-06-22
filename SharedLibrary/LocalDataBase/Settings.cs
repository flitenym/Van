﻿using System.ComponentModel;
using SharedLibrary.AbstractClasses;
using SharedLibrary.Helper.Attributes;

namespace SharedLibrary.LocalDataBase.Models
{
    [ModelClass(TableTitle = "Настройки", IsVisible = false, CanDelete = false, CanInsert = false, CanUpdate = false, CanLoad = false)]
    public class Settings : ModelClass
    {
        [ColumnData(ShowInTable = false)]
        public int ID { get; set; }

        [ColumnData(ShowInTable = false)]
        [Description("Название ключа")]
        public string Name { get; set; }

        //[Description("Название")]
        //public string Title { get; set; }

        [Description("Значение")]
        public string Value { get; set; }
    }
}
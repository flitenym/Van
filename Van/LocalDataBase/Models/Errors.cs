using System;
using System.ComponentModel;
using Van.AbstractClasses;
using Van.Helper.Attributes;

namespace Van.LocalDataBase.Models
{
    [ModelClass(TableTitle = "Ошибки", IsVisible = false, CanDelete = false, CanInsert = false, CanUpdate = false, CanLoad = false)]
    public class Errors : ModelClass
    {
        [Description("Stack")]
        public string StackTrace { get; set; }

        [Description("Название")]
        public DateTime Date { get; set; }
    }
}
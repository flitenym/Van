using System.ComponentModel;
using Van.AbstractClasses;
using Van.Helper.Attributes;

namespace Van.LocalDataBase.Models
{
    [ModelClass(TableTitle = "Настройки", CanDelete = false, CanInsert = false, CanUpdate = false, CanLoad = false)]
    public class Settings : ModelClass
    {
        [ColumnData(ShowInTable = false)]
        public int ID { get; set; }

        [Description("Название")]
        public string Name { get; set; }

        [Description("Значение")]
        public string Value { get; set; }
    }
}
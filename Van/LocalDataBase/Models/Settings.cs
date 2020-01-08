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

        public override string InsertQuery => $@"INSERT INTO {nameof(Settings)}(Name, Value) VALUES (@Name, @Value);  select last_insert_rowid()";

        public override string UpdateQuery(int ID)
        {
            return $@"UPDATE {nameof(Settings)} SET Name = @Name, Value = @Value WHERE ID = {ID}";
        }
    }
}
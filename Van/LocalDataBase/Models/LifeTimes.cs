using System;
using System.ComponentModel;
using Van.AbstractClasses;
using Van.Helper.Attributes;

namespace Van.DataBase.Models
{
    [ModelClass(TableTitle = "Время жизни")]
    public class LifeTimes : ModelClass, ICloneable
    {
        [ColumnData(ShowInTable = false)]
        public int ID { get; set; } 

        [Description("Время жизни")]
        public int? LifeTime { get; set; }

        [Description("Цензурированность")]
        public int? Censor { get; set; }

        public override string InsertQuery() => $@"INSERT INTO {nameof(LifeTimes)}(LifeTime, Censor) VALUES (@LifeTime, @Censor);  select last_insert_rowid()";

        public override string UpdateQuery(int ID)
        {
            return $@"UPDATE {nameof(LifeTimes)} SET LifeTime = @LifeTime, Censor = @Censor WHERE ID = {ID}";
        }

        public object Clone()
        {
            return new LifeTimes
            {
                ID = this.ID,
                LifeTime = this.LifeTime,
                Censor = this.Censor
            };
        }
    }
}

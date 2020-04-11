using SharedLibrary.AbstractClasses;
using SharedLibrary.Helper.Attributes;
using System;
using System.ComponentModel;

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

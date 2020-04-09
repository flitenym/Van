using System;
using System.ComponentModel;
using Van.AbstractClasses;
using Van.Helper.Attributes;
using Van.LocalDataBase.ModelsHelper;

namespace Van.DataBase.Models
{
    [ModelClass(TableTitle = "Оценка качества моделей в диапазоне", CanInsert = false)]
    public class Quality : ModelClass, IMortalityTable, IMethod, ICloneable
    {
        [ColumnData(ShowInTable = false)]
        public int ID { get; set; }

        [ColumnData(ShowInTable = false, IsNullable = false)]
        public int Method { get; set; }

        [ColumnData(ShowInTable = false, IsNullable = false)]
        public int MortalityTableID { get; set; }

        [Description("Возраст X")]  
        public string MortalityTableAgeX { get; set; }

        [ColumnData(ShowInTable = false)]
        [Description("Табличный")]
        public double? Standart { get; set; }

        [Description("Вейбулла")]
        public double? Weibull { get; set; }

        [Description("Рэлея")]
        public double? Relay { get; set; }

        [Description("Гомпертца")]
        public double? Gompertz { get; set; }

        [Description("Экспоненциальное")]
        public double? Exponential { get; set; }

        public object Clone()
        {
            return new Quality
            {
                ID = this.ID,
                Method = this.Method,
                MortalityTableID = this.MortalityTableID,
                MortalityTableAgeX = this.MortalityTableAgeX,
                Standart = this.Standart,
                Weibull = this.Weibull,
                Relay = this.Relay,
                Gompertz = this.Gompertz,
                Exponential = this.Exponential
            };
        }
    }
}

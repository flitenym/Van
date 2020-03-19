using System;
using System.ComponentModel;
using Van.AbstractClasses;
using Van.Helper.Attributes;
using Van.LocalDataBase.ModelsHelper;

namespace Van.DataBase.Models
{
    [ModelClass(TableTitle = "Функция выживания (остаточная)", CanInsert = false)]
    public class ResidualSurvivalFunction : ModelClass, IMortalityTable, IMethod, ICloneable
    {
        [ColumnData(ShowInTable = false)]
        public int ID { get; set; }

        [ColumnData(ShowInTable = false)]
        public int MortalityTableID { get; set; }

        [Description("Возраст X")]  
        public string MortalityTableAgeX { get; set; }

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

        public override string InsertQuery => $@"INSERT INTO {nameof(ResidualSurvivalFunction)}(MortalityTableID, MortalityTableAgeX, Standart, Weibull, Relay, Gompertz, Exponential) VALUES (@MortalityTableID, @MortalityTableAgeX, @Standart, @Weibull, @Relay, @Gompertz, @Exponential);  select last_insert_rowid()";

        public override string UpdateQuery(int ID)
        {
            return $@"UPDATE {nameof(ResidualSurvivalFunction)} SET Standart = @Standart, Weibull = @Weibull, Relay = @Relay, Gompertz = @Gompertz, Exponential = @Exponential WHERE ID = {ID}";
        }

        public object Clone()
        {
            return new ResidualSurvivalFunction
            {
                ID = this.ID,
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

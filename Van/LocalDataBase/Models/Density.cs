using System.ComponentModel;
using Van.AbstractClasses;
using Van.Helper.Attributes;

namespace Van.DataBase.Models
{
    [ModelClass(TableTitle = "Плотность гамма распределения", CanInsert = false)]
    public class Density : ModelClass
    {
        [ColumnData(ShowInTable = false)]
        public int ID { get; set; }

        [ColumnData(ShowInTable = false)]
        public int MortalityTableID { get; set; }

        [Description("Возраст X")]
        public string MortalityTableAgeX { get; set; }

        [Description("Стандартный")]
        public double? Standart { get; set; }

        [Description("Вейбулла")]
        public double? Weibull { get; set; }

        [Description("Рэлея")]
        public double? Relay { get; set; }

        [Description("Гомпертца")]
        public double? Gompertz { get; set; }

        [Description("Экспоненциальное")]
        public double? Exponential { get; set; }

        public override string InsertQuery => string.Empty;

        public override string UpdateQuery(int ID)
        {
            return $@"UPDATE {nameof(Density)} SET Standart = @Standart, Weibull = @Weibull, Relay = @Relay, Gompertz = @Gompertz, Exponential = @Exponential WHERE ID = {ID}";
        }
    }
}

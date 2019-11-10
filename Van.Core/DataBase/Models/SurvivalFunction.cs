using Dapper.Contrib.Extensions;
using System.ComponentModel;

namespace Van.Core.DataBase.Models
{
    [ModelClass(TableTitle = "Функция выживания", CanInsert = false)]
    public class SurvivalFunction : ModelClass
    {
        [Write(false)]
        [Computed]
        [Key]
        public int ID { get; set; }

        [Write(false)]
        [Computed] 
        public int MortalityTableID { get; set; }

        [Write(false)]
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
    }

    /// <summary>
    /// Разрешено только обновление (Добавление запрещено)
    /// </summary>
    public static class SurvivalFunctionQuery
    { 
        public static string UpdateQuery(int ID) {
            return $@"UPDATE {nameof(SurvivalFunction)} SET Standart = @Standart, Weibull = @Weibull, Relay = @Relay, Gompertz = @Gompertz, Exponential = @Exponential WHERE ID = {ID}";
        }
    }


}

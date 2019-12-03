using Dapper.Contrib.Extensions;
using System.ComponentModel;

namespace Van.DataBase.Models
{
    [ModelClass(TableTitle = "Оценка качесва моделей", CanInsert = false)]
    public class QualityAssessmentOfModels : ModelClass
    {
        [Write(false)]
        [Computed]
        [Key]
        public int ID { get; set; }

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
    public static class QualityAssessmentOfModelsQuery
    { 
        public static string UpdateQuery(int ID) {
            return $@"UPDATE {nameof(QualityAssessmentOfModels)} SET Standart = @Standart, Weibull = @Weibull, Relay = @Relay, Gompertz = @Gompertz, Exponential = @Exponential WHERE ID = {ID}";
        }

        public static string InsertQuery => $@"INSERT INTO {nameof(QualityAssessmentOfModels)}(Standart, Weibull, Relay, Gompertz, Exponential) VALUES (@Standart, @Weibull, @Relay, @Gompertz, @Exponential);  select last_insert_rowid()";
    }


}

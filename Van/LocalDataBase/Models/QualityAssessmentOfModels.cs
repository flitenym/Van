using System;
using System.ComponentModel;
using Van.AbstractClasses;
using Van.Helper.Attributes;

namespace Van.DataBase.Models
{
    [ModelClass(TableTitle = "Оценка качесва моделей", CanInsert = false)]
    public class QualityAssessmentOfModels : ModelClass, ICloneable
    {
        [ColumnData(ShowInTable = false)]
        public int ID { get; set; }

        [Description("Оценка")]
        public string Quality { get; set; }

        [Description("Вейбулла")]
        public double? Weibull { get; set; }

        [Description("Рэлея")]
        public double? Relay { get; set; }

        [Description("Гомпертца")]
        public double? Gompertz { get; set; }

        [Description("Экспоненциальное")]
        public double? Exponential { get; set; }

        public override string InsertQuery => $@"INSERT INTO {nameof(QualityAssessmentOfModels)}(Quality, Weibull, Relay, Gompertz, Exponential) VALUES (@Quality, @Weibull, @Relay, @Gompertz, @Exponential);  select last_insert_rowid()";

        public override string UpdateQuery(int ID)
        {
            return $@"UPDATE {nameof(QualityAssessmentOfModels)} SET Quality = @Quality, Weibull = @Weibull, Relay = @Relay, Gompertz = @Gompertz, Exponential = @Exponential WHERE ID = {ID}";
        }

        public object Clone()
        {
            return new QualityAssessmentOfModels
            {
                ID = this.ID,
                Quality = this.Quality,
                Weibull = this.Weibull,
                Relay = this.Relay,
                Gompertz = this.Gompertz,
                Exponential = this.Exponential
            };
        }
    }
}

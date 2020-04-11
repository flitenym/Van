using SharedLibrary.AbstractClasses;
using SharedLibrary.Helper.Attributes;
using System;
using System.ComponentModel;

namespace Van.DataBase.Models
{
    [ModelClass(TableTitle = "Оценка качества моделей", CanInsert = false)]
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

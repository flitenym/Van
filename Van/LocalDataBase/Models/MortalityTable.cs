using SharedLibrary.AbstractClasses;
using SharedLibrary.Helper.Attributes;
using System;
using System.ComponentModel;

namespace Van.DataBase.Models
{
    [ModelClass(TableTitle = "Таблица выживаемости", CanUpload = true)]
    public class MortalityTable : ModelClass, ICloneable
    {
        [ColumnData(ShowInTable = false)]
        public int ID { get; set; }

        [Description("Возраст X")]
        public string AgeX { get; set; }

        [Description("Число доживших")]
        public int? NumberOfSurvivors { get; set; }

        [Description("Число умерших")]
        public int? NumberOfDead { get; set; }

        [Description("Вероятность")]
        public double? Probability { get; set; }

        [ColumnData(ShowInTable = false)]
        [Description("Продолжительность жизни")]
        public double? ExpectedDuration { get; set; }

        public object Clone()
        {
            return new MortalityTable
            {
                ID = this.ID,
                AgeX = this.AgeX,
                NumberOfSurvivors = this.NumberOfSurvivors,
                NumberOfDead = this.NumberOfDead,
                Probability = this.Probability,
                ExpectedDuration = this.ExpectedDuration
            };
        }
    }
}

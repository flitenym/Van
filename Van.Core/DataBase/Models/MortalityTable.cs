using Dapper.Contrib.Extensions;
using System.ComponentModel;

namespace Van.Core.DataBase.Models
{
    [ModelClass(TableTitle = "Таблица выживаемости")]
    public class MortalityTable : ModelClass
    {
        [Write(false)]
        [Computed]
        [Key]
        public int ID { get; set; }

        [Description("Возраст X")]
        public string AgeX { get; set; }

        [Description("Число доживших")]
        public int? NumberOfSurvivors { get; set; }

        [Description("Число умерших")]
        public int? NumberOfDead { get; set; }

        [Description("Вероятность")]
        public double? Probability { get; set; }

        [Description("Продолжительность жизни")]
        public double? ExpectedDuration { get; set; }
    }

    public static class MortalityTableQuery
    {
        public static string UpdateQuery(int ID) {
            return $@"UPDATE {nameof(MortalityTable)} SET AgeX = @AgeX, NumberOfSurvivors = @NumberOfSurvivors, NumberOfDead = @NumberOfDead, Probability = @Probability, ExpectedDuration = @ExpectedDuration WHERE ID = {ID}";
        }

        public static string InsertQuery => $@"INSERT INTO {nameof(MortalityTable)}(AgeX, NumberOfSurvivors, NumberOfDead, Probability, ExpectedDuration) VALUES (@AgeX, @NumberOfSurvivors, @NumberOfDead, @Probability, @ExpectedDuration);  select last_insert_rowid()";
    }


}

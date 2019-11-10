using Dapper.Contrib.Extensions;
using System.ComponentModel;

namespace Van.DataBase.Models
{
    [ModelClass(TableTitle = "Время жизни")]
    public class LifeTimes : ModelClass
    {
        [Write(false)]
        [Computed]
        [Key]
        public int ID { get; set; } 

        [Description("Время жизни")]
        public int? LifeTime { get; set; }

        [Description("Цензурированность")]
        public int? Censor { get; set; }
    }

    public static class LifeTimesQuery
    {
        public static string UpdateQuery(int ID) {
            return $@"UPDATE {nameof(LifeTimes)} SET LifeTime = @LifeTime, Censor = @Censor WHERE ID = {ID}";
        }

        public static string InsertQuery => $@"INSERT INTO {nameof(LifeTimes)}(LifeTime, Censor) VALUES (@LifeTime, @Censor);  select last_insert_rowid()";
    }


}

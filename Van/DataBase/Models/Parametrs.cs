using Dapper.Contrib.Extensions; 

namespace Van.DataBase.Models
{
    [ModelTitle(TableTitle = "Параметры")]
    public class Parametrs : ModelClass
    {
        [Write(false)]
        [Computed]
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}

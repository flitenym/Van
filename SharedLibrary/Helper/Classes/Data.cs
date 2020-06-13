using SharedLibrary.Helper.Attributes;

namespace SharedLibrary.Helper.Classes
{
    public class Data
    {
        [ColumnData(ShowInTable = false)]
        public int ID { get; set; }
        public string AgeX { get; set; }
        public int? NumberOfSurvivors { get; set; }
        public int? NumberOfDead { get; set; }
        [ColumnData(ShowInTable = false)]
        public double? Probability { get; set; }
        public double? ExpectedDuration { get; set; }
    }
}

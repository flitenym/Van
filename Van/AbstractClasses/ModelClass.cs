using Van.Helper.Attributes;

namespace Van.AbstractClasses
{
    public abstract class ModelClass
    {
        [ColumnData(ShowInTable = false)]
        public string Title { get; set; }

        [ColumnData(ShowInTable = false)]
        public bool CanInsert { get; set; }

        [ColumnData(ShowInTable = false)]
        public bool CanDelete { get; set; }

        [ColumnData(ShowInTable = false)]
        public bool CanUpdate { get; set; }

        [ColumnData(ShowInTable = false)]
        public bool CanLoad { get; set; }

        [ColumnData(ShowInTable = false)]
        public abstract string InsertQuery { get; }

        [ColumnData(ShowInTable = false)]
        public abstract string UpdateQuery(int ID);
    }
}

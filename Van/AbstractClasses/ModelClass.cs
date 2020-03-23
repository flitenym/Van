using Van.Helper.Attributes;

namespace Van.AbstractClasses
{
    public abstract class ModelClass
    {
        [ColumnData(ShowInTable = false)]
        public string Title { get; set; }

        [ColumnData(ShowInTable = false)]
        private bool CanInsert { get; set; }

        public bool GetCanInsert() => CanInsert;
        public void SetCanInsert(bool value) { CanInsert = value; }


        [ColumnData(ShowInTable = false)]
        private bool CanDelete { get; set; }

        public bool GetCanDelete() => CanDelete;
        public void SetCanDelete(bool value) { CanDelete = value; }

        [ColumnData(ShowInTable = false)]
        private bool CanUpdate { get; set; }

        public bool GetCanUpdate() => CanUpdate;
        public void SetCanUpdate(bool value) { CanUpdate = value; }

        [ColumnData(ShowInTable = false)]
        private bool CanLoad { get; set; }

        public bool GetCanLoad() => CanLoad;
        public void SetCanLoad(bool value) { CanLoad = value; }

        [ColumnData(ShowInTable = false)]
        public abstract string InsertQuery();

        [ColumnData(ShowInTable = false)]
        public abstract string UpdateQuery(int ID);
    }
}

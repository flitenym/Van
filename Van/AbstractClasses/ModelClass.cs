using Van.Helper;
using Van.Helper.Attributes;

namespace Van.AbstractClasses
{
    public abstract class ModelClass
    {
        [ColumnData(ShowInTable = false)]
        public string Title { get; set; }

        #region CanInsert

        [ColumnData(ShowInTable = false)]
        private bool CanInsert { get; set; }

        public bool GetCanInsert() => CanInsert;
        public void SetCanInsert(bool value) { CanInsert = value; }

        #endregion

        #region CanDelete

        [ColumnData(ShowInTable = false)]
        private bool CanDelete { get; set; }

        public bool GetCanDelete() => CanDelete;
        public void SetCanDelete(bool value) { CanDelete = value; }

        #endregion

        #region CanUpdate

        [ColumnData(ShowInTable = false)]
        private bool CanUpdate { get; set; }

        public bool GetCanUpdate() => CanUpdate;
        public void SetCanUpdate(bool value) { CanUpdate = value; }

        #endregion

        #region CanLoad

        [ColumnData(ShowInTable = false)]
        private bool CanLoad { get; set; }

        public bool GetCanLoad() => CanLoad;
        public void SetCanLoad(bool value) { CanLoad = value; }

        #endregion

        [ColumnData(ShowInTable = false)]
        public string InsertQuery(object obj)
        {
            var typeProperties = obj.GetType().GetProperties();
            var properties = HelperMethods.GetProperties(typeProperties);

            string fields = string.Empty;
            string values = string.Empty;

            for (int i = 0; i < properties.Length; i++)
            {
                fields += $", {properties[i].Name}";
                values += $", @{properties[i].Name}";
            }

            fields = fields.Substring(2, fields.Length - 2);
            values = values.Substring(2, values.Length - 2);

            return $"INSERT INTO {obj.GetType().Name} ({fields}) VALUES ({values});  select last_insert_rowid()";
        }

        [ColumnData(ShowInTable = false)]
        public string UpdateQuery(object obj, object ID)
        {
            var typeProperties = obj.GetType().GetProperties();
            var properties = HelperMethods.GetProperties(typeProperties);

            string fieldsQuery = string.Empty;

            for (int i = 0; i < properties.Length; i++)
            {
                fieldsQuery += $", {properties[i].Name} = @{properties[i].Name}";
            }

            fieldsQuery = fieldsQuery.Substring(2, fieldsQuery.Length - 2);

            return $"UPDATE {obj.GetType().Name} SET {fieldsQuery} WHERE ID = {ID}";
        }
    }
}
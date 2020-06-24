using SharedLibrary.AbstractClasses;
using SharedLibrary.Helper.Attributes;
using System;
using System.ComponentModel;
using Van.LocalDataBase.ModelsHelper;

namespace Van.DataBase.Models
{
    [ModelClass(TableTitle = "Группы")]
    public class GroupInfo : ModelClass, ICloneable
    {
        [ColumnData(ShowInTable = false)]
        public int ID { get; set; }

        [Description("Код")]
        public string Code { get; set; }

        [Description("Группа")]
        public string Group { get; set; }

        public object Clone()
        {
            return new GroupInfo
            {
                ID = this.ID,
                Code = this.Code,
                Group = this.Group
            };
        }
    }
}

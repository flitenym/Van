using SharedLibrary.AbstractClasses;
using SharedLibrary.Helper.Attributes;
using System;
using System.ComponentModel;
using Van.LocalDataBase.ModelsHelper;

namespace Van.DataBase.Models
{
    [ModelClass(TableTitle = "Регион")]
    public class RegionInfo : ModelClass, ICloneable
    {
        [ColumnData(ShowInTable = false)]
        public int ID { get; set; }

        [Description("Код")]
        public int Code { get; set; }

        [Description("Регион")]
        public string Reg { get; set; }

        public object Clone()
        {
            return new RegionInfo
            {
                ID = this.ID,
                Code = this.Code,
                Reg = this.Reg
            };
        }
    }
}

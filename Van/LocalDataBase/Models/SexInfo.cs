using SharedLibrary.AbstractClasses;
using SharedLibrary.Helper.Attributes;
using System;
using System.ComponentModel;
using Van.LocalDataBase.ModelsHelper;

namespace Van.DataBase.Models
{
    [ModelClass(TableTitle = "Пол")]
    public class SexInfo : ModelClass, ICloneable
    {
        [ColumnData(ShowInTable = false)]
        public int ID { get; set; }

        [Description("Код")]
        public string Code { get; set; }

        [Description("Пол")]
        public string Sex { get; set; }

        public object Clone()
        {
            return new SexInfo
            {
                ID = this.ID,
                Code = this.Code,
                Sex = this.Sex
            };
        }
    }
}

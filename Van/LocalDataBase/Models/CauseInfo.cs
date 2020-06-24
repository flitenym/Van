using SharedLibrary.AbstractClasses;
using SharedLibrary.Helper.Attributes;
using System;
using System.ComponentModel;
using Van.LocalDataBase.ModelsHelper;

namespace Van.DataBase.Models
{
    [ModelClass(TableTitle = "Причины смерти")]
    public class CauseInfo : ModelClass, ICloneable
    {
        [ColumnData(ShowInTable = false)]
        public int ID { get; set; }

        [Description("Код")]
        public int Code { get; set; }

        [Description("Причина смерти")]
        public string Cause { get; set; }

        public object Clone()
        {
            return new CauseInfo
            {
                ID = this.ID,
                Code = this.Code,
                Cause = this.Cause
            };
        }
    }
}

using System;

namespace Van.Helper.Attributes
{
    public class ColumnDataAttribute : Attribute
    {
        public bool ShowInTable { get; set; } = true;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Van.AbstractClasses;
using Van.Helper;
using static Van.Helper.Enums;

namespace Van.ThemesGlobal.Dark
{
    class Dark : ThemeBase
    {
        public override string Title => "Темная тема";
        public override string Name => nameof(Dark);
        public override string UriPath => @"ThemesGlobal\Dark\Dark.xaml"; 
        public override int Num => 0; 
        public override ThemeBaseClasses ThemeClass => ThemeBaseClasses.GlobalTheme;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Van.AbstractClasses;
using Van.Helper;
using static Van.Helper.Enums;

namespace Van.ThemesGlobal.Light
{
    class Light : ThemeBase
    {
        public override string Title => "Светлая тема";
        public override string Name => nameof(Light);
        public override string UriPath => @"ThemesGlobal\Light\Light.xaml"; 
        public override int Num => 1; 
        public override ThemeBaseClasses ThemeClass => ThemeBaseClasses.GlobalTheme;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Van.Core.AbstractClasses;
using Van.Core.Helper;
using static Van.Core.Helper.Enums;

namespace Van.Core.ThemesGlobal.Light
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Van.AbstractClasses;
using static Van.Helper.Enums;

namespace Van.Themes.Blue
{
    class Blue : ThemeBase
    {  
        public override string Title => "Голубая тема"; 
        public override string Name => nameof(Blue);
        public override string UriPath => @"Themes\Blue\Blue.xaml";
        public override int Num => 0;
        public override ThemeBaseClasses ThemeClass => ThemeBaseClasses.GeneralTheme;
    }
}

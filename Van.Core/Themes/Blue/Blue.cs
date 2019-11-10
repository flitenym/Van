using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Van.Core.AbstractClasses;
using static Van.Core.Helper.Enums;

namespace Van.Core.Themes.Blue
{
    class Blue : ThemeBase
    {  
        public override string Title => "Голубая тема"; 
        public override string Name => nameof(Blue);
        public override string UriPath => @"Themes\Blue\Blue.xaml";
        public override int Num => 1;
        public override ThemeBaseClasses ThemeClass => ThemeBaseClasses.GeneralTheme;
    }
}

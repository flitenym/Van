using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Van.Core.AbstractClasses;
using static Van.Core.Helper.Enums;

namespace Van.Core.Themes.Green
{
    class Green : ThemeBase
    {  
        public override string Title => "Зеленая тема"; 
        public override string Name => nameof(Green);
        public override string UriPath => @"Themes\Green\Green.xaml";
        public override int Num => 0;
        public override ThemeBaseClasses ThemeClass => ThemeBaseClasses.GeneralTheme;
    }
}

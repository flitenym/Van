using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Van.AbstractClasses;
using static Van.Helper.Enums;

namespace Van.Themes.Green
{
    class Red : ThemeBase
    {  
        public override string Title => "Красная тема"; 
        public override string Name => nameof(Red);
        public override string UriPath => @"Themes\Red\Red.xaml";
        public override int Num => 3;
        public override ThemeBaseClasses ThemeClass => ThemeBaseClasses.GeneralTheme;
    }
}

using Van.AbstractClasses;
using static Van.Helper.StaticInfo.Enums;

namespace Van.Themes.Red
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

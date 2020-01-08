using Van.AbstractClasses;
using static Van.Helper.StaticInfo.Enums;

namespace Van.Themes.Green
{
    class Green : ThemeBase
    {  
        public override string Title => "Зеленая тема"; 
        public override string Name => nameof(Green);
        public override string UriPath => @"Themes\Green\Green.xaml";
        public override int Num => 1;
        public override ThemeBaseClasses ThemeClass => ThemeBaseClasses.GeneralTheme;
    }
}

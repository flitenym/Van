using Van.AbstractClasses; 
using static Van.Helper.StaticInfo.Enums;

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

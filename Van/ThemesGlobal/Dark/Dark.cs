using Van.AbstractClasses;
using static Van.Helper.StaticInfo.Enums;

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

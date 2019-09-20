using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Van.AbstractClasses;

namespace Van.ThemesGlobal.Dark
{
    class Dark : ThemeBase
    {
        public override string Name
        {
            get
            {
                return "Dark Theme";
            }
        }

        public override string UriPath
        {
            get
            {
                return @"ThemesGlobal\Dark\Dark.xaml";
            }
        }

        public override int Num
        {
            get
            {
                return -1;
            }
        }

    }
}

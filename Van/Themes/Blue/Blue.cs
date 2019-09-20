using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Van.AbstractClasses;

namespace Van.Themes.Blue
{
    class Blue : ThemeBase
    {
        public override string Name
        {
            get
            {
                return "Blue Theme";
            }
        }

        public override string UriPath
        {
            get
            {
                return @"Themes\Blue\Blue.xaml";
            }
        }

        public override int Num
        {
            get
            {
                return 1;
            }
        }

    }
}

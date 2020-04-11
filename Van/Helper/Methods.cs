using System.Data; 
using System.Linq; 

namespace Van.Helper
{
    public static class Methods
    {
        /// <summary>
        /// В AgeX может быть лишние значение такие как + и т.д., поэтому спарсим только числа
        /// </summary>
        public static double GetTValue(string ageX)
        {
            // в AgeX может быть лишние значение такие как + и т.д., поэтому спарсим только числа
            if (double.TryParse(string.Join("", ageX.Where(c => char.IsDigit(c))), out double value))
            {
                return value;
            }
            return 0;
        }

    }
}

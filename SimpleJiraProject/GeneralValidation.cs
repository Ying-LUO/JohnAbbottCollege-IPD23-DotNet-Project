using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleJiraProject
{
    class GeneralValidation
    {
        public static bool IsValidName(string name)
        {
            return name.Length < 30 && name.Length > 1;
        }

        public static bool IsValidDate(DateTime date1, DateTime date2)
        {
            return date1 <= date2;
        }

        public static bool IsValidPoint(int point)
        {
            //int
            //if (int.TryParse(point, out int point))
            //{


                return point > 0 && point < 101;
        }

    }
}

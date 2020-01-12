using System;
using System.Collections.Generic;
using System.Text;

namespace WDTAssignment
{
    class Utilities
    {
        public static Boolean IsItAnInt(string s)
        {
            try
            {
                int.Parse(s);
                return true;
            }
            catch (FormatException e)
            {
                Console.WriteLine("Not valid. Please try again.\n");
                return false;
            }
        }

        public static Boolean IsItADouble(string s)
        {
            try
            {
                double.Parse(s);
                return true;
            }
            catch (FormatException e)
            {
                Console.WriteLine("Not valid. Please try again.\n");
                return false;
            }
        }
    }
}

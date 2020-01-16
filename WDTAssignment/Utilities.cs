using System;
using System.Collections.Generic;
using System.Text;

namespace WDTAssignment
{
    class Utilities
    {
        // checks to see if string entered can be parsed to an int or not
        // used for when user is selecting a menu option
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

        // checks to see if string can be parsed into a double or not
        // used for when user is entering an amount to be used for a transaction
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

        // checks to see if the amount entered by user is smaller than or equal to 0 or not
        // used for when the user is entering an amount to be used for a transaction 
        public static void ValidateNotZeroOrNegativeAmt(double amt)
        {
            if (amt <= 0)
            {
                throw new TransactionZeroAmountException("");
            }
        }
    }
}

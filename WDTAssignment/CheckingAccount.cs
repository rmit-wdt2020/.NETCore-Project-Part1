using System;
using System.Collections.Generic;
using System.Text;

namespace WDTAssignment
{
    class CheckingAccount : Account, IAccount
    {
        public CheckingAccount(char accounttype, double minbalance, double minopeningamt)
            : base(accounttype, minbalance, minopeningamt)
        {
            
        }
    }
}

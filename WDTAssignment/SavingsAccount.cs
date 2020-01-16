using System;
using System.Collections.Generic;
using System.Text;

namespace WDTAssignment
{
    class SavingsAccount : Account, IAccount
    {

        public SavingsAccount(char accounttype, double minbalance, double minopeningamt)
            : base(accounttype,  minbalance,  minopeningamt)
        {
            
        }
    }
}

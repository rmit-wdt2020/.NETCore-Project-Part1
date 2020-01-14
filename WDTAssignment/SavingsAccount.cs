using System;
using System.Collections.Generic;
using System.Text;

namespace WDTAssignment
{
    class SavingsAccount : Account
    {

        public SavingsAccount(int accountnumber, int customerid, double balance, double minbalance, double minopeningamt, int transactioncount)
            : base(accountnumber, customerid, balance, minbalance, minopeningamt, transactioncount)
        {
            AccountType = 'S';
        }
    }
}

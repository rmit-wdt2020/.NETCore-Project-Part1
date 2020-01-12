using System;
using System.Collections.Generic;
using System.Text;

namespace WDTAssignment
{
    class CheckingAccount : Accounts
    {
        public CheckingAccount(int accountnumber, int customerid, double balance, double minbalance, double minopeningamt, int transactioncount)
            : base(accountnumber, customerid, balance, minbalance, minopeningamt, transactioncount)
        {
            AccountType = 'C';
        }
    }
}

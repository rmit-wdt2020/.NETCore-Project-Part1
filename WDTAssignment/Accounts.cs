using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json; 

namespace WDTAssignment
{
    // Removed abstract as could not deserialize(JSON) abstract classes 
     class Accounts
    {
        
        public int AccountNumber { get; set; }
        public char AccountType { get; set; }
        public int CustomerID { get; set; }
        public double Balance { get; set; }
        protected double MinBalance { get; set; }
        protected double MinOpeningAmt { get; set; }
        protected int TransactionCount { get; set; }

        // Need to clarify if it's 4 free transactions per account or per customer
        protected const int FREE_TRANSACTIONS = 4;
        
        public List<Transactions> Transactions = new List<Transactions>();

        protected Accounts()
        {

        }

        protected Accounts (int accountnumber, int customerid, double balance, double minbalance, double minopeningamt, int transactioncount)
        {
            AccountNumber = accountnumber;
            CustomerID = customerid;
            Balance = balance;
            MinBalance = minbalance;
            MinOpeningAmt = minopeningamt;
            TransactionCount = transactioncount;
        }

        
    }
}

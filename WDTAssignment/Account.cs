using System;
using System.Collections.Generic;
using System.Text;

namespace WDTAssignment
{
    // Account class not abstract as could not deserialize abstract classes
    class Account
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

        public List<Transaction> Transactions = new List<Transaction>();

        protected Account()
        {

        }

        protected Account(int accountnumber, int customerid, double balance, double minbalance, double minopeningamt, int transactioncount)
        {
            AccountNumber = accountnumber;
            CustomerID = customerid;
            Balance = balance;
            MinBalance = minbalance;
            MinOpeningAmt = minopeningamt;
            TransactionCount = transactioncount;
        }

        public void RecordTransaction(int transactionID, char transType, int destAcc, double transAmount)
        {
            if (destAcc == 0)
            {
                destAcc = AccountNumber;
            }
            var newTrans = new Transaction(transactionID, transType, AccountNumber, destAcc, transAmount, "", DateTime.UtcNow);

            Transactions.Add(newTrans);
        }
    }
}

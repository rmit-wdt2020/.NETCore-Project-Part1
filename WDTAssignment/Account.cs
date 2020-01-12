using System;
using System.Collections.Generic;
using System.Text;

namespace WDTAssignment
{
    abstract class Account
    {
        public int AccountNumber { get; set; }
        public char AccountType { get; set; }
        protected int CustomerID { get; set; }
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

        public void RecordTransaction(char TransType, int DestAcc, string TransAmount)
        {
            if (DestAcc == 0)
            {
                DestAcc = AccountNumber;
            }
            var newTrans = new Transaction(Transactions.Count, TransType, AccountNumber, DestAcc, TransAmount, Balance, "", DateTime.MinValue);

            Transactions.Add(newTrans);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace WDTAssignment
{
    // Account class not abstract as could not deserialize abstract classes
    class Account : IAccount
    {
        public int AccountNumber { get; set; }
        public char AccountType { get; set; }
        public int CustomerID { get; set; }
        public double Balance { get; set; }
        public double MinBalance { get; set; }
        public double MinOpeningAmt { get; set; }
        public int TransactionCount { get; set; }

        // Need to clarify if it's 4 free transactions per account or per customer
        protected const int FREE_TRANSACTIONS = 4;

        public List<Transaction> Transactions = new List<Transaction>();

        protected Account()
        {

        }

        protected Account(char accounttype,  double minbalance, double minopeningamt)
        {
            AccountType = accounttype;
            MinBalance = minbalance;
            MinOpeningAmt = minopeningamt;
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

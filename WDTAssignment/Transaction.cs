using System;
using System.Collections.Generic;
using System.Text;

namespace WDTAssignment
{
    class Transaction
    {
        public int TransactionID { get; set; }
        public char TransactionType { get; set; }
        public int AccountNumber { get; set; }
        public int DestinationAccountNumber { get; set; }
        public double Amount { get; set; }
        public string Comment { get; set; }
        public DateTime TransactionTimeUTC { get; set; }

        public Transaction() { }

        public Transaction(char transactiontype, int accountnumber, int destinationaccountnum, double transactionamount, string comment, DateTime transactiontimeutc)
        {
            TransactionType = transactiontype;
            AccountNumber = accountnumber;
            DestinationAccountNumber = destinationaccountnum;
            Amount = transactionamount;
            Comment = comment;
            TransactionTimeUTC = transactiontimeutc;
        }

        // Overload for string parameter which would be parsed to a DateTime object
        public Transaction(char transactiontype, int accountnumber, int destinationaccountnum, double amount, string comment, string transactiontimeutc)
        {
            TransactionType = transactiontype;
            AccountNumber = accountnumber;
            DestinationAccountNumber = destinationaccountnum;
            Amount = amount;
            Comment = comment;
            TransactionTimeUTC = DateTime.Parse(transactiontimeutc);
        }

        public Transaction(int transactionid, char transactiontype, int accountnumber, int destinationaccountnum, double transactionamount, string comment, DateTime transactiontimeutc)
        {
            TransactionID = transactionid;
            TransactionType = transactiontype;
            AccountNumber = accountnumber;
            DestinationAccountNumber = destinationaccountnum;
            Amount = transactionamount;
            Comment = comment;
            TransactionTimeUTC = transactiontimeutc;
        }

        public Transaction(int transactionid, char transactiontype, int accountnumber, int destinationaccountnum, double amount, string comment, string transactiontimeutc)
        {
            TransactionID = transactionid;
            TransactionType = transactiontype;
            AccountNumber = accountnumber;
            DestinationAccountNumber = destinationaccountnum;
            Amount = amount;
            Comment = comment;
            TransactionTimeUTC = DateTime.Parse(transactiontimeutc);
        }

        public void getDetails()
        {
            string details;

            details = "\nTransaction ID:             " + TransactionID +
                      "\nTransaction Type:           " + TransactionType +
                      "\nAccount Number:             " + AccountNumber +
                      "\nDestination Account Number: " + DestinationAccountNumber +
                      "\nTransaction Amount:         " + Amount +
                      "\nTime & Date:                " + TransactionTimeUTC;

            Console.WriteLine(details);
        }
    }
}

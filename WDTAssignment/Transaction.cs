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

        public Transaction(int transactionid, char transactiontype, int accountnumber, int destinationaccountnum, double amount, string comment, DateTime datetime)
        {
            TransactionID = transactionid;
            TransactionType = transactiontype;
            AccountNumber = accountnumber;
            DestinationAccountNumber = destinationaccountnum;
            Amount = amount;
            Comment = comment;
            TransactionTimeUTC = datetime; 
        }

        public void getDetails()
        {
            string details;

            details = "\nTransaction ID: " + TransactionID +
                      "\nTransaction Type: " + TransactionType +
                      "\nAccount Number: " + AccountNumber +
                      "\nDestination Account Number: " + DestinationAccountNumber +
                      "\nAmount: " + Amount +
                      "\nTime & Date: " + TransactionTimeUTC;

            Console.WriteLine(details);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace WDTAssignment
{
    // custom exception to check if amount entered
    // is less than or equal to 0
    class TransactionZeroAmountException : Exception
    {
        public TransactionZeroAmountException() { }

        public TransactionZeroAmountException(string message)
            : base(String.Format("\nERROR: Transaction amount MUST be more than 0."))
        {

        }
    }
}

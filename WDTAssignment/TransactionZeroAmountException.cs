using System;
using System.Collections.Generic;
using System.Text;

namespace WDTAssignment
{
    class TransactionZeroAmountException : Exception
    {
        public TransactionZeroAmountException() { }

        public TransactionZeroAmountException(string message)
            : base(String.Format("\nERROR: Transaction amount cannot be 0 or less than 0."))
        {

        }
    }
}

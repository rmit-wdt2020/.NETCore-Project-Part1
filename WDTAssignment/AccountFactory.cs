using System;
using System.Collections.Generic;
using System.Text;

namespace WDTAssignment
{
    // Factory pattern to create accounts
    public static class AccountFactory
    {
        public static IAccount Create(char accountType)
        {
            switch (accountType)
            {
                case 'S':
                    return new SavingsAccount('S', 0, 100);
                    
                    
                case 'C':
                    return new CheckingAccount('C', 200, 500);
                    
                       
                default:
                    return null;
                    
            }

        }
    }
}

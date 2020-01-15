using System;
using System.Collections.Generic;
using System.Text;

namespace WDTAssignment
{
    class Logins
    {
        public string LoginID { get; set; }
        public int CustomerID { get; set; }
        public string PasswordHash { get; set; }

        public Logins (string loginID, int customerID, string passwordHash)
        {
            LoginID = loginID;
            CustomerID = customerID;
            PasswordHash = passwordHash; 

        }  
    }
}

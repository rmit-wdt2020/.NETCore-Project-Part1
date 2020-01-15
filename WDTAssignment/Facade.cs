using System;
using System.Collections.Generic;
using System.Text;

namespace WDTAssignment
{
    class Facade
    {
        private BankingSys _bankingSys;

        public Facade ()
        {
            _bankingSys = new BankingSys();
        }

        public void run()
        {
            Console.WriteLine("Welcome to National Wealth Bank of Australasia");
            _bankingSys.db.ImportDatabase();
            _bankingSys.Login();
        }
    }
}

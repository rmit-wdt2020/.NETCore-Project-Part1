using System;
using System.Collections.Generic;
using System.Text;

namespace WDTAssignment
{
    // Facade pattern
    class Facade
    {
        private BankingSys _bankingSys;

        public Facade ()
        {
            _bankingSys = BankingSys.Instance(); 
        }

        // initializes the program
        public void run()
        {
            JSON json = new JSON();

            json.PopulateDatabase(); 
            Console.WriteLine("Welcome to National Wealth Bank of Australasia");
            _bankingSys.db.ImportDatabase();
            _bankingSys.Login();
        }
    }
}

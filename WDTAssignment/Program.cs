using System;

namespace WDTAssignment
{
    class Program
    {
        static void Main(string[] args)
        {
            ////JSON.PopulateDatabase();
            //BankingSys bank = new BankingSys();
            //bank.db.ImportDatabase();
            ////Console.WriteLine(bank.db.GetTransactionID());
            ////bank.TestPopulate();
            //Console.WriteLine("Welcome to National Wealth Bank of Australasia");
            //bank.Login();

            Facade facade = new Facade();
            facade.run();
        }
    }
}

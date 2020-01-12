using System;

namespace WDTAssignment
{
    class Program
    {
        static void Main(string[] args)
        {
            BankingSys bank = new BankingSys();
            bank.TestPopulate();
            Console.WriteLine("Welcome to National Wealth Bank of Australasia");
            bank.Login();
        }
    }
}

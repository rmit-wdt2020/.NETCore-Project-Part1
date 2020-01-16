using System;


namespace WDTAssignment
{
    class Program
    {
        

        static void Main(string[] args)
        {

            Database db = new Database();
            JSON.PopulateDatabase();
            db.ImportDatabase(); 
            Console.WriteLine("Welcome to National Wealth Bank of Australasia");
            BankingSys.Instance().Login();
        }
    }
}

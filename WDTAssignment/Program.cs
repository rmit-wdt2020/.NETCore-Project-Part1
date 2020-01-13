using System;


namespace WDTAssignment
{
    class Program
    {
        static void Main(string[] args)
        {
            //JSON.PopulateDatabase();
            Database.ImportDatabase();

            BankingSys.Customers[0].Accounts[0].Transactions.Add(new Transactions(1, 'D', 4100, 4100, 8888, "CNY red packet from auntie lee", "19/12/2019 08:30:00 PM"));
            Database.InsertTransaction(BankingSys.Customers[0].Accounts[0]);

            foreach (var customer in BankingSys.Customers)
            {
                Console.WriteLine("Account Holder: " + customer.Name);

                foreach (var account in customer.Accounts)
                {
                    Console.WriteLine("Account Number: " + account.AccountNumber);

                    foreach (var transaction in account.Transactions)
                    {
                        Console.WriteLine("Transaction Amount: " + transaction.Amount);
                        Console.WriteLine("Transaction Comment: " + transaction.Comment);
                        Console.WriteLine("Transaction Time: " + transaction.TransactionTimeUTC);
                    }

                }

                Console.WriteLine();
            }








            /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/


            //Database.DownloadLoginArray();
            //Database.DownloadCustomerArray();

            //BankingSys.Logins.Add(new Logins("69696969", 6969, "fuckthisstupidassignmentitsuckssfuckthisstupidassignmentitsuckss"));

            //var Rio = new Customer(98532424, "fuckthisstupidassignmentitsuckssfuckthisstupidassignmentitsuckss", 6969, "Caecario Wardana", 
            //    "45 Clarke St", "Melbourne", "3006");
            //Rio.Accounts.Add(new SavingsAccount(6699, 6969, 79000, 100, 100, 1 ));
            //Rio.Accounts.Add(new CheckingAccount(6700, 6969, 4500, 100, 100, 1));
            //Rio.Accounts[0].Transactions.Add(new Transactions(1, 'D', 6699, 6969, 4000, null, "19/12/2019 08:30:00 PM")); 
            //Rio.Accounts[1].Transactions.Add(new Transactions(2, 'D', 6700, 6700, 100, null, "19/12/2019 09:30:00 PM"));
            //Rio.Accounts[1].Transactions.Add(new Transactions(2, 'W', 6700, 6700, 500, null, "19/12/2019 09:30:00 PM"));

            //BankingSys.Customers.Add(Rio);

            //Database.UpdateBalance(Rio.Accounts[1]);
            //Database.InsertTransaction(Rio.Accounts[1]);



            /*-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/

            //Console.WriteLine("Welcome to National Wealth Bank of Australasia");
            //bank.Login();

        }
    }
}

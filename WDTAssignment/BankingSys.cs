using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace WDTAssignment
{
     class BankingSys
    {

        public static List<Customer> Customers { get; set; }  = new List<Customer>();       
        public static List<Accounts> accList { get; set; } = new List<Accounts>();
        public static List<Logins> Logins { get; set; } = new List<Logins>(); 

        Customer currentCustomer;

        public void Login()
        {
            Console.WriteLine();
            Console.Write("LoginID: ");
            //var login = int.Parse(Console.ReadLine());
            var tempString = Console.ReadLine();

            if(Utilities.IsItAnInt(tempString))
            {
                var login = int.Parse(tempString);
                Validate(login);
            }
            else
            {
                Login();
            }
        }

        public void Validate(int login)
        {
            foreach (var customer in Customers)
            {
                if (login == customer.LoginID)
                {
                    while (true)
                    {
                        string password;
                        Console.WriteLine("\nEnter your password,\n" +
                            "or 'b' to return to previous screen): ");
                        password = Console.ReadLine();

                        if ("b".CompareTo(password.ToLower()) == 0)
                        {
                            Console.WriteLine("Returning to login page");
                            Login();
                        }

                        if (password.CompareTo(customer.Password) == 0)
                        {
                            currentCustomer = customer;
                            MainMenu();
                        }
                        Console.WriteLine("Password is incorrect, please try again.\n");
                    }
                }
            }
            Console.WriteLine("The Login ID you've entered does not exist. Please try again.");
            Login();
        }

        public void MainMenu()
        {
            while (true)
            {
                Console.WriteLine("\n***** National Wealth Bank of Australasia System Menu ******");
                Console.WriteLine("ATM:                            1");
                Console.WriteLine("Account Transfer:               2");
                Console.WriteLine("View Statement:                 3");
                Console.WriteLine("Logout:                         9");
                Console.WriteLine("Exit:                           0");
                Console.Write("Please enter your choice: ");

                var tempString = Console.ReadLine();
                    
                if(Utilities.IsItAnInt(tempString))
                {
                    var option = int.Parse(tempString);
                    MenuChoice(option);
                }
                else
                {
                    MainMenu();
                }
            }
        }

        public void MenuChoice(int option)
        {
            switch (option)
            {
                case 1:
                    ATMMenu();
                    break;

                case 2:
                    AccountTransferMenu();
                    break;

                case 3:
                    StatementMenu();
                    break;

                case 9:
                    Logout();
                    break;

                case 0:
                    Exit();
                    break;

                default:
                    Console.WriteLine("\nInvalid entry: Please try again.");
                    break;
            }
        }

        public void ATMMenu()
        {
            while(true)
            {
                Console.Write("\n***** ATM Menu *****\n" +
                    "Deposit               1\n" +
                    "Withdraw              2\n" +
                    "Back to Main Menu     9\n\n" +
                    "Please enter your choice: ");

                var option = Console.ReadLine();
                if(Utilities.IsItAnInt(option) == true)
                {
                    ATMChoice(int.Parse(option));
                }
            }
        }

        public void ATMChoice(int option)
        {
            switch (option)
            {
                case 1:
                    Deposit();
                    break;
                case 2:
                    break;
                case 9:
                    MainMenu();
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again");
                    ATMMenu();
                    break;
            }
        }

        public void AccountTransferMenu()
        {
            while (true)
            {
                Console.Write("\n***** Account Transfer Menu *****\n" +
                    "Transfer between own account        1\n" +
                    "Transfer to third-party account     2\n" +
                    "Back to Main Menu                   9\n\n" +
                    "Please enter your choice: ");

                var option = Console.ReadLine();
                if (Utilities.IsItAnInt(option) == true)
                {
                    TransferChoice(int.Parse(option));
                }
            }
        }

        public void TransferChoice(int option)
        {
            switch (option)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 9:
                    MainMenu();
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again");
                    AccountTransferMenu();
                        break;
            }
        }

        public void StatementMenu()
        {
            Console.WriteLine("\n***** Statement Menu *****\n" +
                "Which account do you want to view?");
            ListAccounts();

            Console.Write("\nPlease enter your choice: ");
            var choice = Console.ReadLine();

            if(Utilities.IsItAnInt(choice) == true)
            {
                int AccountChoice = int.Parse(choice) - 1;
                // 4 transactions per page
                int transactionCount = 1;
                foreach (var transaction in currentCustomer.Accounts[AccountChoice].Transactions)
                {
                    Console.WriteLine("Transaction " + transactionCount + "\n");
                    transaction.getDetails();
                    transactionCount++;
                    Console.WriteLine();
                }
            }
        }

        public void Logout()
        {
            Console.Clear();
            Login();
        }

        public void Exit()
        {
            Console.Clear();
            Console.WriteLine("Exiting the system now.\n" +
                "Thank you for using National Wealth Bank of Australasia!\n\n" +
                "See you next time!");
            Environment.Exit(0);
        }

        public void Deposit()
        {
            int AccountChoice = 0;

            Console.WriteLine("\nWhich account would you like to deposit to?");
            //have to do checking for error
            ListAccounts();
            Console.Write("\nPlease enter your choice: ");
            var choice = Console.ReadLine();

            if(Utilities.IsItAnInt(choice) == true)
            {
                AccountChoice = int.Parse(choice) - 1;
                Console.Write("\n\nHow much do you wish to deposit: $");
                string tempDouble = Console.ReadLine();

                if (Utilities.IsItADouble(tempDouble) == true)
                {
                    var depositAmount = double.Parse(tempDouble);
                    currentCustomer.Accounts[AccountChoice].Balance += depositAmount;
                    Console.WriteLine("Account balance: $" + currentCustomer.Accounts[AccountChoice].Balance);
                }
                else
                {
                    Deposit();
                }
            }
        }

        public void ListAccounts()
        {
            var AccountCount = 1;
            foreach (var account in currentCustomer.Accounts)
            {
                Console.Write(account.AccountNumber + "\t");
                if (account.AccountType == 'S')
                {
                    Console.Write("Savings\t\t");
                }
                else
                {
                    Console.Write("Checking\t");
                }
                Console.WriteLine(AccountCount);
                AccountCount++;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using SimpleHashing;

namespace WDTAssignment
{
     class BankingSys
    {
        public Database db { get; } = new Database();
        public  List<Customer> Customers { get; set; } = new List<Customer>();
        public  List<Logins> Logins { get; set; } = new List<Logins>();
        Customer currentCustomer;


        // Implemement Singleton pattern 
        private static  BankingSys instance; 

        private BankingSys()
        {

        }

        public static BankingSys Instance()
        {
            if (instance == null)
            {
                instance = new BankingSys(); 
            }

            return instance;
        }
       
        public void Login()
        {
            Console.WriteLine();
            Console.Write("LoginID: ");
            var tempString = Console.ReadLine();

            if (Utilities.IsItAnInt(tempString))
            {
                var login = int.Parse(tempString);
                Validate(login);
            }
            else
            {
                Login();
            }
        }

        // pull password from login table
        // using simple hashing to compare passwords for validation
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
                            "or 'b' to return to previous screen: ");
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
            Console.Clear();
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

                if (Utilities.IsItAnInt(tempString))
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
                    if (TransferConfirmation() == true)
                    {
                        AccountTransferMenu();
                    }
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
            while (true)
            {
                Console.Write("\n***** ATM Menu *****\n" +
                    "Deposit               1\n" +
                    "Withdraw              2\n" +
                    "Back to Main Menu     9\n\n" +
                    "Please enter your choice: ");

                var option = Console.ReadLine();
                if (Utilities.IsItAnInt(option) == true)
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
                    Withdraw();
                    break;
                case 9:
                    Console.WriteLine("Returning to main menu.\n");
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
                    TransferOwn();
                    break;
                case 2:
                    TransferThirdParty();
                    break;
                case 9:
                    Console.WriteLine("Returning to main menu.\n");
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
            ListAccountsOptions();

            Console.Write("\nPlease enter your choice: ");
            var choice = Console.ReadLine();

            if (Utilities.IsItAnInt(choice) == true)
            {
                int AccountChoice = int.Parse(choice) - 1;
                ViewStatement(AccountChoice);
            }
        }

        public void Logout()
        {
            Console.Clear();
            Login();
        }

        // see if can close console window
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
            int accountChoice = 0;

            Console.WriteLine("\nWhich account would you like to deposit to?");
            ListAccountsOptions();
            Console.Write("\nPlease enter your choice: ");
            var choice = Console.ReadLine();

            if (Utilities.IsItAnInt(choice) == true && int.Parse(choice) <= currentCustomer.Accounts.Count)
            {
                accountChoice = int.Parse(choice) - 1;
                Account chosenAccount = currentCustomer.Accounts[accountChoice];
                Console.Write("\n\nHow much do you wish to deposit: $");
                string tempDouble = Console.ReadLine();

                if (Utilities.IsItADouble(tempDouble) == true)
                {
                    var depositAmount = double.Parse(tempDouble);
                    chosenAccount.Balance += depositAmount;
                    Console.WriteLine("\n\nDepositing $" + depositAmount + " into account no. " + chosenAccount.AccountNumber + ".\n" +
                        "Account balance: $" + chosenAccount.Balance + "\n");

                    chosenAccount.RecordTransaction(0, 'D', 0, depositAmount);
                    updateDB(chosenAccount);
                }
                else
                {
                    Deposit();
                }
            }
            else
            {
                Console.WriteLine("Not a valid option. Please try again.");
                Deposit();
            }
        }

        public void Withdraw()
        {
            while (WithdrawConfirmation() == true)
            {
                int accountChoice = 0;

                Console.WriteLine("\nWhich account would you like to withdraw from?");
                ListAccountsOptions();
                Console.Write("\nPlease enter your choice: ");
                var choice = Console.ReadLine();

                if (Utilities.IsItAnInt(choice) == true && int.Parse(choice) <= currentCustomer.Accounts.Count)
                {
                    accountChoice = int.Parse(choice) - 1;
                    Account chosenAccount = currentCustomer.Accounts[accountChoice];
                    WithdrawMoney(chosenAccount);
                    updateDB(chosenAccount);
                }
                else
                {
                    Console.WriteLine("Not a valid option. Please try again.");
                    Withdraw();
                }
            }
        }

        public void WithdrawMoney(Account chosenAccount)
        {
            Console.Write("\n\nHow much do you wish to withdraw: $");
            string tempDouble = Console.ReadLine();

            if (Utilities.IsItADouble(tempDouble) == true)
            {
                var withdrawalAmount = double.Parse(tempDouble);

                if (withdrawalAmount <= (chosenAccount.Balance - 0.1))
                {
                    Console.WriteLine("\nWithdrawing $" + withdrawalAmount + " from account no. " + chosenAccount.AccountNumber + ".\n" +
                        "Cost of withdrawal will be $0.10, and will be deducted from account no. " + chosenAccount.AccountNumber + ".\n");

                    var deficit = (withdrawalAmount + 0.1);
                    chosenAccount.Balance -= deficit;
                    chosenAccount.RecordTransaction(0, 'D', 0, deficit);
                    Console.WriteLine("Account balance: $" + chosenAccount.Balance);
                    updateDB(chosenAccount);

                    Console.WriteLine("Returning to main menu.\n\n");
                }
                else
                {
                    Console.WriteLine("You cannot withdraw more than the balance in your account, which is $" + chosenAccount.Balance + ", not including the $0.10 withdrawal fee.\n" +
                        "Please try again.");
                    WithdrawMoney(chosenAccount);
                }
            }
            else
            {
                Console.WriteLine("Not a valid option. Please try again.");
                Withdraw();
            }
        }

        public Boolean WithdrawConfirmation()
        {
            var confirm = false;
            Console.Write("\nWithdrawing from accounts will cost $0.10 cents.\n" +
                "Do you agree to these terms? (y/n): ");
            var response = Console.ReadLine();

            if (response.ToLower().CompareTo("y") == 0)
            {
                confirm = true;
            }
            else if (response.ToLower().CompareTo("n") == 0)
            {
                Console.WriteLine("\nYou do not agree to these terms.\n" +
                    "Returning to Main Menu.\n");
                return confirm;
            }
            else
            {
                Console.WriteLine("\nNot a valid option. Please try again.");
                WithdrawConfirmation();
            }
            return confirm;
        }

        public void TransferOwn()
        {
            if (currentCustomer.Accounts.Count < 2)
            {
                Console.WriteLine("You cannot transfer from your own account as you only have 1 account.\n" +
                    "Please try again.");
                AccountTransferMenu();
            }
            else
            {
                Console.WriteLine("\nWhich account would you like to transfer from?");
                ListAccountsOptions();
                Console.Write("\nPlease enter your choice: ");
                var choice = Console.ReadLine();

                if (Utilities.IsItAnInt(choice) == true && int.Parse(choice) <= currentCustomer.Accounts.Count)
                {
                    int accountChoice = int.Parse(choice) - 1;
                    Account chosenAccount = currentCustomer.Accounts[accountChoice];
                    Account destAccount = currentCustomer.Accounts[(accountChoice + 1) % 2];
                    Console.Write("\n\nTransfer from account no. " + chosenAccount.AccountNumber + " to account no. " + destAccount.AccountNumber + "? (y/n): ");
                    var response = Console.ReadLine();

                    if (response.ToLower().CompareTo("y") == 0)
                    {
                        TransferMoney(chosenAccount, destAccount);
                    }
                    else if (response.ToLower().CompareTo("n") == 0)
                    {
                        Console.WriteLine("\nYou did not agree to the terms.\nReturning to main menu.\n");
                    }
                    else
                    {
                        Console.WriteLine("Not a valid option. Please try again.\n");
                        TransferOwn();
                    }
                }
                else
                {
                    Console.WriteLine("Not a valid option. Please try again.\n");
                    TransferOwn();
                }
            }
        }

        public void TransferThirdParty()
        {
            Console.WriteLine("\nWhich account would you like to transfer from?");
            ListAccountsOptions();
            Console.Write("\nPlease enter your choice: ");
            var choice = Console.ReadLine();

            if (Utilities.IsItAnInt(choice) == true)
            {
                int accountChoice = int.Parse(choice) - 1;
                Account chosenAccount = currentCustomer.Accounts[accountChoice];
                Console.Write("\nPlease enter the account you would like to transfer to: ");
                var tempDestAcc = Console.ReadLine();

                if (Utilities.IsItAnInt(tempDestAcc) == true)
                {
                    var DestAccNo = int.Parse(tempDestAcc);


                    Boolean accountFound = false;

                    for (int i = 0; i < Customers.Count; i++)
                    {
                        for (int j = 0; j < Customers[i].Accounts.Count; j++)
                        {
                            if (Customers[i].Accounts[j].AccountNumber == DestAccNo)
                            {
                                accountFound = true;
                                Account destAccount = Customers[i].Accounts[j];
                                TransferMoney(chosenAccount, destAccount);
                            }
                        }
                    }

                    if (!accountFound)
                    {
                        Console.WriteLine("No such account exists. Returning to main menu.\n");
                    }
                }
            }
        }

        public void TransferMoney(Account chosenAccount, Account destAccount)
        {
            Console.Write("\n\nHow much would you like to transfer: $");
            string tempDouble = Console.ReadLine();

            if (Utilities.IsItADouble(tempDouble) == true)
            {
                var transferAmount = double.Parse(tempDouble);

                if (transferAmount <= (chosenAccount.Balance - 0.2))
                {
                    Console.WriteLine("\nTransferring $" + transferAmount + " from account no. " + chosenAccount.AccountNumber + " to account no. " + destAccount.AccountNumber + ".\n" +
                        "Cost of transfer will be $0.20, and will be deducted from account no. " + chosenAccount.AccountNumber + ".\n");

                    ListAccounts();

                    Console.WriteLine("\n\nTransferring money now.\n");

                    var deficit = (transferAmount + 0.2);
                    chosenAccount.Balance -= deficit;
                    destAccount.Balance += transferAmount;

                    chosenAccount.RecordTransaction(0, 'T', destAccount.AccountNumber, deficit);
                    destAccount.RecordTransaction(0, 'T', chosenAccount.AccountNumber, transferAmount);
                    updateDB(chosenAccount);
                    updateDB(destAccount);

                    Console.WriteLine("Tranfer complete.\n");
                    ListAccounts();
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("\nYou cannot transfer more than the balance in your account, which is $" + chosenAccount.Balance + ", not including the $0.20 tranfer fee.\n" +
                        "Please Try Again.");
                    TransferMoney(chosenAccount, destAccount);
                }
            }
        }

        public Boolean TransferConfirmation()
        {
            var confirm = false;
            Console.Write("\nTransferring between accounts will cost $0.20 cents.\n" +
                "Do you agree to these terms? (y/n): ");
            var response = Console.ReadLine();

            if (response.ToLower().CompareTo("y") == 0)
            {
                confirm = true;
            }
            else if (response.ToLower().CompareTo("n") == 0)
            {
                Console.WriteLine("\nYou did not agree to these terms.\n" +
                    "Returning to Main Menu.\n");
                return confirm;
            }
            else
            {
                Console.WriteLine("\nNot a valid option. Please try again.\n");
                TransferConfirmation();
            }
            return confirm;
        }

        public void ViewStatement(int accountChoice)
        {
            var chosenAccount = currentCustomer.Accounts[accountChoice];
            var transList = chosenAccount.Transactions;
            var totalTransactions = chosenAccount.Transactions.Count;
            double temp = totalTransactions / 4;
            int totalPages = (int)Math.Round(temp) * 4;
            int currentPrintedTransNo = 1;

            printStatementFirstPage(totalPages, totalTransactions, transList, currentPrintedTransNo);
        }

        public void printTransaction(List<Transaction> list, int currentPrintedTransNo, int i)
        {
            Console.WriteLine("\nTransaction " + currentPrintedTransNo);
            list[i].getDetails();
            Console.WriteLine("\n----------------------------------------");
        }

        public void printStatementFirstPage(int totalPages, int totalTrans, List<Transaction> list, int currentPrintedTransNo)
        {
            var printedTransCount = 0;
            var currentPage = 1;

            Console.WriteLine("\n\nPage No. " + currentPage);
            for (int i = (totalTrans - 1); i >= 0; i--)
            {
                printTransaction(list, currentPrintedTransNo, i);
                currentPrintedTransNo++;
                printedTransCount++;

                if(printedTransCount == 4)
                {
                    break;
                }
            }
            printStatementPageEndChoice(currentPage, totalPages, totalTrans, list, currentPrintedTransNo);
        }

        public void printStatementNextCalledPage(int currentPage, int totalPages, int totalTrans, List<Transaction> list, int currentPrintedTransNo)
        {
            var printedTransCount = 0;
            
            Console.WriteLine("\n\nPage No. " + currentPage);
            for(int i = (totalTrans - currentPrintedTransNo); i >= 0; i--)
            {
                var id = list[i].TransactionID;
                printTransaction(list, currentPrintedTransNo, i);
                currentPrintedTransNo++;
                printedTransCount++;

                if (printedTransCount == 4)
                {
                    break;
                }
            }
            printStatementPageEndChoice(currentPage, totalPages, totalTrans, list, currentPrintedTransNo);
        }

        public void printStatementPageEndChoice(int currentPage, int totalPages, int totalTrans, List<Transaction> list, int currentPrintedTransNo)
        {
            if (totalPages == 1)
            {
                Console.WriteLine("Returning to main menu.\n");
            }
            else if (currentPage == 1 && totalPages > 1)
            {
                Console.WriteLine("Main Menu (m)\t\tNext Page (n)");
            }
            else if (currentPage >= 2 && currentPage < totalPages)
            {
                Console.WriteLine("Previous Page (p)\t\tMain Menu (m)\t\tNext Page (n)");
            }
            else if (currentPage >= 2 && currentPage == totalPages)
            {
                Console.WriteLine("Previous Page (p)\t\tMain Menu (m)");
            }
            string response = Console.ReadLine();

            while (response.ToLower() != "m")
            {
                if(response.ToLower() == "n")
                {
                    currentPage++;
                    printStatementNextCalledPage(currentPage, totalPages, totalTrans, list, currentPrintedTransNo);
                }
                else if(response.ToLower() == "p")
                {
                    currentPage -= 1;
                    currentPrintedTransNo -= 8; // this will have array out of bounds

                    printStatementNextCalledPage(currentPage, totalPages, totalTrans, list, currentPrintedTransNo);
                } 
                else
                {
                    Console.WriteLine("\nInvalid option. Returning to main menu.\n");
                    MainMenu();
                }
            }
            if (response.ToLower() == "m")
            {
                Console.WriteLine("Returning to main menu.\n");
                return;
            }
        }

        public void ListAccountsOptions()
        {
            var accountCount = 1;
            Console.WriteLine("----------------------------------------------------\n" +
                              "| ACCOUNT NUMBER | ACCOUNT TYPE | BALANCE | OPTION |\n" +
                              "----------------------------------------------------");
            foreach (var account in currentCustomer.Accounts)
            {
                Console.Write("  " + account.AccountNumber + "\t\t   ");
                if (account.AccountType == 'S')
                {
                    Console.Write("Savings\t  ");
                }
                else
                {
                    Console.Write("Checking\t  ");
                }
                Console.WriteLine("$" + account.Balance + "\t    " + accountCount);
                accountCount++;
            }
        }

        public void ListAccounts()
        {
            var accountCount = 1;
            Console.WriteLine("-------------------------------------------\n" +
                              "| ACCOUNT NUMBER | ACCOUNT TYPE | BALANCE |\n" +
                              "-------------------------------------------");
            foreach (var account in currentCustomer.Accounts)
            {
                Console.Write("  " + account.AccountNumber + "\t\t   ");
                if (account.AccountType == 'S')
                {
                    Console.Write("Savings\t  ");
                }
                else
                {
                    Console.Write("Checking\t  ");
                }
                Console.WriteLine("$" + account.Balance);
                accountCount++;
            }
        }

        public void updateDB(Account chosenAccount)
        {
            db.InsertTransaction(chosenAccount);
            db.UpdateBalance(chosenAccount);
            chosenAccount.Transactions[(chosenAccount.Transactions.Count - 1)].TransactionID = db.GetTransactionID();
        }
    }
}

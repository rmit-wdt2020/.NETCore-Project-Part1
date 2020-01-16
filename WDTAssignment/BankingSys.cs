using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using SimpleHashing;

namespace WDTAssignment
{
    class BankingSys
    {
        public Database db { get; set; } = new Database();
        public List<Customer> Customers { get; set; } = new List<Customer>();
        public List<Logins> Logins { get; set; } = new List<Logins>();
        Customer currentCustomer;


        // Implemement Singleton pattern 
        private static BankingSys instance;

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


        // User to enter login ID to access accounts
        public void Login()
        {
            Console.WriteLine();
            Console.Write("LoginID: ");
            var tempString = Console.ReadLine();

            if (Utilities.IsItAnInt(tempString))
            {
                var loginID = int.Parse(tempString);
                Validate(loginID);
            }
            else
            {
                Login();
            }
        }

        // user to enter unhashed password
        // program will validate password, and brings user into banking system
        // if the password is correct
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

                        if (PBKDF2.Verify(customer.Password, password) == true)
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

        // prints the main menu, and asks user to choose an option
        public void MainMenu()
        {
            Console.Clear();
            while (true)
            {
                Console.Write("\n***** National Wealth Bank of Australasia System Menu ******\n" +
                                "ATM:                            1\n" +
                                "Account Transfer:               2\n" +
                                "View Statement:                 3\n" +
                                "Logout:                         9\n" +
                                "Exit:                           0\n" +
                                "Please enter your choice: ");
                var tempString = Console.ReadLine();

                //checks if user's choice is an int or not
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

        // switch for the main menu method
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
                        Console.Clear();
                        AccountTransferMenu();
                    }
                    break;

                case 3:
                    Console.Clear();
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

        // prints out the ATM menu
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

        // switch for the ATM menu method
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

        // prints the account transfer menu
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

        // switch for the account transfer menu method
        public void TransferChoice(int option)
        {
            Console.Clear();
            switch (option)
            {
                case 1:
                    TransferOwn();
                    break;
                case 2:
                    TransferThirdParty();
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

        // prints the statement menu
        public void StatementMenu()
        {
            Console.WriteLine("\n***** Statement Menu *****\n" +
                "Which account do you want to view?");
            ListAccountsOptions();

            Console.Write("\nPlease enter your choice, or enter 'c' to cancel: ");
            var choice = Console.ReadLine();

            if (choice.ToLower() == "c")
            {
                Console.Clear();
                return;
            }
            // sets the chosen account, and prints the chosen account's statement
            else if (Utilities.IsItAnInt(choice) == true && int.Parse(choice) <= currentCustomer.Accounts.Count)
            {
                int AccountChoice = int.Parse(choice) - 1;
                ViewStatement(AccountChoice);
            }
            else
            {
                Console.WriteLine("Not a valid option. Please try again.\n");
                StatementMenu();
            }
        }

        // clears the console and runs the login again
        public void Logout()
        {
            Console.Clear();
            Login();
        }

        // exits the program
        public void Exit()
        {
            Console.Clear();
            Console.WriteLine("Exiting the system now.\n" +
                "Thank you for using National Wealth Bank of Australasia!\n\n" +
                "See you next time!");
            Environment.Exit(0);
        }

        // 1. runs the deposit function
        // 2. asks user to enter account choice
        // 3. asks user to enter deposit amount
        // 4. deposits amount --> updates balance and records transaction
        public void Deposit()
        {
            int accountChoice;

            Console.Clear();
            Console.WriteLine("\nWhich account would you like to deposit to?");
            ListAccountsOptions();
            Console.Write("\nPlease enter your choice, or enter 'c' to cancel: ");
            var choice = Console.ReadLine();

            if (choice.ToLower() == "c")
            {
                Console.Clear();
                return;
            }
            else if (Utilities.IsItAnInt(choice) == true && int.Parse(choice) <= currentCustomer.Accounts.Count)
            {
                accountChoice = int.Parse(choice) - 1;
                Account chosenAccount = currentCustomer.Accounts[accountChoice];
                Console.Write("\n\nHow much do you wish to deposit: $");
                string tempDouble = Console.ReadLine();

                if (Utilities.IsItADouble(tempDouble) == true)
                {
                    var depositAmount = double.Parse(tempDouble);
                    // checks to see if the amount entered is more than 0
                    try
                    {
                        Utilities.ValidateNotZeroOrNegativeAmt(depositAmount);
                    }
                    catch (TransactionZeroAmountException e)
                    {

                        Console.WriteLine(e.Message);
                        return;
                    }
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

        // 1. runs the withdraw function
        // 2. asks user to enter account choice
        // 3. asks user to enter withdraw amount
        // 4. deposits amount --> updates balance and records transaction
        public void Withdraw()
        {
            if (WithdrawConfirmation() == true)
            {
                Console.Clear();
                Console.WriteLine("\nWhich account would you like to withdraw from?");
                ListAccountsOptions();
                Console.Write("\nPlease enter your choice, or enter 'c' to cancel: ");
                var choice = Console.ReadLine();

                if (choice.ToLower() == "c")
                {
                    Console.Clear();
                    return;
                }
                else if (Utilities.IsItAnInt(choice) == true && int.Parse(choice) <= currentCustomer.Accounts.Count)
                {
                    var accountChoice = int.Parse(choice) - 1;
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
            else
            {
                Console.WriteLine("\nYou do not agree to these terms.\n" +
                    "Returning to Main Menu.\n");
                return;
            }
        }

        // helper method for the withdraw method
        // checks to see if the withdraw amount entered is more than 0
        // and records transaction and updates balance if everything is ok
        // prints the account balance at the end
        public void WithdrawMoney(Account chosenAccount)
        {
            Console.Write("\n\nHow much do you wish to withdraw: $");
            string tempDouble = Console.ReadLine();

            if (Utilities.IsItADouble(tempDouble) == true)
            {
                var withdrawalAmount = double.Parse(tempDouble);

                try
                {
                    Utilities.ValidateNotZeroOrNegativeAmt(withdrawalAmount);
                }
                catch (TransactionZeroAmountException e)
                {

                    Console.WriteLine(e.Message);
                    Console.WriteLine("Returning to main menu.\n");
                    return;
                }

                if (withdrawalAmount <= (chosenAccount.Balance - 0.1))
                {
                    Console.WriteLine("\nWithdrawing $" + withdrawalAmount + " from account no. " + chosenAccount.AccountNumber + ".\n" +
                        "Cost of withdrawal will be $0.10, and will be deducted from account no. " + chosenAccount.AccountNumber + ".\n");

                    var deficit = (withdrawalAmount + 0.1);
                    chosenAccount.Balance -= deficit;
                    chosenAccount.RecordTransaction(0, 'W', 0, deficit);
                    Console.WriteLine("Account balance: $" + chosenAccount.Balance);

                    Console.WriteLine("Returning to main menu.\n");
                }
                else
                {
                    Console.WriteLine("You cannot withdraw more than the balance in your account, which is $" + chosenAccount.Balance + ", not including the $0.10 withdrawal fee.\n" +
                        "Returning to main menun");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Not a valid option. Please try again.");
                Withdraw();
            }
        }

        // checks to see if the user is ok with the fee incurred for withdrawal
        // if yes, proceed to the withdraw method
        // if no, returns to main menu
        public Boolean WithdrawConfirmation()
        {
            var confirm = false;
            Console.Write("\nWithdrawing from accounts will cost $0.10 cents.\n" +
                "Do you agree to these terms? (y/n): ");
            var response = Console.ReadLine();

            while (response.ToLower() != "n" && response.ToLower() != "y")
            {
                Console.WriteLine("\nNot a valid option. Please try again.");

                Console.Write("\nWithdrawing from accounts will cost $0.10 cents.\n" +
                    "Do you agree to these terms? (y/n): ");
                response = Console.ReadLine();
            }

            if (response.ToLower().CompareTo("y") == 0)
            {
                confirm = true;
            }
            return confirm;
        }

        // runs the transfer method to transfer between user's own accounts
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
                Console.Write("\nPlease enter your choice, or enter 'c' to cancel: ");
                var choice = Console.ReadLine();

                if (choice.ToLower() == "c")
                {
                    Console.Clear();
                    return;
                }
                else if (Utilities.IsItAnInt(choice) == true && int.Parse(choice) <= currentCustomer.Accounts.Count)
                {
                    // sets the chosen account to transfer from
                    // and automatically sets the account to transfer to

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

        // runs the transfer method to transfer from user's account to third-party accounts
        public void TransferThirdParty()
        {
            Console.WriteLine("\nWhich account would you like to transfer from?");
            ListAccountsOptions();
            Console.Write("\nPlease enter your choice. or enter 'c' to cancel: ");
            var choice = Console.ReadLine();

            if (choice.ToLower() == "c")
            {
                Console.Clear();
                return;
            }
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

                    // finds the account that the user entered
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
                        Console.WriteLine("No such account exists. Returning to previous menu.\n");
                    }
                }
            }
        }

        // helper method for the transfer method
        // checks to see if the transfer amount entered is more than 0
        // and records transaction and updates balance if everything is ok
        // prints account balance at the end
        public void TransferMoney(Account chosenAccount, Account destAccount)
        {
            Console.Write("\n\nHow much would you like to transfer: $");
            string tempDouble = Console.ReadLine();

            if (Utilities.IsItADouble(tempDouble) == true)
            {
                var transferAmount = double.Parse(tempDouble);

                if (transferAmount <= (chosenAccount.Balance - 0.2))
                {
                    try
                    {
                        Utilities.ValidateNotZeroOrNegativeAmt(transferAmount);
                    }
                    catch (TransactionZeroAmountException e)
                    {

                        Console.WriteLine(e.Message);
                        Console.WriteLine("Returning to previous menu.\n");
                        return;
                    }
                    Console.Clear();
                    Console.WriteLine("\nTransferring $" + transferAmount + " from account no. " + chosenAccount.AccountNumber + " to account no. " + destAccount.AccountNumber + ".\n" +
                        "Cost of transfer will be $0.20, and will be deducted from account no. " + chosenAccount.AccountNumber + ".\n");

                    Console.WriteLine("\nBEFORE TRANSFER\n" +
                        "**************************************************\n\n");
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
                    Console.WriteLine("\nAFTER TRANSFER\n" +
                        "**************************************************\n\n");
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

        // checks to see if the user is ok with the fee incurred for transferral
        // if yes, proceed to the transfer method
        // if no, returns to main menu
        public Boolean TransferConfirmation()
        {
            var confirm = false;
            Console.Write("\nTransferring of money will cost $0.00 cents.\n" +
                "Do you agree to these terms? (y/n): ");
            var response = Console.ReadLine();

            while (response.ToLower() != "n" && response.ToLower() != "y")
            {
                Console.WriteLine("\nNot a valid option. Please try again.");

                Console.Write("\nTransferring of money will cost $0.10 cents.\n" +
                    "Do you agree to these terms? (y/n): ");
                response = Console.ReadLine();
            }

            if (response.ToLower().CompareTo("y") == 0)
            {
                confirm = true;
            }
            return confirm;
        }
        
        // runs the view statement method
        public void ViewStatement(int accountChoice)
        {
            var chosenAccount = currentCustomer.Accounts[accountChoice];
            var transList = chosenAccount.Transactions;
            double totalTransactions = chosenAccount.Transactions.Count;
            double temp = totalTransactions / 4;
            int totalPages = (int)Math.Ceiling(temp);
            int currentPage = 0;
            printStatementPage(chosenAccount, currentPage, totalPages, transList);
        }

        // prints 1 transaction on the statement page
        public void printTransaction(List<Transaction> list, int currentPrintedTransNo, int i)
        {
            Console.WriteLine("\nTransaction " + currentPrintedTransNo);
            list[i].getDetails();
            Console.WriteLine("\n--------------------------------------------------");
        }

        // prints the account statement page
        // prints 4 transactions on 1 page
        public void printStatementPage(Account chosenAccount, int currentPage, int totalPages, List<Transaction> list)
        {
            Console.Clear();
            var printedTransNo = (list.Count - 1);
            var printedTransCount = 1;

            Console.WriteLine("\nAccount No. " + chosenAccount.AccountNumber);
            for (int i = currentPage; i < totalPages; i++)
            {
                currentPage++;
                Console.WriteLine("Statement Page No. " + currentPage + "\n" +
                    "--------------------------------------------------");

                if (currentPage != 1)
                {
                    printedTransCount = ((currentPage - 1) * 4) + 1;
                }

                // prints 4 transactions or less
                for (int j = 4; j > 0; j--)
                {
                    printedTransNo++;
                    var currentTransPos = list.Count - printedTransCount;
                    printTransaction(list, printedTransCount, currentTransPos);
                    if (printedTransCount == list.Count)
                    {
                        break;
                    }
                    printedTransCount++;
                }
                printStatementPageEndChoice(chosenAccount, currentPage, totalPages, list, printedTransNo);
            }
        }

        // prints the choice to go to previous page, next page, or back to previous menu
        // at the end of the statement page
        // runs the chosen option
        public void printStatementPageEndChoice(Account chosenAccount, int currentPage, int totalPages, List<Transaction> list, int currentPrintedTransNo)
        {
            if (totalPages == 1)
            {
                Console.WriteLine("Press 'Enter' to return to main menu.\n");
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
                if (response.ToLower() == "n" && currentPage == totalPages)
                {
                    Console.WriteLine("\nERROR: Already on last page.\n" +
                        "Please try again.\n\n");
                    printStatementPageEndChoice(chosenAccount, currentPage, totalPages, list, currentPrintedTransNo);
                }
                else if (response.ToLower() == "n")
                {
                    printStatementPage(chosenAccount, currentPage, totalPages, list);
                }
                else if (response.ToLower() == "p" && currentPage == 1)
                {
                    Console.WriteLine("\nERROR: Already on first page.\n" +
                        "Please try again.\n\n");
                    printStatementPageEndChoice(chosenAccount, currentPage, totalPages, list, currentPrintedTransNo);
                }
                else if (response.ToLower() == "p")
                {
                    currentPage -= 2;
                    printStatementPage(chosenAccount, currentPage, totalPages, list);

                }
                else
                {
                    Console.WriteLine("\nInvalid option. Returning to main menu.\n");
                    MainMenu();
                }
            }
            if (response.ToLower() == "m")
            {
                MainMenu();
            }
        }

        // prints the list of the user's accounts as options
        public void ListAccountsOptions()
        {
            var accountCount = 1;
            Console.WriteLine("------------------------------------------------------------\n" +
                              "| OPTION | ACCOUNT NUMBER | ACCOUNT TYPE | ACCOUNT BALANCE |\n" +
                              "------------------------------------------------------------");
            foreach (var account in currentCustomer.Accounts)
            {
                Console.Write("  " + accountCount + "\t   " + account.AccountNumber + "\t\t    ");
                if (account.AccountType == 'S')
                {
                    Console.Write("Savings");
                }
                else
                {
                    Console.Write("Checking");
                }
                Console.WriteLine("\t   $" + account.Balance.ToString("#,##0.00"));
                accountCount++;
            }
        }

        // prints the list of the user's accounts, without options
        public void ListAccounts()
        {
            var accountCount = 1;
            Console.WriteLine("---------------------------------------------------\n" +
                              "| ACCOUNT NUMBER | ACCOUNT TYPE | ACCOUNT BALANCE |\n" +
                              "---------------------------------------------------");
            foreach (var account in currentCustomer.Accounts)
            {
                Console.Write("  " + account.AccountNumber + "\t\t    ");
                if (account.AccountType == 'S')
                {
                    Console.Write("Savings");
                }
                else
                {
                    Console.Write("Checking");
                }
                Console.WriteLine("\t   $" + account.Balance.ToString("#,##0.00"));
                accountCount++;
            }
        }

        // updates the transaction table in the database after every transaction
        public void updateDB(Account chosenAccount)
        {
            db.InsertTransaction(chosenAccount);
            db.UpdateBalance(chosenAccount);
            chosenAccount.Transactions[(chosenAccount.Transactions.Count - 1)].TransactionID = db.GetTransactionID();
        }
    }
}

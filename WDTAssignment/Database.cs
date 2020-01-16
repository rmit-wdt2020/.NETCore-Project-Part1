// References some code from lectures by Shekar Kalra
// as well as some code from tutorials by Matthew Bolger

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace WDTAssignment
{
    class Database
     
    {
        private static IConfigurationRoot Configuration { get; } = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        private static string ConnectionString { get; } = Configuration["ConnectionString"];

        // Get all tables in the database and map into Objects in the program 
        public void ImportDatabase()
        {
            DownloadLoginArray();
            DownloadCustomerArray();
        }

        // Get all objects in the program and updates the database. NOTE: Need to clear database first. 
        public void ExportDatabase()
        {

            InsertAllCustomers();
            InsertAllLogins();
            InsertAllAccounts();
            InsertAllTransactions();
        }

        // NEED: Insert the last transaction in the List to the database Transaction table 
        public void InsertTransaction(Account account)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);

            try
            {
                conn.Open();

                var insertTransaction = conn.CreateCommand();

                insertTransaction.CommandText = "insert into [transaction] (TransactionType, AccountNumber, DestinationAccountNumber, Amount, Comment, TransactionTimeUTC) " +
                                "values (@transactionType, @accountNumber, @destinationAccountNumber, @amount, @comment, @transactionTimeUTC)";

                // Gets last transaction in the List 
                insertTransaction.Parameters.AddWithValue("transactionType", account.Transactions[account.Transactions.Count - 1].TransactionType);
                insertTransaction.Parameters.AddWithValue("accountNumber", account.Transactions[account.Transactions.Count - 1].AccountNumber);
                insertTransaction.Parameters.AddWithValue("destinationAccountNumber", account.Transactions[account.Transactions.Count - 1].DestinationAccountNumber);
                insertTransaction.Parameters.AddWithValue("amount", account.Transactions[account.Transactions.Count - 1].Amount);
                insertTransaction.Parameters.AddWithValue("transactionTimeUTC", account.Transactions[account.Transactions.Count - 1].TransactionTimeUTC);

                if (account.Transactions[account.Transactions.Count - 1].Comment != "")
                {
                    insertTransaction.Parameters.AddWithValue("comment", account.Transactions[account.Transactions.Count - 1].Comment);
                }
                else
                {
                    insertTransaction.Parameters.AddWithValue("comment", DBNull.Value);
                }

                insertTransaction.ExecuteNonQuery();
            }
            catch (SqlException se)
            {
                Console.WriteLine("Exception: {0}", se.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        // NEED: Update the balance of the account being passed into this method 
        public void UpdateBalance(Account account)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);

            try
            {
                conn.Open();

                var updateBalance = conn.CreateCommand();

                updateBalance.CommandText = "update account set balance = " + account.Balance + " where accountnumber = " + account.AccountNumber;

                updateBalance.ExecuteNonQuery();
            }
            catch (SqlException se)
            {
                Console.WriteLine("Exception: {0}", se.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }


        // NEED: Method to map Login Table into Login object in the program 
        public void DownloadLoginArray()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);

            try
            {
                conn.Open();

                var LoginQuery = "select * from login";

                SqlDataReader ReadLogin;
                using (SqlCommand LoginCmd = new SqlCommand(LoginQuery, conn))
                {
                    using (ReadLogin = LoginCmd.ExecuteReader())
                    {
                        // Create new login object and add it into Logins List belonging to BankingSys 
                        while (ReadLogin.Read())
                        {
                            Logins login = new Logins(ReadLogin["LoginID"].ToString(), int.Parse(ReadLogin["CustomerID"].ToString()), ReadLogin["PasswordHash"].ToString());
                            BankingSys.Instance().Logins.Add(login);
                        }
                    }
                }
            }
            catch (SqlException se)
            {
                Console.WriteLine("Exception: {0}", se.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        // NEED: Method to map Customer, Account and Transaction table into corresponding objects in the program 
        public void DownloadCustomerArray()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            try
            {
                conn.Open();

                string CustomerQuery = "select * from customer";

                SqlDataReader ReadLogin;
                SqlCommand LoginCmd;
                SqlDataReader ReadCustomer;
                SqlDataReader ReadAccount;
                SqlCommand AccountCmd;
                SqlCommand TransactionCmd;
                SqlDataReader ReadTransaction;

                using (SqlCommand CustomerCmd = new SqlCommand(CustomerQuery, conn))
                {
                    using (ReadCustomer = CustomerCmd.ExecuteReader())
                    {
                        while (ReadCustomer.Read())
                        {
                            Customer customer = new Customer(0, "", int.Parse(ReadCustomer["CustomerID"].ToString()), ReadCustomer["Name"].ToString(),
                                ReadCustomer["Address"].ToString(), ReadCustomer["City"].ToString(), ReadCustomer["PostCode"].ToString());

                            BankingSys.Instance().Customers.Add(customer);
                        }
                    }
                }

                foreach (var customer in BankingSys.Instance().Customers)
                {

                    using (LoginCmd = new SqlCommand("select loginID, passwordhash from login where customerID = " + customer.CustomerID, conn))
                    {
                        using (ReadLogin = LoginCmd.ExecuteReader())
                        {

                            while (ReadLogin.Read())
                            {
                                customer.LoginID = int.Parse(ReadLogin["loginID"].ToString());
                                customer.Password = ReadLogin["PasswordHash"].ToString();
                            }
                        }
                    }
                }

                foreach (var customer in BankingSys.Instance().Customers)
                {
                    using (AccountCmd = new SqlCommand("select accountnumber, accounttype, balance from account where customerID = " + customer.CustomerID, conn))
                    {
                        using (ReadAccount = AccountCmd.ExecuteReader())
                        {
                            while (ReadAccount.Read())
                            {
                                Account account; 
                                
                                if (ReadAccount["AccountType"].ToString().Contains('S'))
                                {
                                    account = AccountFactory.Create('S') as SavingsAccount;
                                    account.AccountNumber = int.Parse(ReadAccount["AccountNumber"].ToString());
                                    account.CustomerID = customer.CustomerID;
                                    account.Balance = double.Parse(ReadAccount["Balance"].ToString());

                                    customer.Accounts.Add(account);
                            
                                }
                                else if (ReadAccount["AccountType"].ToString().Contains('C'))
                                {
                                    account = AccountFactory.Create('C') as CheckingAccount;
                                    account.AccountNumber = int.Parse(ReadAccount["AccountNumber"].ToString());
                                    account.CustomerID = customer.CustomerID;
                                    account.Balance = double.Parse(ReadAccount["Balance"].ToString());

                                    customer.Accounts.Add(account);
                                }

                                    
                                
                            }
                        }
                    }
                }


                foreach (var customer in BankingSys.Instance().Customers)
                {
                    foreach (var account in customer.Accounts)
                    {
                        using (TransactionCmd = new SqlCommand("select [transactionid], [transactiontype], [accountnumber], [destinationaccountnumber]," +
                            " [amount], [comment], [transactiontimeutc] from [transaction] where [accountnumber] = " + account.AccountNumber, conn))
                        {
                            using (ReadTransaction = TransactionCmd.ExecuteReader())
                            {
                                while (ReadTransaction.Read())
                                {


                                    Transaction transaction = new Transaction(int.Parse(ReadTransaction["TransactionID"].ToString()), Convert.ToChar(ReadTransaction["TransactionType"].ToString()),
                                       int.Parse(ReadTransaction["AccountNumber"].ToString()), int.Parse(ReadTransaction["DestinationAccountNumber"].ToString()),
                                       double.Parse(ReadTransaction["Amount"].ToString()), null, DateTime.Parse(ReadTransaction["TransactionTimeUTC"].ToString()));

                                    if (ReadTransaction["Comment"] != null)
                                    {
                                        transaction.Comment = ReadTransaction["Comment"].ToString();
                                    }

                                    account.Transactions.Add(transaction);

                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException se)
            {
                Console.WriteLine("Exception: {0}", se.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        // Method to push ALL Transaction objects into database 
        public void InsertAllTransactions()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);

            foreach (var customer in BankingSys.Instance().Customers)
            {
                foreach (var account in customer.Accounts)
                {
                    foreach (var transaction in account.Transactions)
                    {
                        try
                        {
                            conn.Open();

                            var populateTransactions = conn.CreateCommand();

                            populateTransactions.CommandText = "insert into [transaction] (TransactionType, AccountNumber, DestinationAccountNumber, Amount, TransactionTimeUTC) " +
                                "values (@transactionType, @accountNumber, @destinationAccountNumber, @amount, @transactionTimeUTC)";
                            populateTransactions.Parameters.AddWithValue("transactionType", transaction.TransactionType);
                            populateTransactions.Parameters.AddWithValue("accountNumber", account.AccountNumber);
                            populateTransactions.Parameters.AddWithValue("destinationAccountNumber", account.AccountNumber);
                            populateTransactions.Parameters.AddWithValue("amount", account.Balance);
                            populateTransactions.Parameters.AddWithValue("transactionTimeUTC", transaction.TransactionTimeUTC);


                            populateTransactions.ExecuteNonQuery();
                        }
                        catch (SqlException se)
                        {
                            Console.WriteLine("Exception: {0}", se.Message);
                        }
                        finally
                        {
                            if (conn.State == ConnectionState.Open)
                            {
                                conn.Close();
                            }
                        }

                    }
                }

            }
        }

        // Method to push ALL Login objects into database 
        public void InsertAllLogins()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);

            try
            {
                conn.Open();

                foreach (var login in BankingSys.Instance().Logins)
                {
                    var PopulateLogin = conn.CreateCommand();

                    PopulateLogin.CommandText = "insert into login (LoginID, CustomerID, PasswordHash) values (@loginID, @customerID, @passwordHash)";
                    PopulateLogin.Parameters.AddWithValue("loginID", login.LoginID);
                    PopulateLogin.Parameters.AddWithValue("customerID", login.CustomerID);
                    PopulateLogin.Parameters.AddWithValue("passwordHash", login.PasswordHash);

                    PopulateLogin.ExecuteNonQuery();
                }
            }
            catch (Exception se)
            {
                Console.WriteLine("Exception: {0}", se.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        // Method to push ALL Customer objects into database 
        public void InsertAllCustomers()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);

            try
            {
                conn.Open();

                foreach (var customer in BankingSys.Instance().Customers)
                {
                    var PopulateCustomer = conn.CreateCommand();

                    PopulateCustomer.CommandText = "insert into customer (CustomerID, Name, Address, City, PostCode) values (@customerID, @name, @address, @city, @postCode)";
                    PopulateCustomer.Parameters.AddWithValue("customerID", customer.CustomerID);
                    PopulateCustomer.Parameters.AddWithValue("name", customer.Name);


                    // Some parameters requires check for NULL values
                    if (customer.Address == null)
                    {
                        PopulateCustomer.Parameters.AddWithValue("address", "NULL");
                    }
                    else
                    {
                        PopulateCustomer.Parameters.AddWithValue("address", customer.Address);
                    }

                    if (customer.City == null)
                    {
                        PopulateCustomer.Parameters.AddWithValue("city", "NULL");
                    }
                    else
                    {
                        PopulateCustomer.Parameters.AddWithValue("city", customer.City);
                    }

                    if (customer.City == null)
                    {
                        PopulateCustomer.Parameters.AddWithValue("postCode", "NULL");
                    }
                    else
                    {
                        PopulateCustomer.Parameters.AddWithValue("postCode", customer.PostCode);
                    }

                    PopulateCustomer.ExecuteNonQuery();
                }

            }
            catch (Exception se)
            {
                Console.WriteLine("Exception: {0}", se.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        // Method to push ALL Account objects into database 
        public void InsertAllAccounts()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);

            foreach (var customer in BankingSys.Instance().Customers)
            {
                foreach (var account in customer.Accounts)
                {
                    try
                    {
                        conn.Open();

                        var populateAccounts = conn.CreateCommand();

                        populateAccounts.CommandText = "insert into account (AccountNumber, AccountType, CustomerID, Balance) values (@accountNumber, @accountType, @customerID, @balance)";
                        populateAccounts.Parameters.AddWithValue("accountNumber", account.AccountNumber);
                        populateAccounts.Parameters.AddWithValue("accountType", account.AccountType);
                        populateAccounts.Parameters.AddWithValue("customerID", account.CustomerID);
                        populateAccounts.Parameters.AddWithValue("balance", account.Balance);

                        populateAccounts.ExecuteNonQuery();

                    }
                    catch (SqlException se)
                    {
                        Console.WriteLine("Exception: {0}", se.Message);
                    }
                    finally
                    {
                        if (conn.State == ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
            }
        }

        public int GetTransactionID()
        {
            int transactionID = 0;
            SqlCommand getTransactionIDCmd;
            SqlDataReader readTransactionID; 

            SqlConnection conn = new SqlConnection(ConnectionString);

            try
            {
                conn.Open();

                using (getTransactionIDCmd = new SqlCommand("select top (1) TransactionID  FROM [Transaction] order by TransactionID desc", conn))
                {
                    using (readTransactionID = getTransactionIDCmd.ExecuteReader())
                    {
                        while(readTransactionID.Read())
                        {
                            transactionID = int.Parse(readTransactionID["TransactionID"].ToString());
                        }
                    }
                }
            }
            catch (SqlException se)
            {
                Console.WriteLine("Exception: {0}", se.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return transactionID;

        }
    }
}

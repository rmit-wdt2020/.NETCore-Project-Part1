// References some code from lectures by Shekar Kalra
// as well as some code from tutorials by Matthew Bolger

using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Converters;
using System.Data;
using Microsoft.Extensions.Configuration; 


namespace WDTAssignment
{
    class JSON
    {
        public List<Logins> Logins { get; }
        public List<Customer> Customer { get; }
        public List<Account> Accounts { get; }
        public List<Transaction> Transaction { get; }

        private static IConfigurationRoot Configuration { get; } = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        private static string ConnectionString { get; } = Configuration["ConnectionString"];

        public void PopulateDatabase()
        {
            SqlCommand Command = new SqlCommand();

            SqlConnection conn = new SqlConnection(ConnectionString);

            try
            {
                conn.Open();

                // Check if the database contains any data
                Command.Connection = conn;
                Command.CommandText = "select count(*) from customer";

                int result = int.Parse(Command.ExecuteScalar().ToString()); 

                // Populate database from JSON if database is empty, else don't do anything
                if(result == 0)
                {
                    PopulateCustomer();
                    PopulateLogin();
                    PopulateAccount();
                    PopulateTransaction();
                }

            }
            catch (Exception se)
            {
                Console.WriteLine("Exception: {0}", se.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

        }

        // Clear Database before inserting values deserialized from JSON file 
        public void ClearDatabase()
        {
            SqlConnection conn = new SqlConnection(ConnectionString);

            try
            {
                conn.Open();

                // Clear Transaction table 
                var deleteTransaction = conn.CreateCommand();
                deleteTransaction.CommandText = "delete from [transaction]";
                deleteTransaction.ExecuteNonQuery();

                // Clear Account table 
                var deleteAccount = conn.CreateCommand();
                deleteAccount.CommandText = "delete from account";
                deleteAccount.ExecuteNonQuery();

                //Clear Login table 
                var deleteLogin = conn.CreateCommand();
                deleteLogin.CommandText = "delete from login";
                deleteLogin.ExecuteNonQuery();

                // Clear Customer table 
                var deleteCustomer = conn.CreateCommand();
                deleteCustomer.CommandText = "delete from customer";
                deleteCustomer.ExecuteNonQuery();

                // NOTE: Deletion will have to be in this order so as to not violate Relational Database constraints 

            }
            catch (Exception se)
            {
                Console.WriteLine("Exception: {0}", se.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        // Deserializes JSON and inserts values to Account table
        public void PopulateAccount()
        {
            using var client = new HttpClient();
            var jsonAccounts = client.GetStringAsync("https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/customers/").Result;
            var customers = JsonConvert.DeserializeObject<List<Customer>>(jsonAccounts, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy hh:mm:ss tt" });

            SqlConnection conn = new SqlConnection(ConnectionString);

            foreach (var customer in customers)
            {

                foreach (var account in customer.Accounts)
                {
                    InsertAccounts(account);
                }
            }

        }
        public void InsertAccounts(Account account)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);

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
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

        }

        // Deserializes JSON and inserts values to Login table
        public void PopulateTransaction()
        {
            //Console.WriteLine("TEST: This method is called.");

            using var client = new HttpClient();
            var json = client.GetStringAsync("https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/customers/").Result;


            var customers = JsonConvert.DeserializeObject<List<Customer>>(json, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy hh:mm:ss tt" });

            SqlConnection conn = new SqlConnection(ConnectionString);

            foreach (var customer in customers)
            {
                foreach (var account in customer.Accounts)
                {
                    foreach (var transaction in account.Transactions)
                    {
                        InsertTransactions(transaction, account);
                    }
                }
            }
        }
        public void InsertTransactions(Transaction transactions, Account account)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);

            try
            {
                conn.Open();

                var populateTransactions = conn.CreateCommand();

                populateTransactions.CommandText = "insert into [transaction] (TransactionType, AccountNumber, DestinationAccountNumber, Amount, TransactionTimeUTC) " +
                    "values (@transactionType, @accountNumber, @destinationAccountNumber, @amount, @transactionTimeUTC)";
                populateTransactions.Parameters.AddWithValue("transactionType", "D");
                populateTransactions.Parameters.AddWithValue("accountNumber", account.AccountNumber);
                populateTransactions.Parameters.AddWithValue("destinationAccountNumber", account.AccountNumber);
                populateTransactions.Parameters.AddWithValue("amount", account.Balance);
                populateTransactions.Parameters.AddWithValue("transactionTimeUTC", transactions.TransactionTimeUTC);
                
                populateTransactions.ExecuteNonQuery();
            }
            catch (SqlException se)
            {
                Console.WriteLine("Exception: {0}", se.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        // Deserializes JSON and inserts values to Login table 
        public void PopulateLogin()
        {
            using var client = new HttpClient();
            var jsonLogins = client.GetStringAsync("https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/logins/").Result;
            var logins = JsonConvert.DeserializeObject<List<Logins>>(jsonLogins);

            SqlConnection conn = new SqlConnection(ConnectionString);

            // Insert values to Login database 
            foreach (var login in logins)
            {
                InsertLogin(login);
            }
        }
        public void InsertLogin(Logins login)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);

            try
            {
                conn.Open();

                var populateLogin = conn.CreateCommand();
                var populateCustomer = conn.CreateCommand();

                populateLogin.CommandText = "insert into login (LoginID, CustomerID, PasswordHash) values (@loginID, @customerID, @passwordHash)";
                populateLogin.Parameters.AddWithValue("loginID", login.LoginID);
                populateLogin.Parameters.AddWithValue("customerID", login.CustomerID);
                populateLogin.Parameters.AddWithValue("passwordHash", login.PasswordHash);

                populateLogin.ExecuteNonQuery();

            }
            catch (SqlException se)
            {
                Console.WriteLine("Exception: {0}", se.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        // Deserializes JSON and inserts values to Customer table 
        public void PopulateCustomer()
        {
            using var client = new HttpClient();

            var jsonCustomers = client.GetStringAsync("https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/customers/").Result;

            var customers = JsonConvert.DeserializeObject<List<Customer>>(jsonCustomers, new IsoDateTimeConverter {DateTimeFormat = "dd/MM/yyyy hh:mm:ss tt" });

            foreach (var customer in customers)
            {
                InsertCustomers(customer);
            }
        }
        public void InsertCustomers(Customer customer)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);

            try
            {
                conn.Open();

                var populateLogin = conn.CreateCommand();
                var populateCustomer = conn.CreateCommand();

                populateCustomer.CommandText = "insert into customer (CustomerID, Name, Address, City, PostCode) values (@customerID, @name, @address, @city, @postCode)";
                populateCustomer.Parameters.AddWithValue("customerID", customer.CustomerID);
                populateCustomer.Parameters.AddWithValue("name", customer.Name);


                // Some parameters requires check for NULL values - Shekhar had Address, City and PostCode set to nothing in JSON but Database requires it set to "NULL" 
                if (customer.Address == null)
                {
                    populateCustomer.Parameters.AddWithValue("address", "NULL");
                }
                else
                {
                    populateCustomer.Parameters.AddWithValue("address", customer.Address);
                }

                if (customer.City == null)
                {
                    populateCustomer.Parameters.AddWithValue("city", "NULL");
                }
                else
                {
                    populateCustomer.Parameters.AddWithValue("city", customer.City);
                }

                if (customer.City == null)
                {
                    populateCustomer.Parameters.AddWithValue("postCode", "NULL");
                }
                else
                {
                    populateCustomer.Parameters.AddWithValue("postCode", customer.PostCode);
                }
                populateCustomer.ExecuteNonQuery();
            }
            catch (SqlException se)
            {
                Console.WriteLine("Exception: {0}", se.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
    }
}


using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Data.SqlClient;
using Newtonsoft.Json.Converters;
using System.Data;


namespace WDTAssignment
{
    class JSON
    {
        public List<Logins> Logins { get; }
        public List<Customer> Customer { get; }
        public List<Accounts> Accounts { get; }
        public List<Transactions> Transaction { get; }

        public static void PopulateDatabase()
        {


            ClearDatabase();

            PopulateCustomer();
            PopulateLogin();
            PopulateAccount();
            PopulateTransaction();

        }

        // Clear Database before inserting values deserialized from JSON file 
        public static void ClearDatabase()
        {
            SqlConnection conn = new SqlConnection("Server = wdt2020.australiasoutheast.cloudapp.azure.com; Database = s3711914; Uid = s3711914; Password = abc123");

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
        public static void PopulateAccount()
        {
            using var client = new HttpClient();
            var jsonAccounts = client.GetStringAsync("https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/customers/").Result;
            var customers = JsonConvert.DeserializeObject<List<Customer>>(jsonAccounts, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy hh:mm:ss tt" });

            SqlConnection conn = new SqlConnection("Server = wdt2020.australiasoutheast.cloudapp.azure.com; Database = s3711914; Uid = s3711914; Password = abc123");

            foreach (var customer in customers)
            {

                foreach (var account in customer.Accounts)
                {
                    InsertAccounts(account);
                }

            }

        }
        public static void InsertAccounts(Accounts account)
        {
            SqlConnection conn = new SqlConnection("Server = wdt2020.australiasoutheast.cloudapp.azure.com; Database = s3711914; Uid = s3711914; Password = abc123");

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
        public static void PopulateTransaction()
        {
            //Console.WriteLine("TEST: This method is called.");

            using var client = new HttpClient();
            var json = client.GetStringAsync("https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/customers/").Result;


            var customers = JsonConvert.DeserializeObject<List<Customer>>(json, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy hh:mm:ss tt" });

            SqlConnection conn = new SqlConnection("Server = wdt2020.australiasoutheast.cloudapp.azure.com; Database = s3711914; Uid = s3711914; Password = abc123");

            foreach (var customer in customers)
            {
                foreach (var account in customer.Accounts)
                {
                    foreach (var transaction in account.Transactions)
                    {
                        // TEST 
                        Console.WriteLine(transaction.TransactionTimeUTC);

                        InsertTransactions(transaction, account);
                        
                    }
                }

            }

        }
        public static void InsertTransactions(Transactions transactions, Accounts account)
        {
            SqlConnection conn = new SqlConnection("Server = wdt2020.australiasoutheast.cloudapp.azure.com; Database = s3711914; Uid = s3711914; Password = abc123");

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
        public static void PopulateLogin()
        {
            using var client = new HttpClient();
            var jsonLogins = client.GetStringAsync("https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/logins/").Result;
            var logins = JsonConvert.DeserializeObject<List<Logins>>(jsonLogins);

            SqlConnection conn = new SqlConnection("Server = wdt2020.australiasoutheast.cloudapp.azure.com; Database = s3711914; Uid = s3711914; Password = abc123");

            // Insert values to Login database 
            foreach (var login in logins)
            {
                InsertLogin(login);
            }
        }
        public static void InsertLogin(Logins login)
        {
            SqlConnection conn = new SqlConnection("Server = wdt2020.australiasoutheast.cloudapp.azure.com; Database = s3711914; Uid = s3711914; Password = abc123");

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
        public static void PopulateCustomer()
        {
            using var client = new HttpClient();

            var jsonCustomers = client.GetStringAsync("https://coreteaching01.csit.rmit.edu.au/~e87149/wdt/services/customers/").Result;


            var customers = JsonConvert.DeserializeObject<List<Customer>>(jsonCustomers, new IsoDateTimeConverter {DateTimeFormat = "dd/MM/yyyy hh:mm:ss tt" });

            foreach (var customer in customers)
            {
                InsertCustomers(customer);
            }
        }
        public static void InsertCustomers(Customer customer)
        {
            SqlConnection conn = new SqlConnection("Server = wdt2020.australiasoutheast.cloudapp.azure.com; Database = s3711914; Uid = s3711914; Password = abc123");

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


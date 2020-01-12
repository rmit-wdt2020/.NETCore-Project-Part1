using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient; 

namespace WDTAssignment
{
    class Database
    {
        public static void ImportDatabase()
        {
            DownloadLoginArray(); 
            DownloadCustomerArray();
        }

        public static void ExportDatabase()
        {
            UploadCustomer();
            UploadLoginArray();
            UploadAccounts();
            UploadTransactions(); 
        }


        public static void DownloadLoginArray()
        {
            SqlConnection conn = new SqlConnection("Server = wdt2020.australiasoutheast.cloudapp.azure.com; Database = s3711914; Uid = s3711914; Password = abc123");

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
                            BankingSys.Logins.Add(login);

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
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }

        }

        public static void DownloadCustomerArray()
        {
            SqlConnection conn = new SqlConnection("Server = wdt2020.australiasoutheast.cloudapp.azure.com; Database = s3711914; Uid = s3711914; Password = abc123");
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
                        int num = 0; 

                        while (ReadCustomer.Read())
                        {
                             
                            Customer customer = new Customer(0, "", int.Parse(ReadCustomer["CustomerID"].ToString()), ReadCustomer["Name"].ToString(),
                                ReadCustomer["Address"].ToString(), ReadCustomer["City"].ToString(), ReadCustomer["PostCode"].ToString());

                            BankingSys.Customers.Add(customer);
                        }

                        
                    }

                }

                foreach (var customer in BankingSys.Customers)
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

                foreach (var customer in BankingSys.Customers)
                {
                   using(AccountCmd = new SqlCommand("select accountnumber, accounttype, balance from account where customerID = " + customer.CustomerID, conn))
                    {
                        using (ReadAccount = AccountCmd.ExecuteReader())
                        {
                            while(ReadAccount.Read())
                            {
                                //Console.WriteLine("\n" + ReadAccount["AccountNumber"]);
                                Accounts account; 
                                if(ReadAccount["AccountType"].ToString().Contains('S'))
                                {
                                     account = new SavingsAccount(int.Parse(ReadAccount["AccountNumber"].ToString()), customer.CustomerID, 
                                        double.Parse(ReadAccount["Balance"].ToString()), 0, 100, 1);
                                } else
                                {
                                     account = new CheckingAccount(int.Parse(ReadAccount["AccountNumber"].ToString()), customer.CustomerID,
                                        double.Parse(ReadAccount["Balance"].ToString()), 200, 500, 1);
                                }


                                if(customer.CustomerID == account.CustomerID)
                                {
                                    customer.Accounts.Add(account); 
                                }
                            }
                        }
                    }

                }


                foreach(var customer in BankingSys.Customers)
                {
                    foreach(var account in customer.Accounts)
                    {
                        using (TransactionCmd = new SqlCommand("select [transactionid], [transactiontype], [accountnumber], [destinationaccountnumber]," +
                            " [amount], [comment] [transactiontimeutc] from [transaction] where [accountnumber] = " + account.AccountNumber, conn))
                        {
                            using(ReadTransaction = TransactionCmd.ExecuteReader())
                            {
                                while(ReadTransaction.Read())
                                {
                                    Transactions transaction = new Transactions(int.Parse(ReadTransaction["TransactionID"].ToString()), Convert.ToChar(ReadTransaction["TransactionType"].ToString()),
                                       int.Parse(ReadTransaction["AccountNumber"].ToString()), int.Parse(ReadTransaction["DestinationAccountNumber"].ToString()), 
                                       double.Parse(ReadTransaction["Amount"].ToString()), /*ReadTransaction["Comment"].ToString()*/null) /*ReadTransaction["TransactionTimeUTC"].ToString()*/;

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
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }


        }

        public static void UploadLoginArray()
        {
            SqlConnection conn = new SqlConnection("Server = wdt2020.australiasoutheast.cloudapp.azure.com; Database = s3711914; Uid = s3711914; Password = abc123");

            try
            {
                conn.Open(); 

                foreach(var login in BankingSys.Logins)
                {
                    var PopulateLogin = conn.CreateCommand();

                    PopulateLogin.CommandText = "insert into login (LoginID, CustomerID, PasswordHash) values (@loginID, @customerID, @passwordHash)";
                    PopulateLogin.Parameters.AddWithValue("loginID", login.LoginID);
                    PopulateLogin.Parameters.AddWithValue("customerID", login.CustomerID);
                    PopulateLogin.Parameters.AddWithValue("passwordHash", login.PasswordHash);

                    PopulateLogin.ExecuteNonQuery();
                }
            }
            catch (Exception se )
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


        public static void UploadCustomer()
        {
            SqlConnection conn = new SqlConnection("Server = wdt2020.australiasoutheast.cloudapp.azure.com; Database = s3711914; Uid = s3711914; Password = abc123");

            try
            {
                conn.Open();

                foreach (var customer in BankingSys.Customers)
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
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        public static void UploadAccounts()
        {
            SqlConnection conn = new SqlConnection("Server = wdt2020.australiasoutheast.cloudapp.azure.com; Database = s3711914; Uid = s3711914; Password = abc123");

            foreach (var customer in BankingSys.Customers)
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
                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }

            }

        }


        public static void UploadTransactions()
        {
            SqlConnection conn = new SqlConnection("Server = wdt2020.australiasoutheast.cloudapp.azure.com; Database = s3711914; Uid = s3711914; Password = abc123");

            foreach (var customer in BankingSys.Customers)
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
                            if (conn.State == System.Data.ConnectionState.Open)
                            {
                                conn.Close();
                            }
                        }

                    }
                }

            }
        }

    }




}

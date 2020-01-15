using System;
using System.Collections.Generic;
using System.Text;

namespace WDTAssignment
{
    class Customer
    {
        public int LoginID { get; set; }
        public string Password { get; set; }
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public List<Account> Accounts { get; set; }  = new List<Account>();

        public Customer(int login, string pw, int id, string name, string address, string city, string postcode)
        {
            LoginID = login;
            Password = pw;
            CustomerID = id;
            Name = name;
            Address = address;
            City = city;
            PostCode = postcode;
            Accounts.Capacity = 2;
        }
    }
}

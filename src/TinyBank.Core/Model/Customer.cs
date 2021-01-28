using System;
using System.Collections.Generic;

namespace TinyBank.Core.Model
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustBankID { get; set; }
        public string Name { get; set; }
        public string SureName { get; set; }
        public string VatNumber { get; set; }
        public List<Accounts> Accounts { get; set; }
        public bool Active { get; set; }
        public string Address { get; set; }
        public CustomerType CustType { get; set; }
        public DateTime Created { get; private set; }
        public Customer()
        {
            Created = DateTime.Now;
            Accounts = new List<Accounts>();
        }
    }
}

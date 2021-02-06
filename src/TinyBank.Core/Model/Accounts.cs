using System;
using System.Collections.Generic;

namespace TinyBank.Core.Model
{
    /// <summary>
    ///     Accounts Data Model
    /// </summary>
    public class Accounts
    {
        public int AccountsId { get; set; }
        public string AccountNumber { get; set; }
        public string AccountDescription { get; set; }
        public string Currency { get; set; }
        public decimal Balance { get; set; } = 0;
        public DateTime Created { get; private set; }
        public bool Active { get; set; } = true;
        public List<Transaction> Transactions { get; set; }

        public Accounts()
        {
            Created = DateTime.Now;
            Transactions = new List<Transaction>();
        }
    }
}

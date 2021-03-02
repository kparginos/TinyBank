using System;
using System.Collections.Generic;
using TinyBank.Model.Types;

namespace TinyBank.Model
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
        public AccountStateTypes State { get; set; } = AccountStateTypes.Inactive;
        public List<Transaction> Transactions { get; set; }

        public Accounts()
        {
            Created = DateTime.Now;
            Transactions = new List<Transaction>();
        }
    }
}

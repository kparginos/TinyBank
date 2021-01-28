using System;
using System.Collections.Generic;

namespace TinyBank.Core.Model
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string TransDescr { get; set; }
        public DateTime Created { get; private set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }

        public Transaction()
        {
            Created = DateTime.Now;
        }
    }
}

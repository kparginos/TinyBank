using System;

using TinyBank.Core.Model.Types;

namespace TinyBank.Core.Model
{
    /// <summary>
    ///     Transaction Data Model
    /// </summary>
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string TransDescr { get; set; }
        public DateTime Created { get; private set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public TransactionStateType State { get; set; } = TransactionStateType.Pending;

        public Transaction()
        {
            Created = DateTime.Now;
        }
    }
}

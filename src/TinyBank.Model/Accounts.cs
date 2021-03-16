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
        public AccountStateTypes State { get; set; } = AccountStateTypes.Inactive;
        public AuditInfo AuditInfo { get; set; }

        #region Navigation properties
        public List<Transaction> Transactions { get; set; }
        public List<Card> Cards { get; set; }
        public int CustomerId { get; set; }
        #endregion

        public Accounts()
        {
            Transactions = new List<Transaction>();
            AuditInfo = new AuditInfo();
            Cards = new List<Card>();
        }
    }
}

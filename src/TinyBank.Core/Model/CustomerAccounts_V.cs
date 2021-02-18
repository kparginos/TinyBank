using System;

using TinyBank.Core.Model.Types;

namespace TinyBank.Core.Model
{
    public class CustomerAccounts_V
    {
        public int? CustomerId { get; set; }
        public string Name { get; set; }
        public string SureName { get; set; }
        public string Address { get; set; }
        public int? CustType { get; set; }
        public string CustBankID { get; set; }
        public string VatNumber { get; set; }
        public DateTime? CustCreateDT { get; set; }
        public bool? Active { get; set; }
        public int? AccountsId { get; set; }
        public string AccountNumber { get; set; }
        public string AccountDescription { get; set; }
        public AccountStateTypes State { get; set; }
        public decimal? Balance { get; set; }
        public string Currency { get; set; }
        public DateTime? AccountCreateDate { get; set; }
    }
}

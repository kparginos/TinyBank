using System.Collections.Generic;

using TinyBank.Model.Types;

namespace TinyBank.Model
{
    /// <summary>
    ///     Customer Data Model
    /// </summary>
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustBankID { get; set; }
        public string Name { get; set; }
        public string SureName { get; set; }
        public string VatNumber { get; set; }
        public bool Active { get; set; } = true;
        public string Address { get; set; }
        public CustomerType CustType { get; set; }
        public string Phone { get; set; }
        public string CountryCode { get; set; }

        #region Navigation Properties
        public List<Accounts> Accounts { get; set; }
        #endregion

        public AuditInfo AuditInfo { get; set; }

        public Customer()
        {
            Accounts = new List<Accounts>();
            AuditInfo = new AuditInfo();
        }
    }
}

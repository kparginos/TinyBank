using TinyBank.Model.Types;

namespace TinyBank.Core.Services.Options
{
    public class RegisterCustomerOptions
    {
        public string Name { get; set; }
        public string SureName { get; set; }
        public string VATNumber { get; set; }
        public string CustomerBankID { get; set; }
        public CustomerType CustType { get; set; }
        public string Address { get; set; }
        public string CountryCode { get; set; }
    }
}

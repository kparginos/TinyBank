using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyBank.Core.Services.Options
{
    public class SearchCustomerOptions
    {
        public string VAtNumber { get; set; }
        public int? CustomerId { get; set; }
        public string CustBankId { get; set; }
        public int? MaxResults{ get; set; }
        public int? Skip { get; set; }
        public List<string> CountryCodes { get; set; }

        public SearchCustomerOptions()
        {
            CountryCodes = new List<string>();
        }
    }
}

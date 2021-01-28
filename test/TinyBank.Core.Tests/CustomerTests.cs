using System.Linq;

using TinyBank.Core.Data;
using TinyBank.Core.Model;
using TinyBank.Core.Model.Types;

using Xunit;

namespace TinyBank.Core.Tests
{
    public class CustomerTests
    {
        [Fact]
        public void Add_New_Customer()
        {
            using var dbcontext = new TinyBankDBContext();

            var customer = new Customer()
            {
                Active = true,
                Address = "Test Address",
                CustBankID = "032846778",
                CustType = CustomerType.Personal,
                Name = "Kostas",
                SureName = "Parginos",
                VatNumber = "123456789"
            };
            dbcontext.Add(customer);
            dbcontext.SaveChanges();
        }
    }
}

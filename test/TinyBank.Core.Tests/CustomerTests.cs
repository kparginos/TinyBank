using System.Linq;

using TinyBank.Core.Data;
using TinyBank.Core.Model;

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

        [Fact]
        public void Add_New_Account_To_Customer()
        {
            using var dbcontext = new TinyBankDBContext();

            var savedCustomer = dbcontext.Set<Customer>()
                .Where(c => c.CustBankID == "032846778")
                .SingleOrDefault();

            Assert.NotNull(savedCustomer);

            savedCustomer.Accounts.Add(new Accounts()
            {
                Active = true,
                AccountDescription = "My Personal Account",
                AccountNumber = "1558642123",
                Currency = "EUR",
                Balance = 1800.0m
            });

            dbcontext.Update(savedCustomer);
            dbcontext.SaveChanges();
        }
    }
}

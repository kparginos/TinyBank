using System;
using System.Linq;

using TinyBank.Core.Data;
using TinyBank.Core.Model;

using Xunit;

namespace TinyBank.Core.Tests
{
    public class AccountsTests
    {
        [Fact]
        public void Add_New_Account()
        {
            using var dbcontext = new TinyBankDBContext();

            var account = new Accounts()
            {
                Active = true,
                AccountDescription = "My Personal Account",
                AccountNumber = "1558642182",
                Currency = "EUR",
                Balance = 1500.0m
            };

            dbcontext.Add(account);
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

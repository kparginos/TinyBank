using System;

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
    }
}

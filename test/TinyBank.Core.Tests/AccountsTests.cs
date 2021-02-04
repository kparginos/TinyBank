using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TinyBank.Core.Config.Extentions;

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
            var config = new ConfigurationBuilder()
               .SetBasePath($"{AppDomain.CurrentDomain.BaseDirectory}")
               .AddJsonFile("appsettings.json", false)
               .Build();

            var connString = config.ReadAppConfiguration();

            var options = new DbContextOptionsBuilder<TinyBankDBContext>();
            options.UseSqlServer(connString.ConnString,
                options =>
                {
                    options.MigrationsAssembly("TinyBank");
                });

            using var dbContext = new TinyBankDBContext(options.Options);

            var account = new Accounts()
            {
                Active = true,
                AccountDescription = "My Personal Account",
                AccountNumber = "1558642182",
                Currency = "EUR",
                Balance = 1500.0m
            };

            dbContext.Add(account);
            dbContext.SaveChanges();
        }

        [Fact]
        public void Add_New_Account_To_Customer()
        {
            var config = new ConfigurationBuilder()
               .SetBasePath($"{AppDomain.CurrentDomain.BaseDirectory}")
               .AddJsonFile("appsettings.json", false)
               .Build();

            var connString = config.ReadAppConfiguration();

            var options = new DbContextOptionsBuilder<TinyBankDBContext>();
            options.UseSqlServer(connString.ConnString,
                options =>
                {
                    options.MigrationsAssembly("TinyBank");
                });

            using var dbContext = new TinyBankDBContext(options.Options);

            var savedCustomer = dbContext.Set<Customer>()
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

            dbContext.Update(savedCustomer);
            dbContext.SaveChanges();
        }
    }
}

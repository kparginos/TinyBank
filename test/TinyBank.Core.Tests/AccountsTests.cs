using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TinyBank.Core.Config.Extentions;

using TinyBank.Core.Data;
using TinyBank.Core.Model;
using TinyBank.Core.Services;
using TinyBank.Core.Services.Interfaces;
using TinyBank.Core.Services.Options;
using Xunit;

namespace TinyBank.Core.Tests
{
    public class AccountsTests : IClassFixture<TinyBankFixture>
    {
        private IAccountsService _account;
        private ICustomerService _customer;

        public AccountsTests(TinyBankFixture fixture)
        {
            _account = fixture.Scope.ServiceProvider
                .GetRequiredService<IAccountsService>();
            _customer = fixture.Scope.ServiceProvider
                .GetRequiredService<ICustomerService>();
        }

        [Fact]
        public void Add_New_Account()
        {
            using var dbContext = new TinyBankDBContext(GetDBOptions().Options);

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

        //[Fact]
        //public void Add_New_Account_With_AccountsService()
        //{
        //    using var dbContext = new TinyBankDBContext(GetDBOptions().Options);

        //    var accountsService = new AccountsService(dbContext);
        //    var account = accountsService.Register(1005, new RegisterAccountOptions()
        //    {
        //        AccountDescr = "Salary Acount",
        //        AccountNumber = "5392-556-2451",
        //        Currency = "EUR"
        //    });

        //    Assert.NotNull(account);
        //}

        [Fact]
        public void Add_New_Account_With_DI()
        {
            var options = new RegisterAccountOptions()
            {
                AccountDescr = "Personal Acount",
                AccountNumber = "8892-511-1031",
                Currency = "USD"
            };
            var account = _account.Register(1006, options);

            Assert.NotNull(account);
        }

        private DbContextOptionsBuilder<TinyBankDBContext> GetDBOptions()
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

            return options;
        }
    }
}

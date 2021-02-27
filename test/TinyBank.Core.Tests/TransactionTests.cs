using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TinyBank.Core.Config.Extentions;
using TinyBank.Data;
using TinyBank.Model;
using TinyBank.Model.Types;
using TinyBank.Core.Services.Interfaces;
using TinyBank.Core.Services.Options;
using Xunit;

namespace TinyBank.Core.Tests
{
    public class TransactionTests : IClassFixture<TinyBankFixture>
    {
        private ITransactionService _transaction;
        private IAccountsService _account;

        public TransactionTests(TinyBankFixture fixture)
        {
            _transaction = fixture.Scope.ServiceProvider
                .GetRequiredService<ITransactionService>();
            _account = fixture.Scope.ServiceProvider
                .GetRequiredService<IAccountsService>();
        }
        [Fact]
        public void Add_New_Transaction_To_Account()
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

            var savedAccount = dbContext.Set<Accounts>()
                .Where(a => a.AccountNumber == "1558642182")
                .SingleOrDefault();

            Assert.NotNull(savedAccount);

            savedAccount.Transactions.Add(new Transaction()
            {
                Amount = 150.0m,
                TransDescr = "Tablet purchase",
                Type = TransactionType.Credit
            });

            dbContext.Update(savedAccount);
            dbContext.SaveChanges();
        }

        [Fact]
        public void Add_New_Transaction_With_DI()
        {
            var options = new RegisterTransactionOptions()
            {
                Amount = 1500.0m,
                TransDescr = "Salary Payment",
                Type = TransactionType.Debit
            };

            var transaction = _transaction.Register(5, options);

            Assert.NotNull(transaction);
        }
    }
}

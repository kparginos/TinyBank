using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using TinyBank.Core.Data;
using TinyBank.Core.Model;
using TinyBank.Core.Model.Types;
using TinyBank.Core.Config.Extentions;

using Xunit;

namespace TinyBank.Core.Tests
{
    public class CustomerTests
    {
        [Fact]
        public void Add_New_Customer()
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
            dbContext.Add(customer);
            dbContext.SaveChanges();
        }

        [Fact]
        public void Get_Customer()
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

            var customers = dbContext.Set<Customer>()
                .Where(c => c.VatNumber == "123456789")
                .ToList();
        }
    }
}

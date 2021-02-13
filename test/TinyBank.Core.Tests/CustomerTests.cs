using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using TinyBank.Core.Data;
using TinyBank.Core.Model;
using TinyBank.Core.Model.Types;
using TinyBank.Core.Config.Extentions;

using Xunit;
using TinyBank.Core.Services;
using TinyBank.Core.Services.Options;
using TinyBank.Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using TinyBank.Core.Consts;

namespace TinyBank.Core.Tests
{
    public class CustomerTests : IClassFixture<TinyBankFixture>
    {
        private ICustomerService _customer;

        public CustomerTests(TinyBankFixture fixture)
        {
            _customer = fixture.Scope.ServiceProvider
                .GetRequiredService<ICustomerService>();
        }
        [Fact]
        public void Add_New_Customer()
        {
            using var dbContext = new TinyBankDBContext(GetDBOptions().Options);

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
        public void Add_New_Customer_With_Service()
        {
            using var dbContext = new TinyBankDBContext(GetDBOptions().Options);

            var customerService = new CustomerService(dbContext);
            var customer = customerService.Register(new RegisterCustomerOptions()
            {
                CustomerBankID="CUSTBNK0001",
                Name = "Ioannis",
                SureName = "Ioannopoulos",
                VATNumber = "033456789",
                CustType = CustomerType.Personal,
                Address = "Cust Address 1"
            });

            Assert.NotNull(customer);
        }

        [Fact]
        public void Add_New_Customer_With_DI()
        {
            var options = new RegisterCustomerOptions()
            {
                CustomerBankID = "CUSTBNK0209",
                Name = "Iliana",
                SureName = "Panagiotopoulou",
                VATNumber = "076451289",
                CustType = CustomerType.Personal,
                Address = "Aigyptou 84"
            };

            var customer = _customer.Register(options);

            Assert.NotNull(customer);
        }

        [Fact]
        public async void Add_New_Customer_Success_with_Async()
        {
            var options = new RegisterCustomerOptions()
            {
                CustomerBankID = "CUSTBNK0219",
                Name = "Melitini",
                SureName = "Parginou",
                VATNumber = "186493164",
                CustType = CustomerType.Personal,
                Address = "Aigyptou 84"
            };

            var result = await _customer.RegisterAsync(options);
            Assert.Equal(ResultCodes.Success, result.Code);
        }

        [Fact]
        public async void Update_Customer_Success_with_Async()
        {
            var options = new RegisterCustomerOptions()
            {
                CustomerBankID = "CUSTBNK0219",
                Name = "Melitini",
                SureName = "Parginou",
                VATNumber = "186493164",
                CustType = CustomerType.Personal,
                Address = "Aigyptou 84 - Glyfada"
            };

            var result = await _customer.UpdateCustomerAsync(2007, options);
            Assert.Equal(ResultCodes.Success, result.Code);
        }

        [Fact]
        public async void Delete_Customer_Success_with_Async()
        {
            var result = await _customer.DeleteCustomerAsync(2007);
            Assert.Equal(ResultCodes.Success, result.Code);
        }

        [Fact]
        public void Get_Customer()
        {
            using var dbContext = new TinyBankDBContext(GetDBOptions().Options);

            var customers = dbContext.Set<Customer>()
                .Where(c => c.VatNumber == "123456789")
                .ToList();
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

using System;
using System.Linq;
using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using TinyBank.Data;
using TinyBank.Model;
using TinyBank.Model.Types;
using TinyBank.Config.Extentions;
using TinyBank.Core.Services;
using TinyBank.Core.Services.Options;
using TinyBank.Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using TinyBank.Core.Consts;

using Xunit;
using System.Threading.Tasks;

namespace TinyBank.Core.Tests
{
    public class CustomerTests : IClassFixture<TinyBankFixture>
    {
        private ICustomerService _customer;
        private IFileParser _fileParser;

        public CustomerTests(TinyBankFixture fixture)
        {
            _customer = fixture.Scope.ServiceProvider
                .GetRequiredService<ICustomerService>();

            _fileParser = fixture.Scope.ServiceProvider
                .GetRequiredService<IFileParser>();
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

        [Theory]
        [InlineData(Country.GreekCountryCode)]
        [InlineData(Country.CyprusCountryCode)]
        [InlineData(Country.ItalyCountryCode)]
        public Customer Add_New_Customer_With_DI(string countryCode)
        {
            var vatNumber = GenerateVat(countryCode);

            Assert.NotNull(vatNumber);

            var options = new RegisterCustomerOptions()
            {
                CustomerBankID = "CUSTBNK0209",
                Name = "Iliana",
                SureName = "Panagiotopoulou",
                VATNumber = vatNumber,
                CustType = CustomerType.Personal,
                Address = "Aigyptou 84",
                CountryCode = countryCode
            };

            var customer = _customer.Register(options);

            Assert.Equal(ResultCodes.Success, customer.Code);

            return customer.Data;
        }

        [Fact]
        public async Task Add_New_Customer_Success_with_Async()
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
        public async Task Update_Customer_Success_with_Async()
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
        public async Task Delete_Customer_Success_with_Async()
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

        [Fact]
        public void Load_CustomerFile_Success()
        {
            var result = _fileParser.LoadCustFile(@"files\Book1.xlsx");

            Assert.Equal(ResultCodes.Success, result.Code);
            Assert.NotNull(result.Data);
            Assert.Equal(3, result.Data.Count);
            Assert.Equal(15873.92m, result.Data[2].TotalGross);
        }

        [Fact]
        public  void ExportCustomerToFile_Success()
        {
            var result = _fileParser.ExportCustomersToFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"files\Customers.xlsx"));

            Assert.Equal(ResultCodes.Success, result.Code);
            Assert.True(result.Data);
        }

        [Fact]
        public void ExportCustomerAccountToFile_Success()
        {
            var result = _fileParser.ExportCustomerAccountsToFile(
                exportPath: Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"files\CustomerAccounts.xlsx"),
                customerID: 1006);

            Assert.Equal(ResultCodes.Success, result.Code);
            Assert.True(result.Data);
        }

        [Fact]
        public void IsValidVatNumber_Check()
        {
            var result = _customer.IsValidVatNumber("GR", "123485679");
            Assert.True(result.Data);

            result = _customer.IsValidVatNumber("IT", "1234856790");
            Assert.True(result.Data);

            result = _customer.IsValidVatNumber("CY", "12348567901");
            Assert.True(result.Data);

            result = _customer.IsValidVatNumber("GB", "123485679");
            Assert.False(result.Data);
        }

        [Fact]
        public void SearchCustomer_Success()
        {
            var c1 = Add_New_Customer_With_DI(Country.GreekCountryCode);
            Assert.NotNull(c1);

            var c2 = Add_New_Customer_With_DI(Country.CyprusCountryCode);
            Assert.NotNull(c2);

            var c3 = Add_New_Customer_With_DI(Country.ItalyCountryCode);
            Assert.NotNull(c3);

            var options = new SearchCustomerOptions()
            {
                CountryCodes = { Country.GreekCountryCode, Country.CyprusCountryCode }
            };

            var customersFound = _customer
                .Search(options)
                .SingleOrDefault();

            Assert.NotNull(customersFound);
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
        private string GenerateVat(string countryCode)
        {
            switch (countryCode)
            {
                case Country.GreekCountryCode:
                    return $"{DateTimeOffset.Now:ssffffff}";
                case Country.CyprusCountryCode:
                    return $"{DateTimeOffset.Now:mmssffffff}";
                case Country.ItalyCountryCode:
                    return $"{DateTimeOffset.Now:mssffffff}";
                default:
                    return null;
            }
        }
    }
}

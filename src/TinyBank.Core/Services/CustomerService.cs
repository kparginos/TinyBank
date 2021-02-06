using System.Linq;
using TinyBank.Core.Data;
using TinyBank.Core.Model;
using TinyBank.Core.Services.Interfaces;
using TinyBank.Core.Services.Options;

namespace TinyBank.Core.Services
{
    public class CustomerService : ICustomerService
    {
        long vatNumber = 0;
        private readonly TinyBankDBContext _dbContext;

        public CustomerService(TinyBankDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Customer Register(RegisterCustomerOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.Name))
                return null;

            if (string.IsNullOrWhiteSpace(options.Name))
                return null;

            if (options.VATNumber.Length != 9)
                return null;

            if (!long.TryParse(options.VATNumber, out vatNumber))
                return null;

            var customer = new Customer()
            {
                Name = options.Name,
                SureName = options.SureName,
                VatNumber = options.VATNumber,
                CustBankID = options.CustomerBankID,
                CustType = options.CustType,
                Address = options.Address
            };

            _dbContext.Add<Customer>(customer);
            _dbContext.SaveChanges();

            return customer;
        }

        public Customer GetCustomerbyID(int customerID)
        {
            return _dbContext.Set<Customer>()
                .Where(c => c.CustomerId == customerID)
                .SingleOrDefault();
        }
    }
}

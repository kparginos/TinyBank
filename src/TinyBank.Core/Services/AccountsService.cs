using System.Linq;

using TinyBank.Core.Data;
using TinyBank.Core.Model;
using TinyBank.Core.Services.Interfaces;
using TinyBank.Core.Services.Options;

namespace TinyBank.Core.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly TinyBankDBContext _dBContext;
        private ICustomerService _customer;

        public AccountsService(TinyBankDBContext dBContext, ICustomerService customer)
        {
            _dBContext = dBContext;
            _customer = customer;
        }

        public Accounts GetAccountbyID(int accountID)
        {
            return _dBContext.Set<Accounts>()
                .Where(a => a.AccountsId == accountID)
                .SingleOrDefault();
        }

        public Accounts Register(int customerID, RegisterAccountOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.AccountNumber))
                return null;
            
            if (string.IsNullOrWhiteSpace(options.AccountDescr))
                return null;
            
            if (string.IsNullOrWhiteSpace(options.Currency))
                return null;

            var customer = _customer.GetCustomerbyID(customerID);
            if (customer == null)
                return null;

            var account = new Accounts()
            {
                AccountDescription = options.AccountDescr,
                AccountNumber = options.AccountNumber,
                Currency = options.Currency,
            };

            customer.Accounts.Add(account);

            _dBContext.SaveChanges();

            return account;
        }
    }
}

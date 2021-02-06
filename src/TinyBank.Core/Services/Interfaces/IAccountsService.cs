using TinyBank.Core.Model;
using TinyBank.Core.Services.Options;

namespace TinyBank.Core.Services.Interfaces
{
    public interface IAccountsService
    {
        public Accounts Register(int customerID, RegisterAccountOptions options);
        public Accounts GetAccountbyID(int accountID);
    }
}

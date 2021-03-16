using System.Threading.Tasks;
using TinyBank.Model;
using TinyBank.Model.Types;
using TinyBank.Core.Services.Options;
using TinyBank.Core.Services.Results;

namespace TinyBank.Core.Services.Interfaces
{
    public interface IAccountsService
    {
        public Result<Accounts> Register(int customerID, RegisterAccountOptions options);
        public Task<Result<Accounts>> RegisterAsync(int customerID, RegisterAccountOptions options);
        public Result<Accounts> SetState(int accountID, AccountStateTypes state);
        public Task<Result<Accounts>> SetStateAsync(int accountID, AccountStateTypes state);
        public Result<Accounts> GetAccountbyID(int accountID);
        public Task<Result<Accounts>> GetAccountbyIDAsync(int accountID);
        public Task<Result<Accounts>> GetAccountbyNumberAsync(string accountNumber);
        public Result<Accounts> GetAccountbyCustomerID(int customerID, int accountID);
        public Task<Result<Accounts>> GetAccountbyCustomerIDAsync(int customerID, int accountID);
    }
}

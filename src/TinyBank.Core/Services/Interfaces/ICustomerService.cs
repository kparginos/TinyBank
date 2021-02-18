using System.Threading.Tasks;

using TinyBank.Core.Model;
using TinyBank.Core.Services.Options;
using TinyBank.Core.Services.Results;

namespace TinyBank.Core.Services.Interfaces
{
    public interface ICustomerService
    {
        public Result<Customer> Register(RegisterCustomerOptions options);
        public Task<Result<Customer>> RegisterAsync(RegisterCustomerOptions options);
        public Result<bool> DeleteCustomer(int customerID);
        public Task<Result<bool>> DeleteCustomerAsync(int customerID);
        public Result<Customer> UpdateCustomer(int customerID, RegisterCustomerOptions options);
        public Task<Result<Customer>> UpdateCustomerAsync(int customerID, RegisterCustomerOptions options);
        public Result<Customer> SetState(int customerID, bool state);
        public Task<Result<Customer>> SetStateAsync(int customerID, bool state);
        public Result<Customer> GetCustomerbyID(int customerID);
        public Task<Result<Customer>> GetCustomerbyIDAsync(int customerID);
        public ResultList<CustomerAccounts_V> GetCustomerAccounts(int customerID);
        public Task<ResultList<CustomerAccounts_V>> GetCustomerAccountsAsync(int customerID);
    }
}

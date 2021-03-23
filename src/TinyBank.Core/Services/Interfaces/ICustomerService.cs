using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using TinyBank.Model;
using TinyBank.Core.Services.Options;
using TinyBank.Core.Services.Results;

namespace TinyBank.Core.Services.Interfaces
{
    public interface ICustomerService
    {
        /// <summary>
        ///     Adds a new customer
        /// </summary>
        /// <param name="options">The customer information of type of RegisterCustomerOptions to be added</param>
        /// <returns>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public Result<Customer> Register(RegisterCustomerOptions options);
        /// <summary>
        ///     Adds a new customer using with Async support
        /// </summary>
        /// <param name="options">The customer information of type of RegisterCustomerOptions to be added</param>
        /// <returns>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public Task<Result<Customer>> RegisterAsync(RegisterCustomerOptions options);
        /// <summary>
        ///     Deletes a given customer Id
        /// </summary>
        /// <param name="customerID">The customer Id to be deleted</param>
        /// <returns>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public Result<bool> DeleteCustomer(int customerID);
        /// <summary>
        ///     Deletes a given customer Id with Async support
        /// </summary>
        /// <param name="customerID">The customer Id to be deleted</param>
        /// <returns>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public Task<Result<bool>> DeleteCustomerAsync(int customerID);
        /// <summary>
        ///     Updates a given customer Id
        /// </summary>
        /// <param name="customerID">The customer Id to updated</param>
        /// <param name="options">The information to be updated as type of RegisterCustomerOptions </param>
        /// <returns>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public Result<Customer> UpdateCustomer(int customerID, RegisterCustomerOptions options);
        /// <summary>
        ///     Updates a given customer Id with Async support
        /// </summary>
        /// <param name="customerID">The customer Id to updated</param>
        /// <param name="options">The information to be updated as type of RegisterCustomerOptions </param>
        /// <returns>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public Task<Result<Customer>> UpdateCustomerAsync(int customerID, RegisterCustomerOptions options);
        /// <summary>
        ///     Changes the status of a given customer Id
        /// </summary>
        /// <param name="customerID">The customer Id for which the status will be changed</param>
        /// <param name="options">The status to be changed to</param>
        /// <returns>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public Result<Customer> SetState(int customerID, bool state);
        /// <summary>
        ///     Changes the status of a given customer Id with Async support
        /// </summary>
        /// <param name="customerID">The customer Id for which the status will be changed</param>
        /// <param name="options">The status to be changed to</param>
        /// <returns>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public Task<Result<Customer>> SetStateAsync(int customerID, bool state);
        /// <summary>
        ///     Gets a list of all customers
        /// </summary>
        /// <returns>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public Result<List<Customer>> GetAllCustomers();
        /// <summary>
        ///     Gets a list of all customers with Async support
        /// </summary>
        /// <returns>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public Task<Result<List<Customer>>> GetAllCustomersAsync();
        /// <summary>
        ///     Gets information of a given customer Id
        /// </summary>
        /// <param name="customerID">The customer Id to retreive information</param>
        /// <returns>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public Result<Customer> GetCustomerbyID(int customerID);
        /// <summary>
        ///     Gets information of a given customer Id with Async support
        /// </summary>
        /// <param name="customerID">The customer Id to retreive information</param>
        /// <returns>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public Task<Result<Customer>> GetCustomerbyIDAsync(int customerID);
        /// <summary>
        ///     Gets a list of accounts bind to a given customer Id
        /// </summary>
        /// <param name="customerID">The customer Id to for which will get the list of accounts</param>
        /// <returns>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public ResultList<CustomerAccounts_V> GetCustomerAccounts(int customerID);
        /// <summary>
        ///     Gets a list of accounts bind to a given customer Id with Async support
        /// </summary>
        /// <param name="customerID">The customer Id to for which will get the list of accounts</param>
        /// <returns>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public Task<ResultList<CustomerAccounts_V>> GetCustomerAccountsAsync(int customerID);
        /// <summary>
        ///     Checks whether a VAT number is valid for a given country code
        /// </summary>
        /// <param name="countryCode">The country code for which the VAT number is applied</param>
        /// <param name="vatNumber">The VAT number to check</param>
        /// <returns>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public Result<bool> IsValidVatNumber(string countryCode, string vatNumber);
        /// <summary>
        ///     Searches for a customer given a set of search options
        /// </summary>
        /// <param name="options">Search option</param>
        /// <returns>
        ///     A customer object of IQueryable type
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Occurs when no search options are passed
        /// </exception>
        public IQueryable<Customer> Search(SearchCustomerOptions options);
    }
}

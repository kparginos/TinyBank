using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using TinyBank.Core.Consts;
using TinyBank.Data;
using TinyBank.Model;
using TinyBank.Core.Services.Interfaces;
using TinyBank.Core.Services.Options;
using TinyBank.Core.Services.Results;

namespace TinyBank.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly TinyBankDBContext _dbContext;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="dbContext">The DBContext to be use by the rest of the methods when accessing the database</param>
        public CustomerService(TinyBankDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region CRUD operations
        /// <summary>
        ///     Adds a new customer
        /// </summary>
        /// <param name="options">RegisterCustomerOptions</param>
        /// <returns>
        ///     Result<Customer>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public Result<Customer> Register(RegisterCustomerOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.Name))
                return new Result<Customer>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"Customer name is empty!"
                };

            if (string.IsNullOrWhiteSpace(options.SureName))
                return new Result<Customer>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"Customer sure name is empty!"
                };

            var validVatNumber = IsValidVatNumber(options.CountryCode, options.VATNumber);

            if (!validVatNumber.IsSuccess())
            {
                return new Result<Customer>()
                {
                    Code = validVatNumber.Code,
                    Message = validVatNumber.Message
                };
            }

            if(_dbContext.Set<Customer>()
                .Any(c => c.VatNumber == options.VATNumber))
            {
                return new Result<Customer>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"Specific Vat Number {options.VATNumber} already exists!"
                };
            }

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

            return new Result<Customer>()
            {
                Code = ResultCodes.Success,
                Data = customer
            };
        }

        /// <summary>
        ///     Adds a new customer using with Async support
        /// </summary>
        /// <param name="options">RegisterCustomerOptions</param>
        /// <returns>
        ///     Task<Result<Customer>>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public async Task<Result<Customer>> RegisterAsync(RegisterCustomerOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.Name))
                return new Result<Customer>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = "Customer name is empty"
                };

            if (string.IsNullOrWhiteSpace(options.SureName))
                return new Result<Customer>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = "Customer sure name is empty"
                };

            var validVatNumber = IsValidVatNumber(options.CountryCode, options.VATNumber);
            if (!validVatNumber.IsSuccess())
            {
                return new Result<Customer>()
                {
                    Code = validVatNumber.Code,
                    Message = validVatNumber.Message
                };
            }

            if (await _dbContext.Set<Customer>()
                .AnyAsync(c => c.VatNumber == options.VATNumber))
            {
                return new Result<Customer>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"Specific Vat Number {options.VATNumber} already exists!"
                };
            }

            var customer = new Customer()
            {
                Name = options.Name,
                SureName = options.SureName,
                VatNumber = options.VATNumber,
                CustBankID = options.CustomerBankID,
                CustType = options.CustType,
                Address = options.Address
            };

            try
            {
                await _dbContext.AddAsync<Customer>(customer);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new Result<Customer>()
                {
                    Code = ResultCodes.InternalServerError,
                    Message = $"Cannot save customer info. Details: {ex.Message}"
                };
            }
            
            return new Result<Customer>()
            {
                Code = ResultCodes.Success,
                Data = customer
            };
        }

        /// <summary>
        ///     Deletes a given customer Id
        /// </summary>
        /// <param name="customerID">The customer Id to be deleted</param>
        /// <returns>
        ///     Result<bool>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public Result<bool> DeleteCustomer(int customerID)
        {
            var result = GetCustomerbyID(customerID);

            if (result.Code == ResultCodes.Success)
            {
                _dbContext.Remove(result.Data);
                _dbContext.SaveChanges();
                return new Result<bool>()
                {
                    Code = result.Code,
                    Message = $"Customer ID {customerID} deleted successfully",
                    Data = true
                };
            }
            else
            {
                return new Result<bool>()
                {
                    Code = result.Code,
                    Message = result.Message,
                    Data = false
                };
            }
        }

        /// <summary>
        ///     Deletes a given customer Id with Async support
        /// </summary>
        /// <param name="customerID">The customer Id to be deleted</param>
        /// <returns>
        ///     Task<Result<bool>>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public async Task<Result<bool>> DeleteCustomerAsync(int customerID)
        {
            var result = await GetCustomerbyIDAsync(customerID);

            if (result.Code == ResultCodes.Success)
            {
                _dbContext.Remove(result.Data);
                await _dbContext.SaveChangesAsync();
                return new Result<bool>()
                {
                    Code = result.Code,
                    Message = $"Customer ID {customerID} deleted successfully",
                    Data = true
                };
            }
            else
            {
                return new Result<bool>()
                {
                    Code = result.Code,
                    Message = result.Message,
                    Data = false
                };
            }
        }

        /// <summary>
        ///     Updates a given customer Id
        /// </summary>
        /// <param name="customerID">The customer Id to updated</param>
        /// <param name="options">The information to be updated as type of RegisterCustomerOptions </param>
        /// <returns>
        ///     Result<Customer>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public Result<Customer> UpdateCustomer(int customerID, RegisterCustomerOptions options)
        {
            if (options == null)
            {
                return new Result<Customer>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"Options must be specified"
                };
            }

            var result = GetCustomerbyID(customerID);

            if(result.IsSuccess())
            {
                var customer = result.Data;

                customer.Address = options.Address;
                customer.CustBankID = options.CustomerBankID;
                customer.CustType = options.CustType;
                customer.Name = options.Name;
                customer.SureName = options.SureName;
                customer.VatNumber = options.VATNumber;

                _dbContext.Update(customer);
                _dbContext.SaveChanges();

                return new Result<Customer>()
                {
                    Code = ResultCodes.Success,
                    Message = $"Customer ID {customerID} updated successfully",
                    Data = customer
                };
            }
            else
            {
                return new Result<Customer>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"Failed to update information for Customer ID {customerID}"
                };
            }
        }

        /// <summary>
        ///     Updates a given customer Id with Async support
        /// </summary>
        /// <param name="customerID">The customer Id to updated</param>
        /// <param name="options">The information to be updated as type of RegisterCustomerOptions </param>
        /// <returns>
        ///     Task<Result<Customer>>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public async Task<Result<Customer>> UpdateCustomerAsync(int customerID, RegisterCustomerOptions options)
        {
            if (options == null)
            {
                return new Result<Customer>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"Options must be specified"
                };
            }

            var result = await GetCustomerbyIDAsync(customerID);

            if (result.Code == ResultCodes.Success)
            {
                var customer = result.Data;

                customer.Address = options.Address;
                customer.CustBankID = options.CustomerBankID;
                customer.CustType = options.CustType;
                customer.Name = options.Name;
                customer.SureName = options.SureName;
                customer.VatNumber = options.VATNumber;

                _dbContext.Update(customer);
                await _dbContext.SaveChangesAsync();

                return new Result<Customer>()
                {
                    Code = ResultCodes.Success,
                    Message = $"Customer ID {customerID} updated successfully",
                    Data = customer
                };
            }
            else
            {
                return new Result<Customer>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"Failed to update information for Customer ID {customerID}"
                };
            }
        }

        /// <summary>
        ///     Changes the status of a given customer Id
        /// </summary>
        /// <param name="customerID">The customer Id to updated</param>
        /// <param name="options">The status to be changed to</param>
        /// <returns>
        ///     Result<Customer>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public Result<Customer> SetState(int customerID, bool state)
        {
            var result = GetCustomerbyID(customerID);

            if(result.Code == ResultCodes.Success)
            {
                var customer = result.Data;

                customer.Active = state;
                _dbContext.Update(customer);
                _dbContext.SaveChanges();

                return new Result<Customer>()
                {
                    Code = result.Code,
                    Message = $"Status for Customer ID {customerID} updated to {state}",
                    Data = customer
                };
            }
            else
            {
                return new Result<Customer>()
                {
                    Code = result.Code,
                    Message = result.Message
                };
            }
        }

        /// <summary>
        ///     Changes the status of a given customer Id with Async support
        /// </summary>
        /// <param name="customerID">The customer Id to updated</param>
        /// <param name="options">The status to be changed to</param>
        /// <returns>
        ///     Task<Result<Customer>>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public async Task<Result<Customer>> SetStateAsync(int customerID, bool state)
        {
            var result = await GetCustomerbyIDAsync(customerID);

            if (result.Code == ResultCodes.Success)
            {
                var customer = result.Data;

                customer.Active = state;
                _dbContext.Update(customer);
                await _dbContext.SaveChangesAsync();

                return new Result<Customer>()
                {
                    Code = result.Code,
                    Message = $"Status for Customer ID {customerID} updated to {state}",
                    Data = customer
                };
            }
            else
            {
                return new Result<Customer>()
                {
                    Code = result.Code,
                    Message = result.Message
                };
            }
        }
        #endregion

        #region Customer Info Operations
        /// <summary>
        ///     Gets information of a given customer Id
        /// </summary>
        /// <param name="customerID">The customer Id to updated</param>
        /// <returns>
        ///     Result<Customer>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public Result<Customer> GetCustomerbyID(int customerID)
        {
            var customer = _dbContext.Customer
                .Where(c => c.CustomerId == customerID)
                .Include(c => c.Accounts)
                .SingleOrDefault();

            if (customer != null)
            {
                return new Result<Customer>()
                {
                    Code = ResultCodes.Success,
                    Data = customer
                };
            }
            else
            {
                return new Result<Customer>()
                {
                    Code = ResultCodes.NotFound,
                    Message = $"Customer ID {customerID} not found"
                };
            }
        }

        /// <summary>
        ///     Gets information of a given customer Id with Async support
        /// </summary>
        /// <param name="customerID">The customer Id to updated</param>
        /// <returns>
        ///     Task<Result<Customer>>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public async Task<Result<Customer>> GetCustomerbyIDAsync(int customerID)
        {
            var customer = await _dbContext.Customer
                .Where(c => c.CustomerId == customerID)
                .Include(c => c.Accounts)
                .SingleOrDefaultAsync();

            if (customer != null)
            {
                return new Result<Customer>()
                {
                    Code = ResultCodes.Success,
                    Message = $"Customer information found",
                    Data = customer
                };
            }
            else
            {
                return new Result<Customer>()
                {
                    Code = ResultCodes.NotFound,
                    Message = $"Customer ID {customerID} not found !"
                };
            }
        }

        /// <summary>
        ///     Gets a list of accounts bind to a given customer Id
        /// </summary>
        /// <param name="customerID">The customer Id to updated</param>
        /// <returns>
        ///     ResultList<CustomerAccounts_V>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public ResultList<CustomerAccounts_V> GetCustomerAccounts(int customerID)
        {
            var customerInfoResult = _dbContext.CustomerAccountsView
                .Where(c => c.CustomerId == customerID)
                .ToList();

            if(customerInfoResult != null)
            {
                return new ResultList<CustomerAccounts_V>()
                {
                    Code = ResultCodes.Success,
                    Message = $"Found {customerInfoResult.Count} row(s)",
                    Data = customerInfoResult
                };
            }
            else
            {
                return new ResultList<CustomerAccounts_V>()
                {
                    Code = ResultCodes.NotFound,
                    Message = $"Customer ID {customerID} not found"
                };
            }
        }

        /// <summary>
        ///     Gets a list of accounts bind to a given customer Id with Async support
        /// </summary>
        /// <param name="customerID">The customer Id to updated</param>
        /// <returns>
        ///     Task<ResultList<CustomerAccounts_V>>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public async Task<ResultList<CustomerAccounts_V>> GetCustomerAccountsAsync(int customerID)
        {
            var customerInfoResult = await _dbContext.CustomerAccountsView
                .Where(c => c.CustomerId == customerID)
                .ToListAsync();

            if (customerInfoResult != null)
            {
                return new ResultList<CustomerAccounts_V>()
                {
                    Code = ResultCodes.Success,
                    Message = $"Found {customerInfoResult.Count} row(s)",
                    Data = customerInfoResult
                };
            }
            else
            {
                return new ResultList<CustomerAccounts_V>()
                {
                    Code = ResultCodes.NotFound,
                    Message = $"Customer ID {customerID} not found"
                };
            }
        }

        /// <summary>
        ///     Gets a list of all customers
        /// </summary>
        /// <returns>
        ///     Result<List<Customer>>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public Result<List<Customer>> GetAllCustomers()
        {
            var customers = _dbContext.Customer
                .ToList();

            if (customers != null)
            {
                return new Result<List<Customer>>()
                {
                    Code = ResultCodes.Success,
                    Data = customers
                };
            }
            else
            {
                return new Result<List<Customer>>()
                {
                    Code = ResultCodes.NotFound,
                    Message = $"Customers found {customers.Count}"
                };
            }
        }

        /// <summary>
        ///     Gets a list of all customers with Async support
        /// </summary>
        /// <returns>
        ///     Result<List<Customer>>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public async Task<Result<List<Customer>>> GetAllCustomersAsync()
        {
            var customers = await _dbContext.Customer
                .ToListAsync();

            if (customers != null)
            {
                return new Result<List<Customer>>()
                {
                    Code = ResultCodes.Success,
                    Data = customers
                };
            }
            else
            {
                return new Result<List<Customer>>()
                {
                    Code = ResultCodes.NotFound,
                    Message = $"Customers found {customers.Count}"
                };
            }
        }

        /// <summary>
        ///     Checks whether a VAT number is valid for a given country code
        /// </summary>
        /// <param name="countryCode">The country code for whichc the VAT number is applied</param>
        /// <param name="vatNumber">The VAT number to check</param>
        ///     Result<bool>
        ///     Result.Code should be Success(200)
        ///     Check Result.Code and Result.Message to get more details about possible errors
        /// </returns>
        public Result<bool> IsValidVatNumber(string countryCode, string vatNumber)
        {
            if (!string.IsNullOrWhiteSpace(countryCode))
            {
                return new Result<bool>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"Country Code cannot be Null or Empty",
                    Data = false
                };
            }
            if (!string.IsNullOrWhiteSpace(vatNumber))
            {
                return new Result<bool>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"VAT Number cannot be Null or Empty",
                    Data = false
                };
            }
            if (Country.VatValidNumbers.TryGetValue(countryCode, out var _))
            {
                return new Result<bool>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"Country Code is invalid",
                    Data = false
                };
            }
            if (vatNumber.Length == Country.VatValidNumbers.GetValueOrDefault(countryCode))
            {
                return new Result<bool>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"VAT Number {vatNumber} length is invalid for Country Code {countryCode}",
                    Data = false
                };
            }

            return new Result<bool>()
            {
                Code = ResultCodes.Success,
                Message = "VAT Number is valid",
                Data = true
            };
        }
        #endregion
    }
}

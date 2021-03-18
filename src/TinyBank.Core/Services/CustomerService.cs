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
        long vatNumber = 0;
        private readonly TinyBankDBContext _dbContext;

        public CustomerService(TinyBankDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region CRUD operations
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

            if (options.VATNumber.Length != 9)
                return new Result<Customer>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"VAT Number length is invalid!"
                };

            if (!long.TryParse(options.VATNumber, out vatNumber))
                return new Result<Customer>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"VAT number must be numeric!"
                };

            if (!Country.SupportedCountryCodes.Contains(options.CountryCode))
            {
                return new Result<Customer>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"Country code {options.CountryCode} is not supported"
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

            if (options.VATNumber.Length != 9)
                return new Result<Customer>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = "VAT Number length is invalid"
                };

            if (!long.TryParse(options.VATNumber, out vatNumber))
                return new Result<Customer>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = "VAT Number name is invalid"
                };

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

        public Result<Customer> UpdateCustomer(int customerID, RegisterCustomerOptions options)
        {
            var result = GetCustomerbyID(customerID);

            if(result.Code == ResultCodes.Success)
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

        public async Task<Result<Customer>> UpdateCustomerAsync(int customerID, RegisterCustomerOptions options)
        {
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
        #endregion
    }
}

using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TinyBank.Core.Consts;
using TinyBank.Data;
using TinyBank.Model;
using TinyBank.Model.Types;
using TinyBank.Core.Services.Interfaces;
using TinyBank.Core.Services.Options;
using TinyBank.Core.Services.Results;

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


        public Result<Accounts> Register(int customerID, RegisterAccountOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.AccountNumber))
                return new Result<Accounts>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = "Account Number is invalid!"
                };

            if (string.IsNullOrWhiteSpace(options.AccountDescr))
                return new Result<Accounts>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = "Account Description not set"
                };

            if (string.IsNullOrWhiteSpace(options.Currency))
                return new Result<Accounts>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = "Currency is invalid"
                };

            var result = _customer.GetCustomerbyID(customerID);
            if (result.Code != ResultCodes.Success)
                return new Result<Accounts>()
                {
                    Code = result.Code,
                    Message = result.Message
                };

            var account = new Accounts()
            {
                AccountDescription = options.AccountDescr,
                AccountNumber = options.AccountNumber,
                Currency = options.Currency,
            };

            result.Data.Accounts.Add(account);

            _dBContext.SaveChanges();

            return new Result<Accounts>()
            {
                Code = ResultCodes.Success,
                Data = account
            };
        }
        public async Task<Result<Accounts>> RegisterAsync(int customerID, RegisterAccountOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.AccountNumber))
                return null;

            if (string.IsNullOrWhiteSpace(options.AccountDescr))
                return null;

            if (string.IsNullOrWhiteSpace(options.Currency))
                return null;

            var result = await _customer.GetCustomerbyIDAsync(customerID);
            if (result.Code != ResultCodes.Success)
                return new Result<Accounts>()
                {
                    Code = result.Code,
                    Message = result.Message
                };
            //if (customer == null)
            //    return null;

            var account = new Accounts()
            {
                AccountDescription = options.AccountDescr,
                AccountNumber = options.AccountNumber,
                Currency = options.Currency,
            };

            result.Data.Accounts.Add(account);

            await _dBContext.SaveChangesAsync();

            return new Result<Accounts>()
            {
                Code = result.Code,
                Message = $"New Account ID {account.AccountsId} added for Customer ID {customerID}",
                Data = account
            };
        }
        public Result<Accounts> SetState(int accountID, AccountStateTypes state)
        {
            var result = GetAccountbyID(accountID);

            if(result.Code == ResultCodes.Success)
            {
                var account = result.Data;

                if(account.Balance != 0.0m && state == AccountStateTypes.Closed)
                {
                    // Account has balance and therefor cannot be closed
                    return new Result<Accounts>()
                    {
                        Code = ResultCodes.BadRequest,
                        Message = $"Account has balance and therefor cannot be closed",
                        Data = account
                    };
                }
                else
                {
                    account.State = state; 
                    _dBContext.Update(account);
                    _dBContext.SaveChanges();

                    return new Result<Accounts>()
                    {
                        Code = result.Code,
                        Message = $"Account ID {accountID} changed to {state} successfully",
                        Data = account
                    };
                }                
            }
            else
            {
                return new Result<Accounts>()
                {
                    Code = result.Code,
                    Message = result.Message
                };
            }
        }
        public async Task<Result<Accounts>> SetStateAsync(int accountID, AccountStateTypes state)
        {
            var result = await GetAccountbyIDAsync(accountID);

            if (result.Code == ResultCodes.Success)
            {
                var account = result.Data;

                if (account.Balance != 0.0m && state == AccountStateTypes.Closed)
                {
                    // Account has balance and therefor cannot be closed
                    return new Result<Accounts>()
                    {
                        Code = ResultCodes.BadRequest,
                        Message = $"Account has balance and therefor cannot be closed",
                        Data = account
                    };
                }
                else
                {
                    account.State = state;
                    _dBContext.Update(account);
                    await _dBContext.SaveChangesAsync();

                    return new Result<Accounts>()
                    {
                        Code = result.Code,
                        Message = $"Account ID {accountID} changed to {state} successfully",
                        Data = account
                    };
                }
            }
            else
            {
                return new Result<Accounts>()
                {
                    Code = result.Code,
                    Message = result.Message
                };
            }
        }
        public Result<Accounts> GetAccountbyID(int accountID)
        {
            var account = _dBContext.Set<Accounts>()
                .Where(a => a.AccountsId == accountID)
                //.Include(c => c.Cards)
                .SingleOrDefault();

            if (account != null)
            {
                return new Result<Accounts>()
                {
                    Code = ResultCodes.Success,
                    Data = account
                };
            }
            else
            {
                return new Result<Accounts>()
                {
                    Code = ResultCodes.NotFound,
                    Message = $"Account ID {accountID} not found!"
                };
            }
        }
        public async Task<Result<Accounts>> GetAccountbyIDAsync(int accountID)
        {
            var account = await _dBContext.Accounts
                .Where(a => a.AccountsId == accountID)
                .SingleOrDefaultAsync();

            if (account != null)
            {
                return new Result<Accounts>()
                {
                    Code = ResultCodes.Success,
                    Data = account
                };
            }
            else
            {
                return new Result<Accounts>()
                {
                    Code = ResultCodes.NotFound,
                    Message = $"Account ID {accountID} not found!"
                };
            }
        }
        public Result<Accounts> GetAccountbyCustomerID(int customerID, int accountID)
        {
            var result = _customer.GetCustomerbyID(customerID);

            if (result.Code == ResultCodes.Success)
            {
                var account = result.Data.Accounts
                  .Where(a => a.AccountsId == accountID)
                  .SingleOrDefault();

                return new Result<Accounts>()
                {
                    Code = ResultCodes.Success,
                    Data = account
                };
            }
            else
            {
                return new Result<Accounts>()
                {
                    Code = ResultCodes.NotFound,
                    Message = $"Could not find Account ID {accountID} for Customer ID {customerID}"
                };
            }
            
        }
        public async Task<Result<Accounts>> GetAccountbyCustomerIDAsync(int customerID, int accountID)
        {
            var result = await _customer.GetCustomerbyIDAsync(customerID);

            if (result.Code == ResultCodes.Success)
            {
                var account = result.Data.Accounts
                  .Where(a => a.AccountsId == accountID)
                  .SingleOrDefault();

                if (account != null)
                {
                    return new Result<Accounts>()
                    {
                        Code = ResultCodes.Success,
                        Data = account
                    };
                }
                else
                {
                    return new Result<Accounts>()
                    {
                        Code = ResultCodes.NotFound,
                        Message = $"Could not find Account ID {accountID} for Customer ID {customerID}"
                    };
                }
            }
            else
            {
                return new Result<Accounts>()
                {
                    Code = result.Code,
                    Message = result.Message
                };
            }
        }
        public async Task<Result<Accounts>> GetAccountbyNumberAsync(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                return new Result<Accounts>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"Account Number cannot be empty or null"
                };
            }

            var account = await _dBContext.Set<Accounts>()
                .Where(a => a.AccountNumber == accountNumber)
                .SingleOrDefaultAsync();
            if (account == null)
            {
                return new Result<Accounts>()
                {
                    Code = ResultCodes.NotFound,
                    Message = $"Account Number not found"
                };
            }

            return new Result<Accounts>()
            {
                Code = ResultCodes.Success,
                Message = $"Account number found",
                Data = account
            };
        }
    }
}

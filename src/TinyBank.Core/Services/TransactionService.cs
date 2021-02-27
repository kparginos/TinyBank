using System;
using System.Threading.Tasks;

using TinyBank.Core.Consts;
using TinyBank.Core.Data;
using TinyBank.Model;
using TinyBank.Core.Services.Interfaces;
using TinyBank.Core.Services.Options;
using TinyBank.Core.Services.Results;

namespace TinyBank.Core.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly TinyBankDBContext _dBContext;
        private IAccountsService _account;

        public TransactionService(TinyBankDBContext dBContext, IAccountsService account)
        {
            _dBContext = dBContext;
            _account = account;
        }

        public Result<Transaction> Register(int accountID, RegisterTransactionOptions options)
        {
            var result = _account.GetAccountbyID(accountID);
            if (result.Code != ResultCodes.Success)
                return new Result<Transaction>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"Could not find Account ID {accountID}"
                };

            // check account balance. Must be >= options.Type * options.Amount
            if (options.Type == Model.Types.TransactionType.Credit && result.Data.Balance + (int)options.Type * options.Amount < 0)
                return new Result<Transaction>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"Not enough balance for Account ID {accountID} !"
                };

            var transaction = new Transaction()
            {
                Amount = options.Amount,
                Type = options.Type,
                TransDescr = options.TransDescr
            };

            // update account balance
            result.Data.Balance += (int)options.Type * options.Amount;
            result.Data.Transactions.Add(transaction);

            try
            {
                _dBContext.SaveChanges();
                return new Result<Transaction>()
                {
                    Code = ResultCodes.Success,
                    Data = transaction
                };
            }
            catch (Exception ex)
            {
                return new Result<Transaction>()
                {
                    Code = ResultCodes.InternalServerError,
                    Message = $"Fail to save Transaction for account ID {accountID}. Details: {ex.Message}"
                };
            }
        }
        public async Task<Result<Transaction>> RegisterAsync(int accountID, RegisterTransactionOptions options)
        {
            var result = await _account.GetAccountbyIDAsync(accountID);
            if (result.Code != ResultCodes.Success)
                return new Result<Transaction>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"Could not find Account ID {accountID}"
                };

            // check account balance. Must be >= options.Type * options.Amount
            if (options.Type == Model.Types.TransactionType.Credit && result.Data.Balance + (int)options.Type * options.Amount < 0)
                return new Result<Transaction>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"Not enough balance for Account ID {accountID} !"
                };

            var transaction = new Transaction()
            {
                Amount = options.Amount,
                Type = options.Type,
                TransDescr = options.TransDescr
            };

            // update account balance
            result.Data.Balance += (int)options.Type * options.Amount;
            result.Data.Transactions.Add(transaction);

            try
            {
                await _dBContext.SaveChangesAsync();
                return new Result<Transaction>()
                {
                    Code = ResultCodes.Success,
                    Data = transaction
                };
            }
            catch (Exception ex)
            {
                return new Result<Transaction>()
                {
                    Code = ResultCodes.InternalServerError,
                    Message = $"Fail to save Transaction for account ID {accountID}. Details: {ex.Message}"
                };
            }
        }
    }
}

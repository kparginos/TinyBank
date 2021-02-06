using TinyBank.Core.Data;
using TinyBank.Core.Model;
using TinyBank.Core.Services.Interfaces;
using TinyBank.Core.Services.Options;

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
        public Transaction Register(int accountID, RegisterTransactionOptions options)
        {
            var account = _account.GetAccountbyID(accountID);
            if (account == null)
                return null;

            // check account balance. Must be >= options.Type * options.Amount
            if (options.Type == Model.Types.TransactionType.Credit && account.Balance + (int)options.Type * options.Amount < 0)
            return null;

            var transaction = new Transaction()
            {
                Amount = options.Amount,
                Type = options.Type,
                TransDescr = options.TransDescr
            };

            // update account balance
            account.Balance += (int)options.Type * options.Amount;
            account.Transactions.Add(transaction);

            _dBContext.SaveChanges();

            return transaction;
        }
    }
}

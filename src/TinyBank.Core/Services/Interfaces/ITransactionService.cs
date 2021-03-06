﻿using System.Threading.Tasks;

using TinyBank.Model;
using TinyBank.Core.Services.Options;
using TinyBank.Core.Services.Results;

namespace TinyBank.Core.Services.Interfaces
{
    public interface ITransactionService
    {
        public Result<Transaction> Register(int accountID, RegisterTransactionOptions options);
        public Task<Result<Transaction>> RegisterAsync(int accountID, RegisterTransactionOptions options);
    }
}

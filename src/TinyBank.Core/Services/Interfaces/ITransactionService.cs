using TinyBank.Core.Model;
using TinyBank.Core.Services.Options;

namespace TinyBank.Core.Services.Interfaces
{
    public interface ITransactionService
    {
        public Transaction Register(int accountID, RegisterTransactionOptions options);
    }
}

using TinyBank.Model.Types;

namespace TinyBank.Core.Services.Options
{
    public class RegisterTransactionOptions
    {
        public string TransDescr { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public TransactionStateType State { get; set; } = TransactionStateType.Pending;
    }
}

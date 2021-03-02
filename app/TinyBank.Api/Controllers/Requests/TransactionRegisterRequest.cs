using TinyBank.Core.Services.Options;

namespace TinyBank.API.Controllers.Requests
{
    public class TransactionRegisterRequest
    {
        public int CustomerID { get; set; }
        public int AccountID { get; set; }
        public RegisterTransactionOptions Options { get; set; }
    }
}

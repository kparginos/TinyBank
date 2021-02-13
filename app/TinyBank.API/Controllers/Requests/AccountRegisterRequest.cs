using TinyBank.Core.Services.Options;

namespace TinyBank.API.Controllers.Requests
{
    public class AccountRegisterRequest
    {
        public int CustomerID { get; set; }
        public RegisterAccountOptions Options { get; set; }
    }
}

using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using TinyBank.API.Controllers.Requests;
using TinyBank.Core.Consts;
using TinyBank.Core.Model.Types;
using TinyBank.Core.Services.Interfaces;

namespace TinyBank.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly ICustomerService _customer;
        private readonly IAccountsService _account;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger,
            ICustomerService customer,
            IAccountsService account)
        {
            _logger = logger;
            _account = account;
            _customer = customer;
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(
            [FromBody] AccountRegisterRequest request)
        {
            var resultCustomer = await _customer.GetCustomerbyIDAsync(request.CustomerID);

            if(resultCustomer.Code == ResultCodes.Success)
            {
                var resultAccount = await _account.RegisterAsync(resultCustomer.Data.CustomerId, request.Options);

                return Json(resultAccount);
            }
            else
            {
                return Json(resultCustomer);
            }
        }

        [HttpGet("{accountID:int}/state/{state:int}")]
        public async Task<IActionResult> SetAccountState(int accountID, AccountStateTypes state)
        {
            var result = await _account.SetStateAsync(accountID, state);

            return Json(result);
        }
    }
}

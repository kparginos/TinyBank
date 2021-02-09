using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using TinyBank.Core.Services.Interfaces;
using TinyBank.Core.Services.Options;

namespace TinyBank.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customer;
        private readonly IAccountsService _account;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ILogger<CustomersController> logger,
            ICustomerService customer,
            IAccountsService account)
        {
            _logger = logger;
            _customer = customer;
            _account = account;
        }

        [HttpPost]
        public IActionResult Register(
            [FromBody] RegisterCustomerOptions options)
        {
            var customer = _customer.Register(options);

            return Json(customer);
        }

        [HttpGet("{customerID:int}")]
        public IActionResult GetCustomerbyID(int customerID)
        {
            var customer = _customer.GetCustomerbyID(customerID);

            return Json(customer);
        }

        [HttpGet("{customerID:int}/{accountID:int}")]
        public IActionResult GetCustomerAccount(int customerID, int accountID)
        {
            var account = _account.GetAccountbyCustomerID(customerID, accountID);

            return Json(account);
        }
    }
}

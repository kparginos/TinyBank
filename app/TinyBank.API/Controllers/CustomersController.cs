using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TinyBank.Core.Consts;
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
        private readonly IFileParser _fileParser;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ILogger<CustomersController> logger,
            ICustomerService customer,
            IAccountsService account,
            IFileParser fileParser)
        {
            _logger = logger;
            _customer = customer;
            _account = account;
            _fileParser = fileParser;
        }

        [HttpPost("Register")]
        //[Route("Register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterCustomerOptions options)
        {
            var customer = await _customer.RegisterAsync(options);

            return Json(customer);
        }

        [HttpGet("{customerID:int}")]
        public async Task<IActionResult> GetCustomerbyID(int customerID)
        {
            var customer = await _customer.GetCustomerbyIDAsync(customerID);

            return Json(customer);
        }

        [HttpGet("{customerID:int}/account/{accountID:int}")]
        public async Task<IActionResult> GetCustomerAccount(int customerID, int accountID)
        {
            var account = await _account.GetAccountbyCustomerIDAsync(customerID, accountID);

            return Json(account);
        }

        [HttpGet("getcustomeraccounts/{customerID:int}")]
        public async Task<IActionResult> GetCustomerAccounts(int customerID)
        {
            var result = await _customer.GetCustomerAccountsAsync(customerID);

            return Json(result);
        }

        [HttpGet("{customerID:int}/state/{state:bool}")]
        public async Task<IActionResult> SetCustomerState(int customerID, bool state)
        {
            var result = await _customer.SetStateAsync(customerID, state);

            return Json(result);
        }

        //[Route("ExportCustomerData")]
        //[HttpPost]
        [HttpGet("ExportCustomerData")]
        public IActionResult ExportCustomerData()
        {
            var result = _fileParser.ExportCustomersToFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"files\CustomerData.xlsx"));

            return Json(result);
        }

        [HttpGet("ExportCustomerAccounts/{customerID:int}")]
        public IActionResult ExportCustomerAccounts(int customerID)
        {
            var result = _fileParser.ExportCustomerAccountsToFile(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"files\CustomerAccountsData.xlsx"),
                customerID);

            return Json(result);            
        }

    }
}

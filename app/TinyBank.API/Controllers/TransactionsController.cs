using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using TinyBank.API.Controllers.Requests;
using TinyBank.Core.Consts;
using TinyBank.Core.Services.Interfaces;

namespace TinyBank.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : Controller
    {
        private readonly ITransactionService _transaction;
        private readonly IAccountsService _account;

        public TransactionsController(
            ITransactionService transaction,
            IAccountsService account)
        {
            _transaction = transaction;
            _account = account;
        }

        [HttpPost]
        public async Task<IActionResult> Register(
            [FromBody] TransactionRegisterRequest request)
        {
            var accountResult = await _account.GetAccountbyCustomerIDAsync(request.CustomerID, request.AccountID);

            if (accountResult.Code == ResultCodes.Success)
            {
                var result = await _transaction.RegisterAsync(
                    accountResult.Data.AccountsId,
                    request.Options);

                return Json(result);
            }
            else
            {
                return Json(accountResult);
            }
        }
    }
}

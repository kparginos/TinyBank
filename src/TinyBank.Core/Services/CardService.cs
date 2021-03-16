using System;
using System.Threading.Tasks;

using TinyBank.Core.Consts;
using TinyBank.Core.Services.Interfaces;
using TinyBank.Core.Services.Options;
using TinyBank.Core.Services.Results;
using TinyBank.Data;
using TinyBank.Model;

namespace TinyBank.Core.Services
{
    public class CardService : ICardService
    {
        private readonly TinyBankDBContext _dbContext;
        private readonly IAccountsService _account;

        public CardService(TinyBankDBContext dbContext,
            IAccountsService account)
        {
            _dbContext = dbContext;
            _account = account;
        }
        public async Task<Result<Card>> RegisterAsync(RegisterCardOptions options)
        {
            var validations = BasicValidations(options);

            if(validations.Code != ResultCodes.Success)
            {
                return new Result<Card>()
                {
                    Code = validations.Code,
                    Message = validations.Message
                };
            }

            var account = await _account.GetAccountbyNumberAsync(options.AccountNumber); 

            var card = new Card()
            {
                Active = options.Active,
                CardNumber = options.CardNumber,
                AvailableBalance = options.AvailableBalance,
                Type = options.Type
            };

            account.Data.Cards.Add(card);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return new Result<Card>()
                {
                    Code = ResultCodes.InternalServerError,
                    Message = $"Fail to create new Card\n" +
                    $"Details: {ex.Message}\n" +
                    $"StackTrace: {ex.StackTrace}\n" +
                    $"{((ex.InnerException != null) ? "Inner Exception: " : "")}{((ex.InnerException != null) ? ex.InnerException.Message : "")}"
                };
            }

            return new Result<Card>()
            {
                Code = ResultCodes.Success,
                Message = $"New Card {card.CardNumber} added successfully",
                Data = card
            };
        }

        private Result<Card> BasicValidations(RegisterCardOptions options)
        {
            if(string.IsNullOrWhiteSpace(options.CardNumber))
            {
                return new Result<Card>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"Card number cannot be empty or null"
                };
            }
            if(options.Type == Model.Types.CardType.Undefined)
            {
                return new Result<Card>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"Card type cannot be {options.Type}"
                };
            }
            if (string.IsNullOrWhiteSpace(options.AccountNumber))
            {
                return new Result<Card>()
                {
                    Code = ResultCodes.BadRequest,
                    Message = $"Account Number cannot be empty or null"
                };
            }

            return new Result<Card>()
            {
                Code = ResultCodes.Success
            };
        }
    }
}

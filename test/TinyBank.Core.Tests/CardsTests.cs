using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using Xunit;

using TinyBank.Core.Services.Interfaces;
using TinyBank.Model;
using TinyBank.Core.Services.Options;
using TinyBank.Core.Consts;

namespace TinyBank.Core.Tests
{
    public class CardsTests : IClassFixture<TinyBankFixture>
    {
        private readonly ICardService _card;
        private readonly ICustomerService _customer;
        private readonly IAccountsService _account;

        public CardsTests(TinyBankFixture fixture)
        {
            _card = fixture.Scope.ServiceProvider
                .GetRequiredService<ICardService>();
            _customer = fixture.Scope.ServiceProvider
                .GetRequiredService<ICustomerService>();
            _account = fixture.Scope.ServiceProvider
                .GetRequiredService<IAccountsService>();
        }

        [Fact]
        public async void Add_Card_Success()
        {
            var options = new RegisterCardOptions()
            {
                Active = true,
                AvailableBalance = 0,
                CardNumber = "4587-6521-7832-1111",
                Type = Model.Types.CardType.Credit,
                AccountNumber = "4792-533-1051"
            };

            var cardResult = await _card.RegisterAsync(options);

            Assert.Equal(ResultCodes.Success, cardResult.Code);
        }
    }
}

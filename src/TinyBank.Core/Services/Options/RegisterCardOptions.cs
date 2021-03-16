using TinyBank.Model.Types;

namespace TinyBank.Core.Services.Options
{
    public class RegisterCardOptions
    {
        public string CardNumber { get; set; }
        public decimal?  AvailableBalance { get; set; }
        public CardType Type { get; set; }
        public bool Active { get; set; }
        public string AccountNumber { get; set; }
    }
}

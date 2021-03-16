using System.Threading.Tasks;
using TinyBank.Core.Services.Options;
using TinyBank.Core.Services.Results;
using TinyBank.Model;

namespace TinyBank.Core.Services.Interfaces
{
    public interface ICardService
    {
        public Task<Result<Card>> RegisterAsync(RegisterCardOptions options);
    }
}

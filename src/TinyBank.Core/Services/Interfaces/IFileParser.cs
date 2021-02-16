using System.Collections.Generic;
using System.Threading.Tasks;

using TinyBank.Core.Model;
using TinyBank.Core.Services.Results;

namespace TinyBank.Core.Services.Interfaces
{
    public interface IFileParser
    {
        public Result<List<CustomerFile>> LoadCustFile(string path);
    }
}

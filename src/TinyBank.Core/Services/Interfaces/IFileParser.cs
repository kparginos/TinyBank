using System.Collections.Generic;

using TinyBank.Core.Model;
using TinyBank.Core.Services.Results;

namespace TinyBank.Core.Services.Interfaces
{
    public interface IFileParser
    {
        public Result<List<CustomerFile>> LoadCustFile(string path);

        public Result<bool> ExportCustomersToFile(string exportPath);

        public Result<bool> ExportCustomerAccountsToFile(string exportPath, int customerID);
    }
}

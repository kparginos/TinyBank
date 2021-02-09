using System.Collections.Generic;

using TinyBank.Core.Model;
using TinyBank.Core.Services.Options;

namespace TinyBank.Core.Services.Interfaces
{
    public interface ICustomerService
    {
        public Customer Register(RegisterCustomerOptions options);
        public Customer GetCustomerbyID(int customerID);
    }
}

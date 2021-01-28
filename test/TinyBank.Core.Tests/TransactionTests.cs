using System.Linq;

using TinyBank.Core.Data;
using TinyBank.Core.Model;
using TinyBank.Core.Model.Types;

using Xunit;

namespace TinyBank.Core.Tests
{
    public class TransactionTests
    {
        [Fact]
        public void Add_New_Transaction_To_Account()
        {
            using var dbcontext = new TinyBankDBContext();

            var savedCustomer = dbcontext.Set<Customer>()
                .Where(c => c.CustBankID == "032846778")
                .SingleOrDefault();

            Assert.NotNull(savedCustomer);

            var savedAccount = dbcontext.Set<Accounts>()
                .Where(a => a.AccountNumber == "1558642182")
                .SingleOrDefault();

            Assert.NotNull(savedAccount);

            savedAccount.Transactions.Add(new Transaction()
            {
                Amount = 150.0m,
                TransDescr = "Tablet purchase",
                Type = TransactionType.Credit
            });

            dbcontext.Update(savedAccount);
            dbcontext.SaveChanges();
        }
    }
}

using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TinyBank.Core.Consts;
using TinyBank.Model.Types;
using TinyBank.Core.Services.Extentions;
using TinyBank.Core.Services.Interfaces;
using TinyBank.Core.Services.Options;

namespace TinyBank.ConsoleApp
{
    class Program
    {
        private static IServiceScope Scope;
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(Usage());
            }
            else
            {
                BuildServices();
                switch (args[0])
                {
                    case "-c":
                        RunCustomerService(args);
                        break;
                    case "-a":
                        RunAccountsService(args);
                        break;
                    case "-t":
                        RunTransactionsService(args);
                        break;
                    default:
                        Console.WriteLine(Usage());
                        break;
                };
            }
        }

        private static void RunTransactionsService(string[] args)
        {
            throw new NotImplementedException();
        }

        private static void RunAccountsService(string[] args)
        {
            if(args.Length == 1)
            {
                Console.WriteLine(Usage());
            }
            else
            {
                var _account = Scope.ServiceProvider.GetRequiredService<IAccountsService>();
                switch(args[1])
                {
                    case "-a":
                        var options = new RegisterAccountOptions()
                        {
                            AccountNumber = args[2].Split(',')[0],
                            AccountDescr = args[2].Split(',')[1],
                            Currency = args[2].Split(',')[2]
                        };
                        _account.Register(int.Parse(args[2].Split(',')[3]), options);
                        break;
                    case "-q":
                        var result = _account.GetAccountbyID(int.Parse(args[2]));
                        if (result.Code == ResultCodes.Success)
                        {
                            var account = result.Data;
                            Console.WriteLine(
                                $"Account ID: {account.AccountsId}\n" +
                                $"Account Number: {account.AccountNumber}\n" +
                                $"Account Description: {account.AccountDescription}\n" +
                                $"Account Balance: {account.Balance}\n" +
                                $"Account Currency: {account.Currency}\n" +
                                $"Created On: {account.Created}\n" +
                                $"Account Active: {account.State}");
                        }
                        else
                        {
                            Console.WriteLine("Account ID not found !");
                        }
                        break;
                }
            }
        }

        private static void RunCustomerService(string[] args)
        {
            if (args.Length == 1)
            {
                Console.WriteLine(Usage());
            }
            else
            {
                var _customer = Scope.ServiceProvider.GetRequiredService<ICustomerService>();                
                switch (args[1])
                {
                    case "-a":
                        var options = new RegisterCustomerOptions()
                        {
                            Name = args[2].Split(',')[0],
                            SureName = args[2].Split(',')[1],
                            VATNumber = args[2].Split(',')[2],
                            CustomerBankID = args[2].Split(',')[3],
                            CustType = (CustomerType)int.Parse(args[2].Split(',')[4]),
                            Address = args[2].Split(',')[5]
                        };
                        _customer.Register(options);
                        break;
                    case "-q":
                        var result = _customer.GetCustomerbyID(int.Parse(args[2]));
                        if (result.Code == ResultCodes.Success)
                        {
                            var cust = result.Data;
                            Console.WriteLine(
                                $"Customer ID: {cust.CustomerId}\n" +
                                $"Customer Name: {cust.Name}\n" +
                                $"Customer Sure Name: {cust.SureName}\n" +
                                $"Customer Bank ID: {cust.CustBankID}\n" +
                                $"Customer VAT Number: {cust.VatNumber}\n" +
                                $"Customer Address: {cust.Address}\n" +
                                $"Created On: {cust.Created}\n" +
                                $"Customer Active: {cust.Active}");
                        }
                        else
                        {
                            Console.WriteLine("Customer ID not found !");
                        }
                        break;
                }
            }
        }

        private static void BuildServices()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath($"{AppDomain.CurrentDomain.BaseDirectory}")
                .AddJsonFile("appsettings.json", false)
                .Build();

            // Initialize Depedency Conttainer
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddAppServices(config);

            Scope = serviceCollection
                .BuildServiceProvider()
                .CreateScope();
        }

        private static string Usage()
        {
            return "Invalid arguments.\nTo use the TinyBank.ConsoleApp from commandline, use the following syntax:\n" +
                "tinybank.consoleapp.exe [service] [action] [arguments]\n\n" +
                "[Services]\n" +
                "\t\t-c: Customer Service\n" +
                "\t\t-a: Accounts Service\n" +
                "\t\t-t: Transactions Service\n\n" +
                "[Actions]\n" +
                "\t\t-a: Add entity\n" +
                "\t\t-q: Query entity\n\n" +
                "[Arguments(specific per service)]\n" +
                "\t\t(When adding a customer provide the following separated by commas: customer_name,customer_surename,customer_VATNumber,customer_BankID,customer_Type,customer_Address\n" +
                "\t\t(When searching for a customer provide the following: customer_id\n" +
                "\t\t(When adding an account provide the following separated by commas: account_number,account_descr,account_currency,account_cust_id)\n";
        }
    }
}

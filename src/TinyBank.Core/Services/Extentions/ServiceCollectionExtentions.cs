using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TinyBank.Core.Config;
using TinyBank.Core.Config.Extentions;
using TinyBank.Data;
using TinyBank.Core.Services.Interfaces;

namespace TinyBank.Core.Services.Extentions
{
    public static class ServiceCollectionExtentions
    {
        public static void AddAppServices(
            this IServiceCollection @this, IConfiguration config)
        {
            @this.AddSingleton<AppConfig>(
                config.ReadAppConfiguration());

            @this.AddDbContext<TinyBankDBContext>(
                (serviceProvider, optionsBuilder) =>
                {
                    var appConfig = serviceProvider.GetRequiredService<AppConfig>();

                    optionsBuilder.UseSqlServer(appConfig.ConnString);
                });

            var appConfig = config.ReadAppConfiguration();
            if (appConfig.Environment == "Production")
            {
                @this.AddScoped<ICustomerService, CustomerService>();
                @this.AddScoped<IAccountsService, AccountsService>();
                @this.AddScoped<ITransactionService, TransactionService>();
                @this.AddScoped<IFileParser, CustomerFileService>();
            }
            else
            {
                @this.AddScoped<ICustomerService, CustomerService>();
                @this.AddScoped<IAccountsService, AccountsService>();
                @this.AddScoped<ITransactionService, TransactionService>();
                @this.AddScoped<IFileParser, CustomerFileService>();
            }

        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TinyBank.Core.Config;
using TinyBank.Core.Config.Extentions;
using TinyBank.Core.Data;

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
                //@this.AddScoped<ICustomerService, CustomerService>();
                //@this.AddScoped<IOrderService, OrderService>();
            }
            else
            {
                //@this.AddScoped<ICustomerService, DummyCustomerService>();
                //@this.AddScoped<IOrderService, OrderService>();
            }

        }
    }
}

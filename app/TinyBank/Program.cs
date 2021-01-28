using System;
using System.Reflection;

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

using TinyBank.Core.Data;
using TinyBank.Core.Config.Extentions;
using Microsoft.EntityFrameworkCore;

namespace TinyBank
{
    class Program
    {
        static void Main(string[] args)
        {
            //var config = new ConfigurationBuilder()
            //    .SetBasePath($"{AppDomain.CurrentDomain.BaseDirectory}")
            //    .AddJsonFile("appsettings.json", false)
            //    .Build();

            //var serviceCollection = new ServiceCollection();
            //serviceCollection.AddAppServices(config);
        }
    }

    // Use the following to create the migrations
    public class DbContextFactory : IDesignTimeDbContextFactory<TinyBankDBContext>
    {  
        public TinyBankDBContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath($"{AppDomain.CurrentDomain.BaseDirectory}")
                .AddJsonFile("appsettings.json", false)
                .Build();
            var config = configuration.ReadAppConfiguration();
            var optionsBuilder = new DbContextOptionsBuilder<TinyBankDBContext>();
            optionsBuilder.UseSqlServer(
                config.ConnString,
                options => {
                    options.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name); // ("TinyBank");
                });

            return new TinyBankDBContext(optionsBuilder.Options);
        }
    }
}

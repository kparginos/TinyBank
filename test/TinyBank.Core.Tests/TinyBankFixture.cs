using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TinyBank.Core.Services.Extentions;

namespace TinyBank.Core.Tests
{
    public class TinyBankFixture : IDisposable
    {
        public IServiceScope Scope { get; private set; }

        public TinyBankFixture()
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
        public void Dispose()
        {
            Scope.Dispose();
        }
    }
}

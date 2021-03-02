using Microsoft.Extensions.Configuration;

namespace TinyBank.Config.Extentions
{
    public static class ConfigurationExtentions
    {
        public static AppConfig ReadAppConfiguration(
            this IConfiguration @this)
        {
            //var minLoggingLevel = @this.GetSection("MinLoggingLevel").Value;
            var connString = @this.GetSection("ConnectionStrings").GetSection("TinyBankDatabase").Value;
            //var clientID = @this.GetSection("ClientConfig")
            //    .GetSection("ClientId").Value;
            //var clientSecret = @this.GetSection("ClientConfig")
            //    .GetSection("ClientSecret").Value;
            var environment = @this.GetSection("Environment").Value;

            return new AppConfig()
            {
                //MinLoggingLevel = minLoggingLevel,
                ConnString = connString,
                //ClientConfig = new ClientConfig()
                //{
                //    ClientId = clientID,
                //    ClientSecret = clientSecret
                //},
                Environment = environment
            };
        }
    }
}

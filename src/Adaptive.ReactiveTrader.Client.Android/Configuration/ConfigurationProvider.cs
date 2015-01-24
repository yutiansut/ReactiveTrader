using Adaptive.ReactiveTrader.Client.Configuration;

namespace Adaptive.ReactiveTrader.Client.Android.Configuration
{
    internal sealed class ConfigurationProvider : IConfigurationProvider
    {
        public string[] Servers
        {
            //get { return new[] { "http://localhost:8080" }; }
            get { return new[] { "https://reactivetrader.azurewebsites.net/signalr" }; }
        }
    }
}

namespace Adaptive.ReactiveTrader.Client.Configuration
{
    class ConfigurationProvider : IConfigurationProvider
    {
        public string[] Servers
        {
            get { return new[] {"http://reactivetrader.azurewebsites.net/signalr"}; }
        }
    }
}

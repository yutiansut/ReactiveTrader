namespace Adaptive.ReactiveTrader.Client.Configuration
{
    class ConfigurationProvider : IConfigurationProvider
    {
        public string[] Servers
        {
            get { return new[] { "http://localhost:8080" }; }
            //get { return new[] {"http://reactivetrader.azurewebsites.net/signalr"}; }
        }
    }
}

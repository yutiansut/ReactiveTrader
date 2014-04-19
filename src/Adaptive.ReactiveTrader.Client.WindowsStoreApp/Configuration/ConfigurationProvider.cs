namespace Adaptive.ReactiveTrader.Client.Configuration
{
    class ConfigurationProvider : IConfigurationProvider
    {
        public string[] Servers
        {
        //    get { return new[] {"http://reactivetrader.azurewebsites.net/signalr"}; }
            get { return new[] {"http://localhost:8080"}; }
        }
    }
}

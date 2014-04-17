using Adaptive.ReactiveTrader.Server.Pricing;
using Adaptive.ReactiveTrader.Server.Transport;
using Adaptive.ReactiveTrader.Web;
using Autofac;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Adaptive.ReactiveTrader.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //log4net.Config.XmlConfigurator.Configure();

            var bootstrapper = new Bootstrapper();
            var container = bootstrapper.Build();
            var priceFeed = container.Resolve<IPriceFeed>();
            priceFeed.Start();

            var hubConfiguration = new HubConfiguration
            {
                // you don't want to use that in prod, just when debugging
                EnableDetailedErrors = true,
                Resolver = new AutofacSignalRDependencyResolver(container)
            };

            app.MapSignalR(hubConfiguration);
        }
    }
}
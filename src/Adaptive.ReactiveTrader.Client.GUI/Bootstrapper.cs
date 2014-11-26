using Adaptive.ReactiveTrader.Client.Concurrency;
using Adaptive.ReactiveTrader.Client.Configuration;
using Adaptive.ReactiveTrader.Client.Domain.Instrumentation;
using Autofac;

namespace Adaptive.ReactiveTrader.Client
{
    public class Bootstrapper : BootstrapperBase
    {
        protected override void RegisterTypes(ContainerBuilder builder)
        {
            builder.RegisterType<ConfigurationProvider>().As<IConfigurationProvider>();
            builder.RegisterType<ConstantRateConfigurationProvider>().As<IConstantRateConfigurationProvider>();
            builder.RegisterType<UserProvider>().As<IUserProvider>();
            builder.RegisterType<ConcurrencyService>().As<IConcurrencyService>();
            builder.RegisterType<GnuPlot>().As<IGnuPlot>();
        }
    }
}
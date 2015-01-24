using Adaptive.ReactiveTrader.Client.Android.Concurrency;
using Adaptive.ReactiveTrader.Client.Android.Configuration;
using Adaptive.ReactiveTrader.Client.Concurrency;
using Adaptive.ReactiveTrader.Client.Configuration;
using Autofac;

namespace Adaptive.ReactiveTrader.Client.Android
{
    public class Bootstrapper : BootstrapperBase
    {
        protected override void RegisterTypes(ContainerBuilder builder)
        {
            builder.RegisterType<ConfigurationProvider>().As<IConfigurationProvider>();
            builder.RegisterType<ConstantRateConfigurationProvider>().As<IConstantRateConfigurationProvider>();
            builder.RegisterType<UserProvider>().As<IUserProvider>();
            builder.RegisterType<ConcurrencyService>().As<IConcurrencyService>().SingleInstance();
        }
    }
}
using System;

namespace Adaptive.ReactiveTrader.Client.Configuration
{
    class UserProvider : IUserProvider
    {
        public string Username
        {
            get { return "Trader-" + new Random().Next(100000); }
        }
    }
}

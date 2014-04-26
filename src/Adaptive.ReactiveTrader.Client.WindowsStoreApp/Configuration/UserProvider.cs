using System;

namespace Adaptive.ReactiveTrader.Client.Configuration
{
    class UserProvider : IUserProvider
    {
        public string Username
        {
            get { return "Win8-" + new Random().Next(1000); }
        }
    }
}

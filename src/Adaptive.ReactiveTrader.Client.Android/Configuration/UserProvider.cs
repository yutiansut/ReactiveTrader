using System;
using Adaptive.ReactiveTrader.Client.Configuration;

namespace Adaptive.ReactiveTrader.Client.Android.Configuration
{
    internal sealed class UserProvider : IUserProvider
    {
        public string Username
        {
            get { return "WPF-" + new Random().Next(1000); }    // xamtodo
        }
    }
}
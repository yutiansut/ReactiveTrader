using Windows.ApplicationModel.Core;

namespace Adaptive.ReactiveTrader.Client.Configuration
{
    class UserProvider : IUserProvider
    {
        public string Username
        {
            get
            {
                return "Trader-" + CoreApplication.Id;
            }
        }
    }
}

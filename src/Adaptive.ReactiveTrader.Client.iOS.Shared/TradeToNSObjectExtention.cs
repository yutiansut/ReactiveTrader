using System;
using Foundation;
using Adaptive.ReactiveTrader.Client.Domain.Models.Execution;
using Newtonsoft.Json;

namespace Adaptive.ReactiveTrader.Client.iOS.Shared
{

    public static class TradeToNSObjectExtention
    {
        public static NSString ToNSString(this ITrade trade)
        {
            var json = JsonConvert.SerializeObject(trade);
            return new NSString(json);
        }

        public static ITrade ToTrade(this NSString nsString)
        {
            return JsonConvert.DeserializeObject<Trade>(nsString);
        }

    }
}

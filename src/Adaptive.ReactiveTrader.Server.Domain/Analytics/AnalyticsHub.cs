using System.Threading.Tasks;
using Adaptive.ReactiveTrader.Server.Transport;
using Adaptive.ReactiveTrader.Shared;
using Adaptive.ReactiveTrader.Shared.DTO;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Adaptive.ReactiveTrader.Server.Analytics
{
    [HubName(ServiceConstants.Server.AnalyticsHub)]
    public class AnalyticsHub : Hub
    {
        private readonly IContextHolder _contextHolder;

        public AnalyticsHub(IContextHolder contextHolder)
        {
            _contextHolder = contextHolder;
        }

        [HubMethodName(ServiceConstants.Server.SubscribeAnalytics)]
        public async Task SubscribeAnalytics()
        {
            _contextHolder.AnalyticsHubClients = Clients;

            await Groups.Add(Context.ConnectionId, ServiceConstants.Server.AnalyticsGroup);
        }

        [HubMethodName(ServiceConstants.Server.UnsubscribeAnalytics)]
        public async Task UnsubscribeAnalytics()
        {
            await Groups.Remove(Context.ConnectionId, ServiceConstants.Server.AnalyticsGroup);
        }
    }
}

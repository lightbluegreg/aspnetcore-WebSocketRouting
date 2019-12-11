using System.Collections.Generic;
using WebSocketRouting.Routing;

namespace WebSocketRouting.Service
{
    public interface IWebSocketRoutingService
    {
        List<WebSocketRoute> GetRoutes();
    }
}
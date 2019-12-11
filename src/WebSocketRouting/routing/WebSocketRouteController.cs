namespace WebSocketRouting.Routing
{
    public abstract class WebSocketRouteController : IWebSocketRouteController {
        public WebSocketRouteController(IHttpContextAccessor context) {
            
        }
    }
}
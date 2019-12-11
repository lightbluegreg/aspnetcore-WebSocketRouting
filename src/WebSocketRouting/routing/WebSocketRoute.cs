using System.Collections.Generic;
using System.Reflection;

namespace WebSocketRouting.Routing
{
    internal class WebSocketRoute
    {
        public Regex Matcher { get; set; }
        public MethodInfo Handler { get; }
        public List<WebSocketRouteSegment> Segments { get; set; }

        public WebSocketRoute(MethodInfo handler)
        {
            Handler = handler;
            Segments = new List<WebSocketRouteSegment>();
        }
    }
}
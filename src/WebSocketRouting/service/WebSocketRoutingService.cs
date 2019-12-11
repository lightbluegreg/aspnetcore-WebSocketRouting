using System;
using System.Collections.Generic;
using System.Linq;
using WebSocketRouting.Routing;

namespace WebSocketRouting.Service
{
    public class WebSocketRoutingService : IWebSocketRoutingService
    {
        private readonly List<WebSocketRoute> _routes = new List<WebSocketRoute>();

        public WebSocketRoutingService(List<Type> controllers)
        {
            var methods = controllers.SelectMany(t => t.GetMethods().Where(m => m.GetCustomAttributes(false).OfType<WebSocketRouteAttribute>().Any())).ToList();

            methods.ForEach(m =>
            {
                var route = new WebSocketRoute(m);
                var attr = m.GetCustomAttributes(false).OfType<WebSocketRouteAttribute>().First() as WebSocketRouteAttribute;
                var template = @"^";
                var segments = attr.Template.Split('/', StringSplitOptions.RemoveEmptyEntries).ToList();
                for (var x = 0; x < segments.Count; x++)
                {
                    var segmentx = segments[x];
                    if (!segmentx.ContainsAny("?", "#", "&"))
                    {
                        var segment = WebSocketRouteSegment.Create(x, segmentx);
                        route.Segments.Add(segment);
                        template += $"\\/{segment.Value}";
                    }
                }
                template += ".*$";
                route.Matcher = new Regex(template, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                _routes.Add(route);
            });
        }
        public List<WebSocketRoute> GetRoutes() => _routes;
    }
}
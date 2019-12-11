using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using WebSocketRouting.Routing;

namespace WebSocketRouting.Middleware
{
    public class WebSocketRoutingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public WebSocketRoutingMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task Invoke(HttpContext context, IWebSocketRoutingService service)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var route = service.GetRoutes().FirstOrDefault(h => h.Matcher.IsMatch(context.Request.Path));

                if (route == null)
                {
                    context.Response.StatusCode = 400;
                    return;
                }

                using (var scope = _serviceProvider.CreateScope())
                {
                    var controllerContext = scope.ServiceProvider.GetRequiredService(route.Handler.DeclaringType);
                    var ws = await context.WebSockets.AcceptWebSocketAsync();
                    context.Items.Add("WebSocket", ws);
                    var match = route.Matcher.Match(context.Request.Path);
                    var parms = route.Segments.Where(s => s.IsParameter).OrderBy(o => o.Order).Select(s => match.Groups[s.Name].Value).ToArray();
                    var task = (Task)route.Handler.Invoke(controllerContext, parms);

                    await task;
                }
            }
            else context.Response.StatusCode = 400;
        }
    }
}
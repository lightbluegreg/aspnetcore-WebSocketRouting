using System;
using System.Linq;
using WebSocketRouting.Routing;
using WebSocketRouting.Service;
using WebSocketRouting.Middleware;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WebSocketExtensions
    {
        public static IApplicationBuilder UseWebSocketRouting(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            return app.UseMiddleware<WebSocketRoutingMiddleware>();
        }

        public static IServiceCollection AddWebSocketRouting(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            var controllers = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => typeof(IWebSocketRouteController).IsAssignableFrom(p) && p.IsClass).ToList();
            services.AddSingleton<IWebSocketRoutingService>(sp => new WebSocketRoutingService(controllers));
            controllers.ForEach(controller => services.AddScoped(controller));
            return services;
        }
    }
}
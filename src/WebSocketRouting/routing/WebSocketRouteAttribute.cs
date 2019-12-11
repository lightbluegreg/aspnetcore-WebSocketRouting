using System;

namespace WebSocketRouting.Routing
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class WebSocketRouteAttribute : Attribute
    {
        public string Template { get; set; }
        public WebSocketRouteAttribute(string template) => Template = template;
    }
}
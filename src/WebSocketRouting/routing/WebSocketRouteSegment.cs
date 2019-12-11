namespace WebSocketRouting.Routing
{
    internal class WebSocketRouteSegment
    {
        public int Order { get; set; }
        public string Template { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsParameter { get; set; }

        public static WebSocketRouteSegment Create(int order, string template)
        {
            var segment = new WebSocketRouteSegment
            {
                Order = order,
                Template = template,
                Name = template.TrimStart('{').TrimEnd('}'),
                IsParameter = template.StartsWith("{") && template.EndsWith("}")
            };
            segment.Value = segment.IsParameter ? $"(?<{segment.Name}>.*)" : segment.Name;
            return segment;
        }
    }
}
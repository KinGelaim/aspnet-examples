using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace Routing.Debugger.Middlewares;

public class RouteDebuggerMiddleware
{
    private readonly RequestDelegate _next;

    public RouteDebuggerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.OnStarting(static state =>
        {
            if (state is HttpContext httpContext && httpContext.GetEndpoint() is { } endpoint)
            {
                var info = GetEndpointInformation(endpoint, httpContext.GetRouteData());

                var response = httpContext.Response;
                var results = info.Select(kv => $"{kv.Key}:{kv.Value}; ");
                response.Headers.Append("X-ROUTE-INFO", string.Join("", results));
            }

            return Task.CompletedTask;
        }, context);

        System.Diagnostics.Debug.WriteLine("Before next middleware");

        await _next(context);

        System.Diagnostics.Debug.WriteLine("After next middleware");
    }

    private static Dictionary<string, string?> GetEndpointInformation(Endpoint endpoint, RouteData? routeData)
    {
        var elements = new Dictionary<string, string?>
            {
                { "Name", endpoint.DisplayName }
            };

        if (endpoint is RouteEndpoint route)
        {
            elements.Add("Pattern", route.RoutePattern.RawText);

            foreach (var metadata in route.Metadata)
            {
                if (metadata is RouteNameMetadata name)
                {
                    elements["Name"] = name.RouteName;
                }

                if (metadata is HttpMethodActionConstraint methods)
                {
                    elements["Methods"] = string.Join(",", methods.HttpMethods);
                }
            }
        }

        if (routeData is { } && routeData.Values.Count != 0)
        {
            elements["RouteData"] =
                string.Join(",", routeData.Values.Select(x => $"{x.Key}={x.Value}"));
        }

        return elements;
    }
}
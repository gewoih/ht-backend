using System.Reflection;
using HT.Api.Features.Base;

namespace HT.Api.Extensions;

public static class FeatureEndpointExtensions
{
    public static void MapFeatureEndpoints(this IEndpointRouteBuilder app,
        string prefix = "/api")
    {
        var root = app.MapGroup(prefix);

        var features = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => !t.IsAbstract && typeof(IEndpointFeature).IsAssignableFrom(t))
            .Select(Activator.CreateInstance)
            .Cast<IEndpointFeature>();

        foreach (var f in features)
            f.Map(root);
    }
}
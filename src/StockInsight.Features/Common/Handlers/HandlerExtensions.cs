using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace StockInsight.Features.Common.Handlers;

internal static class HandlerExtensions
{
    public static IServiceCollection AddHandlers(this IServiceCollection services, Assembly assembly)
    {
        var handlerTypes = assembly.DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } && type.IsAssignableTo(typeof(IHandler)));

        foreach (var handlerType in handlerTypes)
        {
            services.TryAddScoped(handlerType);
        }

        return services;
    }
}

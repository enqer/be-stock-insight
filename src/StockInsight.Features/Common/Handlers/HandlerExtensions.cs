using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace StockInsight.Features.Common.Handlers;

internal static class HandlerExtensions
{
    public static IServiceCollection AddHandlers(this IServiceCollection services, Assembly assembly)
    {
        foreach (var handlerType in assembly.ConcreteTypesOf<IHandler>())
        {
            services.TryAddScoped(handlerType);
        }

        return services;
    }
}

using System.Reflection;

namespace StockInsight.Features.Common;

internal static class AssemblyExtensions
{
    public static IEnumerable<Type> ConcreteTypesOf<T>(this Assembly assembly) =>
        assembly.DefinedTypes.Where(x => x is { IsAbstract: false, IsInterface: false } && x.IsAssignableTo(typeof(T)));
}

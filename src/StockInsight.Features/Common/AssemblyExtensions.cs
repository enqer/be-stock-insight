using System.Reflection;

namespace StockInsight.Features.Common;

internal static class AssemblyExtensions
{
    public static IEnumerable<Type> ConcreteTypesOf<T>(this Assembly assembly) =>
        assembly.DefinedTypes.Where(type => type is { IsAbstract: false, IsInterface: false } && type.IsAssignableTo(typeof(T)));
}

using System.Collections.ObjectModel;
using System.Data;

namespace PiraSoft.Tools.Data.Mapping;

public static class Mapper
{
    private static readonly IDictionary<Type, TypeMappings> _typeMappingsCatalog = new Dictionary<Type, TypeMappings>();

    public static IReadOnlyDictionary<Type, TypeMappings> TypeMappingsCatalog { get; } = new ReadOnlyDictionary<Type, TypeMappings>(_typeMappingsCatalog);

    public static TypeMappings AddTypeMappings<T>()
        => AddTypeMappings(typeof(T));

    public static TypeMappings AddTypeMappings(Type type)
        => AddTypeMappings(TypeMappings.Generate(type));

    public static TypeMappings AddTypeMappings(TypeMappings typeMappings)
    {
        _typeMappingsCatalog.AddOrUpdate(typeMappings.Type, typeMappings);
        return typeMappings;
    }

    public static TypeMappings EnsureTypeMappings(Type type)
        => _typeMappingsCatalog.TryGetValue(type) ?? AddTypeMappings(type);

    public static object? Map(Func<object> factory, DataRow? row)
        => Map(factory, row, null);

    public static object? Map(Func<object> factory, DataRow? row, TypeMappings? mappings)
        => Map<object>(factory, row, mappings);

    public static void Map(object target, DataRow? row)
        => Map(target, row, null);

    public static void Map(object target, DataRow? row, TypeMappings? mappings)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(row);
         
        mappings ??= EnsureTypeMappings(target.GetType());

        if (target.GetType() != mappings.Type)
        {
            throw new ArgumentException($"Type of {nameof(target)} is not the same of type specified in {nameof(mappings)}.");
        }

        mappings.Mappings.ForEach(i => i.Map(target, row));
    }

    public static T? Map<T>(DataRow? row)
        where T : new()
        => Map<T>(() => new T(), row, null);

    public static T? Map<T>(DataRow? row, TypeMappings? mappings)
        where T : new()
        => Map<T>(() => new T(), row, mappings);

    public static T? Map<T>(Func<T> factory, DataRow? row)
        => Map<T>(factory, row, null);

    public static T? Map<T>(Func<T> factory, DataRow? row, TypeMappings? mappings)
    {
        ArgumentNullException.ThrowIfNull(factory);

        if (row == null)
        {
            return default;
        }

        var retVal = factory();

        if (retVal == null)
        {
            throw new InvalidOperationException($"{nameof(factory)} returns a null object.");
        }

        Map(retVal, row, mappings);

        return retVal;
    }
}

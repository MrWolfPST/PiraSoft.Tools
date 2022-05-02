using PiraSoft.Tools.Data.Mapping;

namespace System.Data;

public static class RowExtensions
{
    public static object? Map(this DataRow row, Func<object> factory)
        => row.Map(factory, null);

    public static object? Map(this DataRow row, Func<object> factory, TypeMappings? mappings)
        => Mapper.Map(factory, row, mappings);

    public static void Map(this DataRow row, object target)
        => row.Map(target, null);

    public static void Map(this DataRow row, object target, TypeMappings? mappings)
        => Mapper.Map(target, row, mappings);

    public static T? Map<T>(this DataRow row) where T : new()
        => row.Map(() => new T(), null);

    public static T? Map<T>(this DataRow row, TypeMappings? mappings) where T : new()
        => row.Map(() => new T(), mappings);

    public static T? Map<T>(this DataRow row, Func<T> factory)
        => row.Map(factory, null);

    public static T? Map<T>(this DataRow row, Func<T> factory, TypeMappings? mappings)
        => Mapper.Map(factory, row, mappings);
}

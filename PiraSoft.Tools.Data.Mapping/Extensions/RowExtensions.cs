using PiraSoft.Tools.Data.Mapping;

namespace System.Data;

/// <summary>
/// A set of <see cref="DataRow"/> extension methods that use <see cref="Mapper"/>.
/// </summary>
public static class RowExtensions
{
    /// <summary>
    /// Returns the object popolated by data.
    /// </summary>
    /// <param name="row">The data.</param>
    /// <param name="factory">Method used for get instance of returning object.</param>
    /// <returns>The object popolated with retrieved data.</returns>
    public static object? Map(this DataRow row, Func<object> factory)
        => row.Map(factory, null);

    /// <summary>
    /// Returns the object popolated by data.
    /// </summary>
    /// <param name="row">The data.</param>
    /// <param name="factory">Method used for get instance of returning object.</param>
    /// <param name="mappings">Type mappings informations.</param>
    /// <returns>The object popolated with retrieved data.</returns>
    public static object? Map(this DataRow row, Func<object> factory, TypeMappings? mappings)
        => Mapper.Map(factory, row, mappings);

    /// <summary>
    /// Returns the object popolated by data.
    /// </summary>
    /// <param name="row">The data.</param>
    /// <param name="target">The target object.</param>
    public static void Map(this DataRow row, object target)
        => row.Map(target, null);

    /// <summary>
    /// Returns the object popolated by data.
    /// </summary>
    /// <param name="row">The data.</param>
    /// <param name="target">The target object.</param>
    /// <param name="mappings">Type mappings informations.</param>
    public static void Map(this DataRow row, object target, TypeMappings? mappings)
        => Mapper.Map(target, row, mappings);

    /// <summary>
    /// Returns the object popolated by data.
    /// </summary>
    /// <typeparam name="T">Type of the object to return. The type must be implement a parameterless constructor.</typeparam>
    /// <param name="row">The data.</param>
    /// <returns>The object popolated with retrieved data.</returns>
    public static T? Map<T>(this DataRow row) where T : new()
        => row.Map(() => new T(), null);

    /// <summary>
    /// Returns the object popolated by data.
    /// </summary>
    /// <typeparam name="T">Type of the object to return. The type must be implement a parameterless constructor.</typeparam>
    /// <param name="row">The data.</param>
    /// <param name="mappings">Type mappings informations.</param>
    /// <returns>The object popolated with retrieved data.</returns>
    public static T? Map<T>(this DataRow row, TypeMappings? mappings) where T : new()
        => row.Map(() => new T(), mappings);

    /// <summary>
    /// Returns the object popolated by data.
    /// </summary>
    /// <typeparam name="T">Type of the object to return.</typeparam>
    /// <param name="row">The data.</param>
    /// <param name="factory">Method used for get instance of returning object.</param>
    /// <returns>The object popolated with retrieved data.</returns>
    public static T? Map<T>(this DataRow row, Func<T> factory)
        => row.Map(factory, null);

    /// <summary>
    /// Returns the object popolated by data.
    /// </summary>
    /// <typeparam name="T">Type of the object to return.</typeparam>
    /// <param name="row">The data.</param>
    /// <param name="factory">Method used for get instance of returning object.</param>
    /// <param name="mappings">Type mappings informations.</param>
    /// <returns>The object popolated with retrieved data.</returns>
    public static T? Map<T>(this DataRow row, Func<T> factory, TypeMappings? mappings)
        => Mapper.Map(factory, row, mappings);
}

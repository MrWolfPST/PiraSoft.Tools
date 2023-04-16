using PiraSoft.Tools.Data.Mapping;
using System.Data.Common;

namespace PiraSoft.Tools.Data;

/// <summary>
/// A set of <see cref="Command{TDataReader, TParameter}"/> extension methods that use <see cref="Mapper"/>.
/// </summary>
public static class CommandGetDataRowExtensions
{
    /// <summary>
    /// Returns the object popolated by data.
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="factory">Method used for get instance of returning object.</param>
    /// <returns>The object popolated with retrieved data.</returns>
    public static object? Get<TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<object> factory)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.Get(factory, null);

    /// <summary>
    /// Returns the object popolated by data.
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="factory">Method used for get instance of returning object.</param>
    /// <param name="mappings">Type mappings informations.</param>
    /// <returns>The object popolated with retrieved data.</returns>
    public static object? Get<TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<object> factory, TypeMappings? mappings)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => Mapper.Map(factory, target.GetDataRow(), mappings);

    /// <summary>
    /// Returns the object popolated by data.
    /// </summary>
    /// <typeparam name="T">Type of the object to return. The type must be implement a parameterless constructor.</typeparam>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <returns>The object popolated with retrieved data.</returns>
    public static T? Get<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target)
        where T : new()
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.Get(() => new T(), null);

    /// <summary>
    /// Returns the object popolated by data.
    /// </summary>
    /// <typeparam name="T">Type of the object to return. The type must be implement a parameterless constructor.</typeparam>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="mappings">Type mappings informations.</param>
    /// <returns>The object popolated with retrieved data.</returns>
    public static T? Get<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target, TypeMappings? mappings)
        where T : new()
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.Get(() => new T(), mappings);

    /// <summary>
    /// Returns the object popolated by data.
    /// </summary>
    /// <typeparam name="T">Type of the object to return.</typeparam>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="factory">Method used for get instance of returning object.</param>
    /// <returns>The object popolated with retrieved data.</returns>
    public static T? Get<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<T> factory)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.Get(factory, (TypeMappings?)null);

    /// <summary>
    /// Returns the object popolated by data.
    /// </summary>
    /// <typeparam name="T">Type of the object to return.</typeparam>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="factory">Method used for get instance of returning object.</param>
    /// <param name="mappings">Type mappings informations.</param>
    /// <returns>The object popolated with retrieved data.</returns>
    public static T? Get<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<T> factory, TypeMappings? mappings)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => Mapper.Map(factory, target.GetDataRow(), mappings);
}

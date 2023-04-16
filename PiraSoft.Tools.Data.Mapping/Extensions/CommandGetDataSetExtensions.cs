using PiraSoft.Tools.Data.Mapping;
using System.Data;
using System.Data.Common;

namespace PiraSoft.Tools.Data;

/// <summary>
/// A set of <see cref="Command{TDataReader, TParameter}"/> extension methods that use <see cref="Mapper"/>.
/// </summary>
public static class CommandGetDataSetExtensions
{
    /// <summary>
    /// Returns a list of objects popolated by retrieved data.
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="factory">Method used for get instance of returning object.</param>
    /// <returns>The list of objects popolated with retrieved data.</returns>
    public static IEnumerable<object> List<TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<object> factory)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.List(factory, null);

    /// <summary>
    /// Returns a list of objects popolated by retrieved data.
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="factory">Method used for get instance of returning object.</param>
    /// <param name="mappings">Type mappings informations.</param>
    /// <returns>The list of objects popolated with retrieved data.</returns>
    public static IEnumerable<object> List<TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<object> factory, TypeMappings? mappings)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => from r in target.GetDataTable().AsEnumerable() select Mapper.Map(factory, r, mappings);

    /// <summary>
    /// Returns a list of objects popolated by retrieved data.
    /// </summary>
    /// <typeparam name="T">Type of the object to return. The type must be implement a parameterless constructor.</typeparam>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <returns>The list of objects popolated with retrieved data.</returns>
    public static IEnumerable<T> List<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target)
        where T : new()
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.List(() => new T(), null);

    /// <summary>
    /// Returns a list of objects popolated by retrieved data.
    /// </summary>
    /// <typeparam name="T">Type of the object to return. The type must be implement a parameterless constructor.</typeparam>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="mappings">Type mappings informations.</param>
    /// <returns>The list of objects popolated with retrieved data.</returns>
    public static IEnumerable<T> List<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target, TypeMappings? mappings)
        where T : new()
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.List(() => new T(), mappings);

    /// <summary>
    /// Returns a list of objects popolated by retrieved data.
    /// </summary>
    /// <typeparam name="T">Type of the object to return.</typeparam>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="factory">Method used for get instance of returning object.</param>
    /// <returns>The list of objects popolated with retrieved data.</returns>
    public static IEnumerable<T> List<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<T> factory)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.List(factory, null);

    /// <summary>
    /// Returns a list of objects popolated by retrieved data.
    /// </summary>
    /// <typeparam name="T">Type of the object to return.</typeparam>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="factory">Method used for get instance of returning object.</param>
    /// <param name="mappings">Type mappings informations.</param>
    /// <returns>The list of objects popolated with retrieved data.</returns>
    public static IEnumerable<T> List<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<T> factory, TypeMappings? mappings)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => from r in target.GetDataTable().AsEnumerable() select Mapper.Map(factory, target.GetDataRow(), mappings);
}

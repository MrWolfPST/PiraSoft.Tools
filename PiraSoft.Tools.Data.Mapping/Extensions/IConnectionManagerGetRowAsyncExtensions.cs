using PiraSoft.Tools.Data.Mapping;
using System.Data;
using System.Data.Common;

namespace PiraSoft.Tools.Data;

/// <summary>
/// A set of <see cref="IConnectionManager{TDataReader, TParameter}"/> extension methods that use <see cref="Mapper"/>.
/// </summary>
public static class IConnectionManagerGetRowAsyncExtensions
{
    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManagerGetRowExtensions.Get{TDataReader, TParameter}(IConnectionManager{TDataReader, TParameter}, Func{object}, string, CommandType, int?, IEnumerable{TParameter}?)"/>.
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="factory">Method used for get instance of returning object.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task<object?> GetAsync<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, Func<object> factory, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetAsync(factory, null, commandText, commandType, commandTimeout, parameters, cancellationToken);

    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManagerGetRowExtensions.Get{TDataReader, TParameter}(IConnectionManager{TDataReader, TParameter}, Func{object}, TypeMappings?, string, CommandType, int?, IEnumerable{TParameter}?)"/>
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="factory">Method used for get instance of returning object.</param>
    /// <param name="mappings">Type mappings informations.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task<object?> GetAsync<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, Func<object> factory, TypeMappings? mappings, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => Mapper.Map(factory, await target.GetDataRowAsync(commandText, commandType, commandTimeout, parameters, cancellationToken), mappings);

    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManagerGetRowExtensions.Get{T, TDataReader, TParameter}(IConnectionManager{TDataReader, TParameter}, string, CommandType, int?, IEnumerable{TParameter}?)"/>
    /// </summary>
    /// <typeparam name="T">Type of the object to return. The type must be implement a parameterless constructor.</typeparam>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task<T?> GetAsync<T, TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default)
        where T : new()
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetAsync(() => new T(), null, commandText, commandType, commandTimeout, parameters, cancellationToken);

    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManagerGetRowExtensions.Get{T, TDataReader, TParameter}(IConnectionManager{TDataReader, TParameter}, TypeMappings?, string, CommandType, int?, IEnumerable{TParameter}?)"/>
    /// </summary>
    /// <typeparam name="T">Type of the object to return. The type must be implement a parameterless constructor.</typeparam>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="mappings">Type mappings informations.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task<T?> GetAsync<T, TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, TypeMappings? mappings, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default)
        where T : new()
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetAsync(() => new T(), mappings, commandText, commandType, commandTimeout, parameters, cancellationToken);

    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManagerGetRowExtensions.Get{T, TDataReader, TParameter}(IConnectionManager{TDataReader, TParameter}, Func{T}, string, CommandType, int?, IEnumerable{TParameter}?)"/>
    /// </summary>
    /// <typeparam name="T">Type of the object to return.</typeparam>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="factory">Method used for get instance of returning object.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task<T?> GetAsync<T, TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, Func<T> factory, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetAsync(factory, null, commandText, commandType, commandTimeout, parameters, cancellationToken);

    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManagerGetRowExtensions.Get{T, TDataReader, TParameter}(IConnectionManager{TDataReader, TParameter}, Func{T}, TypeMappings?, string, CommandType, int?, IEnumerable{TParameter}?)"/>
    /// </summary>
    /// <typeparam name="T">Type of the object to return.</typeparam>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="factory">Method used for get instance of returning object.</param>
    /// <param name="mappings">Type mappings informations.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task<T?> GetAsync<T, TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, Func<T> factory, TypeMappings? mappings, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => Mapper.Map(factory, await target.GetDataRowAsync(commandText, commandType, commandTimeout, parameters, cancellationToken), mappings);
}

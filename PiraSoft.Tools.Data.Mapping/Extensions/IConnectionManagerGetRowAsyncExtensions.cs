using System.Data;
using System.Data.Common;

namespace PiraSoft.Tools.Data.Mapping;

public static class IConnectionManagerGetRowAsyncExtensions
{
    public static Task<object?> GetAsync<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, Func<object> factory, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetAsync(factory, null, commandText, commandType, commandTimeout, parameters, cancellationToken);

    public static async Task<object?> GetAsync<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, Func<object> factory, TypeMappings? mappings, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => Mapper.Map(factory, await target.GetDataRowAsync(commandText, commandType, commandTimeout, parameters, cancellationToken), mappings);

    public static Task<T> GetAsync<T, TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default)
        where T : new()
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetAsync(() => new T(), null, commandText, commandType, commandTimeout, parameters, cancellationToken);

    public static Task<T> GetAsync<T, TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, TypeMappings? mappings, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default)
        where T : new()
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetAsync(() => new T(), mappings, commandText, commandType, commandTimeout, parameters, cancellationToken);

    public static Task<T> GetAsync<T, TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, Func<T> factory, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetAsync(factory, null, commandText, commandType, commandTimeout, parameters, cancellationToken);

    public static async Task<T> GetAsync<T, TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, Func<T> factory, TypeMappings? mappings, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => Mapper.Map(factory, await target.GetDataRowAsync(commandText, commandType, commandTimeout, parameters, cancellationToken), mappings);
}

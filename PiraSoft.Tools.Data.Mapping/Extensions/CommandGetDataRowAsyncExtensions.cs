using PiraSoft.Tools.Data.Mapping;
using System.Data.Common;

namespace PiraSoft.Tools.Data;

public static class CommandGetDataRowAsyncExtensions
{
    public static Task<object?> GetAsync<TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<object> factory, CancellationToken cancellationToken = default)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetAsync(factory, null, cancellationToken);

    public static async Task<object?> GetAsync<TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<object> factory, TypeMappings? mappings, CancellationToken cancellationToken = default)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => Mapper.Map(factory, await target.GetDataRowAsync(cancellationToken), mappings);

    public static Task<T?> GetAsync<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target, CancellationToken cancellationToken = default)
        where T : new()
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetAsync(() => new T(), null, cancellationToken);

    public static Task<T?> GetAsync<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target, TypeMappings? mappings, CancellationToken cancellationToken = default)
        where T : new()
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetAsync(() => new T(), mappings, cancellationToken);

    public static Task<T?> GetAsync<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<T> factory, CancellationToken cancellationToken = default)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetAsync(factory, null, cancellationToken);

    public static async Task<T?> GetAsync<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<T> factory, TypeMappings? mappings, CancellationToken cancellationToken = default)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => Mapper.Map(factory, await target.GetDataRowAsync(cancellationToken), mappings);
}

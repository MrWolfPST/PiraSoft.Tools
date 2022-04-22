using System.Data;
using System.Data.Common;

namespace PiraSoft.Tools.Data.Mapping;

public static class CommandGetDataSetAsyncExtensions
{
    public static Task<IEnumerable<object>> ListAsync<TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<object> factory, CancellationToken cancellationToken = default)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ListAsync(factory, null, cancellationToken);

    public static async Task<IEnumerable<object>> ListAsync<TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<object> factory, TypeMappings? mappings, CancellationToken cancellationToken = default)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => from r in (await target.GetDataTableAsync(cancellationToken)).AsEnumerable() select Mapper.Map(factory, r, mappings);

    public static Task<IEnumerable<T>> ListAsync<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target, CancellationToken cancellationToken = default)
        where T : new()
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ListAsync(() => new T(), null, cancellationToken);

    public static Task<IEnumerable<T>> ListAsync<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target, TypeMappings? mappings, CancellationToken cancellationToken = default)
        where T : new()
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ListAsync(() => new T(), mappings, cancellationToken);

    public static Task<IEnumerable<T>> ListAsync<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<T> factory, CancellationToken cancellationToken = default)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ListAsync(factory, null, cancellationToken);

    public static async Task<IEnumerable<T>> ListAsync<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<T> factory, TypeMappings? mappings, CancellationToken cancellationToken = default)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => from r in (await target.GetDataTableAsync(cancellationToken)).AsEnumerable() select (T)Mapper.Map(factory, target.GetDataRow(), mappings);
}

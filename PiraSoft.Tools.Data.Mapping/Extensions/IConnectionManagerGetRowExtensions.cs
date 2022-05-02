using PiraSoft.Tools.Data.Mapping;
using System.Data;
using System.Data.Common;

namespace PiraSoft.Tools.Data;
public static class IConnectionManagerGetRowExtensions
{
    public static object? Get<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, Func<object> factory, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.Get(factory, null, commandText, commandType, commandTimeout, parameters);

    public static object? Get<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, Func<object> factory, TypeMappings? mappings, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => Mapper.Map(factory, target.GetDataRow(commandText, commandType, commandTimeout, parameters), mappings);

    public static T? Get<T, TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default)
        where T : new()
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.Get(() => new T(), null, commandText, commandType, commandTimeout, parameters);

    public static T? Get<T, TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, TypeMappings? mappings, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default)
        where T : new()
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.Get(() => new T(), mappings, commandText, commandType, commandTimeout, parameters);

    public static T? Get<T, TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, Func<T> factory, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.Get(factory, null, commandText, commandType, commandTimeout, parameters);

    public static T? Get<T, TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, Func<T> factory, TypeMappings? mappings, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => Mapper.Map(factory, target.GetDataRow(commandText, commandType, commandTimeout, parameters), mappings);
}

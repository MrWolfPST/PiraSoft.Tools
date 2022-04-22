using System.Data.Common;

namespace PiraSoft.Tools.Data.Mapping;

public static class CommandGetDataRowExtensions
{
    public static object? Get<TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<object> factory)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.Get(factory, null);

    public static object? Get<TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<object> factory, TypeMappings? mappings)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => Mapper.Map(factory, target.GetDataRow(), mappings);

    public static T Get<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target)
        where T : new()
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.Get(() => new T(), null);

    public static T Get<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target, TypeMappings? mappings)
        where T : new()
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.Get(() => new T(), mappings);

    public static T Get<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<T> factory)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.Get(factory, (TypeMappings?)null);

    public static T Get<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<T> factory, TypeMappings? mappings)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => (T)Mapper.Map(factory, target.GetDataRow(), mappings);
}

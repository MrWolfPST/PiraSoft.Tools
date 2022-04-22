using System.Data;
using System.Data.Common;

namespace PiraSoft.Tools.Data.Mapping;

public static class CommandGetDataSetExtensions
{
    public static IEnumerable<object> List<TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<object> factory)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.List(factory, null);

    public static IEnumerable<object> List<TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<object> factory, TypeMappings? mappings)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => from r in target.GetDataTable().AsEnumerable() select Mapper.Map(factory, r, mappings);

    public static IEnumerable<T> List<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target)
        where T : new()
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.List(() => new T(), null);

    public static IEnumerable<T> List<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target, TypeMappings? mappings)
        where T : new()
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.List(() => new T(), mappings);

    public static IEnumerable<T> List<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<T> factory)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.List(factory, null);

    public static IEnumerable<T> List<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<T> factory, TypeMappings? mappings)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => from r in target.GetDataTable().AsEnumerable() select (T)Mapper.Map(factory, target.GetDataRow(), mappings);
}

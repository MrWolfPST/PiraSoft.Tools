using System.Data;
using System.Data.Common;

namespace PiraSoft.Tools.Data;

public static class ICommandHostExtensions
{
    public static Command<TDataReader, TParameter> StoredProcedure<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string storedProdecureName)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => new(target, storedProdecureName, CommandType.StoredProcedure);

    public static Command<TDataReader, TParameter> Query<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string query)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => new(target, query, CommandType.Text);
}

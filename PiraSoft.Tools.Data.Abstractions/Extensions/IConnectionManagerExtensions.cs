using System.Data;
using System.Data.Common;

namespace PiraSoft.Tools.Data;

public static class IConnectionManagerExtensions
{
    public static int ExecuteNonQuery<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteNonQuery(commandText, commandType, commandTimeout, parameters);

    public static T? ExecuteScalar<T, TDataReader, TParameter>(IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteScalar<T>(commandText, commandType, commandTimeout, parameters);

    public static TDataReader ExecuteReader<TDataReader, TParameter>(IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CommandBehavior behavior = CommandBehavior.Default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteReader(commandText, commandType, commandTimeout, behavior, parameters);

    public static DataSet GetDataSet<TDataReader, TParameter>(IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataSet(commandText, commandType, commandTimeout, parameters);

    public static DataTable GetDataTable<TDataReader, TParameter>(IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataTable(commandText, commandType, commandTimeout, parameters);

    public static DataRow? GetDataRow<TDataReader, TParameter>(IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataRow(commandText, commandType, commandTimeout, parameters);

    public static Task<int> ExecuteNonQueryAsync<TDataReader, TParameter>(IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CancellationToken cancellationToken = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteNonQueryAsync(commandText, commandType, commandTimeout, parameters, cancellationToken);

    public static Task<T?> ExecuteScalarAsync<T, TDataReader, TParameter>(IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CancellationToken cancellationToken = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteScalarAsync<T>(commandText, commandType, commandTimeout, parameters, cancellationToken);

    public static Task<TDataReader> ExecuteReaderAsync<TDataReader, TParameter>(IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CommandBehavior behavior = CommandBehavior.Default, CancellationToken cancellationToken = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteReaderAsync(commandText, commandType, commandTimeout, behavior, parameters, cancellationToken);

    public static Task<DataSet> GetDataSetAsync<TDataReader, TParameter>(IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CancellationToken cancellationToken = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataSetAsync(commandText, commandType, commandTimeout, parameters, cancellationToken);

    public static Task<DataTable> GetDataTableAsync<TDataReader, TParameter>(IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CancellationToken cancellationToken = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataTableAsync(commandText, commandType, commandTimeout, parameters, cancellationToken);

    public static Task<DataRow?> GetDataRowAsync<TDataReader, TParameter>(IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CancellationToken cancellationToken = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataRowAsync(commandText, commandType, commandTimeout, parameters, cancellationToken);
}

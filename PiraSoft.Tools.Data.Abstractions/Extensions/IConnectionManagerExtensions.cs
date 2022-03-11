using System.Data;
using System.Data.Common;

namespace PiraSoft.Tools.Data;

public static class IConnectionManagerExtensions
{
    #region Command

    public static Command<TDataReader, TParameter> StoredProcedure<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string storedProdecureName)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => new(target, storedProdecureName, CommandType.StoredProcedure);

    public static Command<TDataReader, TParameter> Query<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string query)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => new(target, query, CommandType.Text);

    #endregion

    #region ExecuteNonQuery

    public static int ExecuteNonQuery<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteNonQuery(commandText, CommandType.StoredProcedure, default, parameters);

    public static int ExecuteNonQuery<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteNonQuery(commandText, commandType, commandTimeout, parameters);

    public static Task<int> ExecuteNonQueryAsync<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteNonQueryAsync(commandText, CommandType.StoredProcedure, default, parameters, default);

    public static Task<int> ExecuteNonQueryAsync<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CancellationToken cancellationToken = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteNonQueryAsync(commandText, commandType, commandTimeout, parameters, cancellationToken);

    #endregion

    #region ExecuteScalar

    public static T? ExecuteScalar<T, TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteScalar<T>(commandText, CommandType.StoredProcedure, default, parameters);

    public static T? ExecuteScalar<T, TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteScalar<T>(commandText, commandType, commandTimeout, parameters);

    public static Task<T?> ExecuteScalarAsync<T, TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteScalarAsync<T>(commandText, CommandType.StoredProcedure, default, parameters, default);

    public static Task<T?> ExecuteScalarAsync<T, TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CancellationToken cancellationToken = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteScalarAsync<T>(commandText, commandType, commandTimeout, parameters, cancellationToken);

    #endregion

    #region ExecuteReader

    public static TDataReader ExecuteReader<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteReader(commandText, CommandType.StoredProcedure, default, CommandBehavior.Default, parameters);

    public static TDataReader ExecuteReader<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CommandBehavior behavior = CommandBehavior.Default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteReader(commandText, commandType, commandTimeout, behavior, parameters);

    public static Task<TDataReader> ExecuteReaderAsync<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteReaderAsync(commandText, CommandType.StoredProcedure, default, CommandBehavior.Default, parameters, default);

    public static Task<TDataReader> ExecuteReaderAsync<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CommandBehavior behavior = CommandBehavior.Default, CancellationToken cancellationToken = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteReaderAsync(commandText, commandType, commandTimeout, behavior, parameters, cancellationToken);

    #endregion

    #region GetDataSet

    public static DataSet GetDataSet<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataSet(commandText, CommandType.StoredProcedure, default, parameters);

    public static DataSet GetDataSet<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataSet(commandText, commandType, commandTimeout, parameters);

    public static Task<DataSet> GetDataSetAsync<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataSetAsync(commandText, CommandType.StoredProcedure, default, parameters, default);

    public static Task<DataSet> GetDataSetAsync<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CancellationToken cancellationToken = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataSetAsync(commandText, commandType, commandTimeout, parameters, cancellationToken);

    #endregion

    #region GetDataTable

    public static DataTable GetDataTable<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataTable(commandText, CommandType.StoredProcedure, default, parameters);

    public static DataTable GetDataTable<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataTable(commandText, commandType, commandTimeout, parameters);

    public static Task<DataTable> GetDataTableAsync<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataTableAsync(commandText, CommandType.StoredProcedure, default, parameters, default);

    public static Task<DataTable> GetDataTableAsync<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CancellationToken cancellationToken = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataTableAsync(commandText, commandType, commandTimeout, parameters, cancellationToken);

    #endregion

    #region GetDataRow

    public static DataRow? GetDataRow<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataRow(commandText, CommandType.StoredProcedure, default, parameters);

    public static DataRow? GetDataRow<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataRow(commandText, commandType, commandTimeout, parameters);

    public static Task<DataRow?> GetDataRowAsync<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataRowAsync(commandText, CommandType.StoredProcedure, default, parameters, default);

    public static Task<DataRow?> GetDataRowAsync<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CancellationToken cancellationToken = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataRowAsync(commandText, commandType, commandTimeout, parameters, cancellationToken);

    #endregion
}

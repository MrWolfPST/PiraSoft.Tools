using System.Data;
using System.Data.Common;

namespace PiraSoft.Tools.Data;

/// <summary>
/// A set of <see cref="IConnectionManagerExtensions"/> extension methods.
/// </summary>
public static class IConnectionManagerExtensions
{
    #region Command

    /// <summary>
    /// Initialize a stored procedure command.
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="storedProdecureName">Stored procedure to execute.</param>
    /// <returns>An instance of <see cref="Command{TDataReader, TParameter}"/> that represents the stored procedure fo execute.</returns>
    public static Command<TDataReader, TParameter> StoredProcedure<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string storedProdecureName)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => new(target, storedProdecureName, CommandType.StoredProcedure);

    /// <summary>
    /// Initializire a query command.
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="query">Query to execute.</param>
    /// <returns>An instance of <see cref="Command{TDataReader, TParameter}"/> that represents the query fo execute.</returns>
    public static Command<TDataReader, TParameter> Query<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string query)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => new(target, query, CommandType.Text);

    #endregion

    #region ExecuteNonQuery

    /// <summary>
    /// Executes the SQL statement.
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>The number of rows affected.</returns>
    public static int ExecuteNonQuery<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteNonQuery(commandText, CommandType.StoredProcedure, default, parameters);

    /// <summary>
    /// Executes the SQL statement.
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>The number of rows affected.</returns>
    public static int ExecuteNonQuery<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteNonQuery(commandText, commandType, commandTimeout, parameters);

    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManagerExtensions.ExecuteNonQuery{TDataReader, TParameter}(IConnectionManager{TDataReader, TParameter}, string, TParameter[])"/>.
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task<int> ExecuteNonQueryAsync<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteNonQueryAsync(commandText, CommandType.StoredProcedure, default, parameters, default);

    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManagerExtensions.ExecuteNonQuery{TDataReader, TParameter}(IConnectionManager{TDataReader, TParameter}, string, CommandType, int?, TParameter[])"/>.
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task<int> ExecuteNonQueryAsync<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CancellationToken cancellationToken = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteNonQueryAsync(commandText, commandType, commandTimeout, parameters, cancellationToken);

    #endregion

    #region ExecuteScalar

    /// <summary>
    /// Executes the SQL statement, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>The casted value of first column of the first row in the result set returned by the query.</returns>
    public static T? ExecuteScalar<T, TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteScalar<T>(commandText, CommandType.StoredProcedure, default, parameters);

    /// <summary>
    /// Executes the SQL statement, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>The casted value of first column of the first row in the result set returned by the query.</returns>
    public static T? ExecuteScalar<T, TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteScalar<T>(commandText, commandType, commandTimeout, parameters);

    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManagerExtensions.ExecuteScalar{T, TDataReader, TParameter}(IConnectionManager{TDataReader, TParameter}, string, TParameter[])"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task<T?> ExecuteScalarAsync<T, TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteScalarAsync<T>(commandText, CommandType.StoredProcedure, default, parameters, default);

    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManagerExtensions.ExecuteScalarAsync{T, TDataReader, TParameter}(IConnectionManager{TDataReader, TParameter}, string, CommandType, int?, CancellationToken, TParameter[])"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task<T?> ExecuteScalarAsync<T, TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CancellationToken cancellationToken = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteScalarAsync<T>(commandText, commandType, commandTimeout, parameters, cancellationToken);

    #endregion

    #region ExecuteReader

    /// <summary>
    /// Executes the SQL statement and returns an <see cref="DbDataReader"/>.
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>An <see cref="DbDataReader"/> object.</returns>
    public static TDataReader ExecuteReader<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteReader(commandText, CommandType.StoredProcedure, default, CommandBehavior.Default, parameters);

    /// <summary>
    /// Executes the SQL statement and returns an <see cref="DbDataReader"/>.
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="behavior"></param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>An <see cref="DbDataReader"/> object.</returns>
    public static TDataReader ExecuteReader<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CommandBehavior behavior = CommandBehavior.Default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteReader(commandText, commandType, commandTimeout, behavior, parameters);

    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManagerExtensions.ExecuteReaderAsync{TDataReader, TParameter}(IConnectionManager{TDataReader, TParameter}, string, TParameter[])"/>.
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task<TDataReader> ExecuteReaderAsync<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteReaderAsync(commandText, CommandType.StoredProcedure, default, CommandBehavior.Default, parameters, default);

    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManagerExtensions.ExecuteReaderAsync{TDataReader, TParameter}(IConnectionManager{TDataReader, TParameter}, string, CommandType, int?, CommandBehavior, CancellationToken, TParameter[])"/>.
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="behavior"></param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task<TDataReader> ExecuteReaderAsync<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CommandBehavior behavior = CommandBehavior.Default, CancellationToken cancellationToken = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ExecuteReaderAsync(commandText, commandType, commandTimeout, behavior, parameters, cancellationToken);

    #endregion

    #region GetDataSet

    /// <summary>
    /// Executes the SQL statement and returns an <see cref="DataSet"/>.
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>An <see cref="DataSet"/> object.</returns>
    public static DataSet GetDataSet<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataSet(commandText, CommandType.StoredProcedure, default, parameters);

    /// <summary>
    /// Executes the SQL statement and returns an <see cref="DataSet"/>.
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>An <see cref="DataSet"/> object.</returns>
    public static DataSet GetDataSet<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataSet(commandText, commandType, commandTimeout, parameters);

    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManagerExtensions.GetDataSet{TDataReader, TParameter}(IConnectionManager{TDataReader, TParameter}, string, TParameter[])"/>.
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task<DataSet> GetDataSetAsync<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataSetAsync(commandText, CommandType.StoredProcedure, default, parameters, default);

    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManagerExtensions.GetDataSet{TDataReader, TParameter}(IConnectionManager{TDataReader, TParameter}, string, CommandType, int?, TParameter[])"/>.
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task<DataSet> GetDataSetAsync<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CancellationToken cancellationToken = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataSetAsync(commandText, commandType, commandTimeout, parameters, cancellationToken);

    #endregion

    #region GetDataTable

    /// <summary>
    /// Executes the SQL statement and returns an <see cref="DataTable"/>.
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>An <see cref="DataTable"/> object.</returns>
    public static DataTable GetDataTable<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataTable(commandText, CommandType.StoredProcedure, default, parameters);

    /// <summary>
    /// Executes the SQL statement and returns an <see cref="DataTable"/>.
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>An <see cref="DataTable"/> object.</returns>
    public static DataTable GetDataTable<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataTable(commandText, commandType, commandTimeout, parameters);

    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManagerExtensions.GetDataTable{TDataReader, TParameter}(IConnectionManager{TDataReader, TParameter}, string, TParameter[])"/>.
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task<DataTable> GetDataTableAsync<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataTableAsync(commandText, CommandType.StoredProcedure, default, parameters, default);

    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManagerExtensions.GetDataTable{TDataReader, TParameter}(IConnectionManager{TDataReader, TParameter}, string, CommandType, int?, TParameter[])"/>.
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task<DataTable> GetDataTableAsync<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CancellationToken cancellationToken = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataTableAsync(commandText, commandType, commandTimeout, parameters, cancellationToken);

    #endregion

    #region GetDataRow

    /// <summary>
    /// Executes the SQL statement and returns an <see cref="DataRow"/>.
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>An <see cref="DataRow"/> object.</returns>
    public static DataRow? GetDataRow<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataRow(commandText, CommandType.StoredProcedure, default, parameters);

    /// <summary>
    /// Executes the SQL statement and returns an <see cref="DataRow"/>.
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>An <see cref="DataRow"/> object.</returns>
    public static DataRow? GetDataRow<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataRow(commandText, commandType, commandTimeout, parameters);

    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManagerExtensions.GetDataRow{TDataReader, TParameter}(IConnectionManager{TDataReader, TParameter}, string, TParameter[])"/>.
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task<DataRow?> GetDataRowAsync<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataRowAsync(commandText, CommandType.StoredProcedure, default, parameters, default);

    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManagerExtensions.GetDataRow{TDataReader, TParameter}(IConnectionManager{TDataReader, TParameter}, string, CommandType, int?, TParameter[])"/>.
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task<DataRow?> GetDataRowAsync<TDataReader, TParameter>(this IConnectionManager<TDataReader, TParameter> target, string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CancellationToken cancellationToken = default, params TParameter[] parameters)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.GetDataRowAsync(commandText, commandType, commandTimeout, parameters, cancellationToken);

    #endregion
}

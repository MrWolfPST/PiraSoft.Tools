using System.Data;
using System.Data.Common;

namespace PiraSoft.Tools.Data;

/// <summary>
/// Provides a set of methods for data base interaction.
/// </summary>
/// <typeparam name="TDataReader">The type of data reader.</typeparam>
/// <typeparam name="TParameter">The type of parameters.</typeparam>
public interface IConnectionManager<TDataReader, TParameter>
        where TDataReader : DbDataReader
        where TParameter : DbParameter
{
    /// <summary>
    /// An instance of <see cref="DbProviderFactory"/> used for creating instance of a provider's implementation of the data source classes.
    /// </summary>
    DbProviderFactory Factory { get; }

    /// <summary>
    /// Executes the SQL statement.
    /// </summary>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>The number of rows affected.</returns>
    int ExecuteNonQuery(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default);

    /// <summary>
    /// Executes the SQL statement, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
    /// </summary>
    /// <typeparam name="T">Type of returned value.</typeparam>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>The casted value of first column of the first row in the result set returned by the query.</returns>
    T? ExecuteScalar<T>(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default);

    /// <summary>
    /// Executes the SQL statement and returns an <see cref="DbDataReader"/>.
    /// </summary>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="behavior">One of the enumeration values that specifies the command behavior.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>An <see cref="DbDataReader"/> object.</returns>
    TDataReader ExecuteReader(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CommandBehavior behavior = CommandBehavior.Default, IEnumerable<TParameter>? parameters = default);

    /// <summary>
    /// Executes the SQL statement and returns an <see cref="DataSet"/>.
    /// </summary>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>An <see cref="DataSet"/> object.</returns>
    DataSet GetDataSet(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default);

    /// <summary>
    /// Executes the SQL statement and returns an <see cref="DataTable"/>.
    /// </summary>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>An <see cref="DataTable"/> object.</returns>
    DataTable GetDataTable(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default);

    /// <summary>
    /// Executes the SQL statement and returns an <see cref="DataRow"/>.
    /// </summary>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>An <see cref="DataRow"/> object.</returns>
    DataRow? GetDataRow(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default);

    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManager{TDataReader, TParameter}.ExecuteNonQuery(string, CommandType, int?, IEnumerable{TParameter}?)"/>.
    /// </summary>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<int> ExecuteNonQueryAsync(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default);

    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManager{TDataReader, TParameter}.ExecuteScalar{T}(string, CommandType, int?, IEnumerable{TParameter}?)"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<T?> ExecuteScalarAsync<T>(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default);

    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManager{TDataReader, TParameter}.ExecuteReader(string, CommandType, int?, CommandBehavior, IEnumerable{TParameter}?)"/>.
    /// </summary>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="behavior">One of the enumeration values that specifies the command behavior.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<TDataReader> ExecuteReaderAsync(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CommandBehavior behavior = CommandBehavior.Default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default);

    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManager{TDataReader, TParameter}.GetDataSet(string, CommandType, int?, IEnumerable{TParameter}?)"/>.
    /// </summary>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<DataSet> GetDataSetAsync(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default);

    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManager{TDataReader, TParameter}.GetDataTable(string, CommandType, int?, IEnumerable{TParameter}?)"/>.
    /// </summary>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<DataTable> GetDataTableAsync(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default);

    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManager{TDataReader, TParameter}.GetDataRow(string, CommandType, int?, IEnumerable{TParameter}?)"/>.
    /// </summary>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<DataRow?> GetDataRowAsync(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default);
}

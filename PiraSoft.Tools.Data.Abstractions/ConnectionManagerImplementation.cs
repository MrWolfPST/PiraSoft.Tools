using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;

namespace PiraSoft.Tools.Data;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable S2436 // Methods should not have too many parameters
#pragma warning restore IDE0079 // Remove unnecessary suppression
/// <summary>
/// The base implementation of data base interaction class.
/// </summary>
/// <typeparam name="TConnection">The type of connection.</typeparam>
/// <typeparam name="TDataReader">The type of data reader.</typeparam>
/// <typeparam name="TDataAdapter">The type of data adapter</typeparam>
/// <typeparam name="TCommand">The type of command</typeparam>
/// <typeparam name="TParameter">The type of parameters.</typeparam>
public abstract class ConnectionManagerImplementation<TConnection, TDataReader, TDataAdapter, TCommand, TParameter>
    : IConnectionManager<TDataReader, TParameter>
        where TConnection : DbConnection
        where TDataReader : DbDataReader
        where TDataAdapter : DbDataAdapter
        where TCommand : DbCommand
        where TParameter : DbParameter
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning restore S2436 // Methods should not have too many parameters
#pragma warning restore IDE0079 // Remove unnecessary suppression
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionManagerImplementation{TConnection, TDataReader, TDataAdapter, TCommand, TParameter}"/> class.
    /// </summary>
    /// <param name="logger">An instance of <see cref="ILogger"/>.</param>
    /// <param name="factory">An instance of <see cref="DbProviderFactory"/> used for creating instance of a provider's implementation of the data source classes.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="logger"/> is null.
    /// <paramref name="factory"/> is null.
    /// </exception>
    protected ConnectionManagerImplementation(ILogger? logger, DbProviderFactory? factory)
    {
        this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.Factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    /// <summary>
    /// An instance of <see cref="DbProviderFactory"/> used for creating instance of a provider's implementation of the data source classes.
    /// </summary>
    public DbProviderFactory Factory { get; }

    /// <summary>
    /// An instance of <see cref="ILogger"/>.
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Executes the SQL statement.
    /// </summary>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>The number of rows affected.</returns>
    public int ExecuteNonQuery(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default)
        => this.Execute((c) => c.ExecuteNonQuery(), commandText, commandType, commandTimeout, parameters);

    /// <summary>
    /// Executes the SQL statement, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
    /// </summary>
    /// <typeparam name="T">Type of returned value.</typeparam>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>The casted value of first column of the first row in the result set returned by the query.</returns>
    public T? ExecuteScalar<T>(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default)
        => this.Execute((c) => (T?)c.ExecuteScalar(), commandText, commandType, commandTimeout, parameters);

    /// <summary>
    /// Executes the SQL statement and returns an <see cref="DbDataReader"/>.
    /// </summary>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="behavior">One of the enumeration values that specifies the command behavior.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>An <see cref="DbDataReader"/> object.</returns>
    public TDataReader ExecuteReader(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CommandBehavior behavior = CommandBehavior.Default, IEnumerable<TParameter>? parameters = default)
    {
        if ((behavior & CommandBehavior.CloseConnection) == CommandBehavior.CloseConnection)
        {
            throw new ArgumentException($"Value {CommandBehavior.CloseConnection} is not allowd.", nameof(behavior));
        }

        return this.Execute((c) => (TDataReader)c.ExecuteReader(behavior), commandText, commandType, commandTimeout, parameters);
    }

    /// <summary>
    /// Executes the SQL statement and returns an <see cref="DataSet"/>.
    /// </summary>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>An <see cref="DataSet"/> object.</returns>
    public DataSet GetDataSet(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default)
    {
        var ds = new DataSet();

        return this.Execute((c) =>
        {
            using var dataAdapter = this.CreateDataAdapter(c);
            dataAdapter.Fill(ds);

            return ds;
        }, commandText, commandType, commandTimeout, parameters);
    }

    /// <summary>
    /// Executes the SQL statement and returns an <see cref="DataTable"/>.
    /// </summary>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>An <see cref="DataTable"/> object.</returns>
    public DataTable GetDataTable(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default)
    {
        var ds = this.GetDataSet(commandText, commandType, commandTimeout, parameters);

        return ds.Tables.AsEnumerable().FirstOrDefault() ?? new DataTable();
    }

    /// <summary>
    /// Executes the SQL statement and returns an <see cref="DataRow"/>.
    /// </summary>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <returns>An <see cref="DataRow"/> object.</returns>
    public DataRow? GetDataRow(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default)
    {
        var dt = this.GetDataTable(commandText, commandType, commandTimeout, parameters);

        return dt.Rows.AsEnumerable().FirstOrDefault();
    }

    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManager{TDataReader, TParameter}.ExecuteNonQuery(string, CommandType, int?, IEnumerable{TParameter}?)"/>.
    /// </summary>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task<int> ExecuteNonQueryAsync(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default)
        => this.ExecuteAsync((c) => c.ExecuteNonQueryAsync(cancellationToken), commandText, commandType, commandTimeout, parameters, cancellationToken);

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
    public async Task<T?> ExecuteScalarAsync<T>(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default)
        => await this.ExecuteAsync(async (c) => (T?)(await c.ExecuteScalarAsync(cancellationToken)), commandText, commandType, commandTimeout, parameters, cancellationToken);

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
    public Task<TDataReader> ExecuteReaderAsync(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CommandBehavior behavior = CommandBehavior.Default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default)
    {
        if ((behavior & CommandBehavior.CloseConnection) == CommandBehavior.CloseConnection)
        {
            throw new ArgumentException($"Value {CommandBehavior.CloseConnection} is not allowd.", nameof(behavior));
        }

        return this.ExecuteReaderInternalAsync(commandText, commandType, commandTimeout, behavior, parameters, cancellationToken);
    }

    private async Task<TDataReader> ExecuteReaderInternalAsync(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CommandBehavior behavior = CommandBehavior.Default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default)
        => await this.ExecuteAsync(async (c) => (TDataReader)(await c.ExecuteReaderAsync(behavior, cancellationToken)), commandText, commandType, commandTimeout, parameters, cancellationToken);

    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManager{TDataReader, TParameter}.GetDataSet(string, CommandType, int?, IEnumerable{TParameter}?)"/>.
    /// </summary>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<DataSet> GetDataSetAsync(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default)
    {
        var ds = new DataSet();
        var cnn = this.GetConnection();
        var nextResulset = false;

        try
        {
            await cnn.OpenAsync(cancellationToken);

            var dr = await this.ExecuteAsync(async (c) => (TDataReader)(await c.ExecuteReaderAsync(cancellationToken)), cnn, commandText, commandType, commandTimeout, parameters, cancellationToken);

            do
            {
                var dt = new DataTable();

                await dt.LoadAsync(dr, cancellationToken);
                ds.Tables.Add(dt);
                nextResulset = await dr.NextResultAsync(cancellationToken);
            } while (!dr.IsClosed && nextResulset);

            return ds;
        }
        finally
        {
            await cnn.CloseAsync();
            cnn.Dispose();
        }
    }

    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManager{TDataReader, TParameter}.GetDataTable(string, CommandType, int?, IEnumerable{TParameter}?)"/>.
    /// </summary>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns></returns>
    public async Task<DataTable> GetDataTableAsync(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default)
    {
        var ds = await this.GetDataSetAsync(commandText, commandType, commandTimeout, parameters, cancellationToken);

        return ds.Tables.AsEnumerable().FirstOrDefault() ?? new DataTable();
    }

    /// <summary>
    /// This is the asynchronous version of <see cref="IConnectionManager{TDataReader, TParameter}.GetDataRow(string, CommandType, int?, IEnumerable{TParameter}?)"/>.
    /// </summary>
    /// <param name="commandText">The SQL statement.</param>
    /// <param name="commandType">The value indicating how the <paramref name="commandText"/> is to be interpreted. Default value is <see cref="CommandType.StoredProcedure"/></param>
    /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error.</param>
    /// <param name="parameters">The list of parametes used by the SQL statement.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<DataRow?> GetDataRowAsync(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default)
    {
        var dt = await this.GetDataTableAsync(commandText, commandType, commandTimeout, parameters, cancellationToken);

        return dt.Rows.AsEnumerable().FirstOrDefault();
    }

    /// <summary>
    /// Returns a connection to database.
    /// </summary>
    /// <returns>Connection used for database interaction.</returns>
    protected internal abstract TConnection GetConnection();

    /// <summary>
    /// Provides an implementation of connection dispose.
    /// </summary>
    /// <param name="connection">The connection to dispose.</param>
    protected virtual void DisposeConnection(TConnection connection)
        => connection.Dispose();

    private TDataAdapter CreateDataAdapter(TCommand command)
    {
        var retVal = this.Factory.CreateDataAdapter() as TDataAdapter ?? throw new ApplicationException($"Unable to create an instance of {typeof(TDataAdapter)}.");

        retVal.SelectCommand = command;

        return retVal;
    }

    private TCommand CreateCommand(TConnection connection, string commandText, CommandType commandType, int? commandTimeout, IEnumerable<TParameter>? parameters)
    {
        if (commandTimeout.GetValueOrDefault() < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(commandTimeout), "Value must be greater than 0.");
        }

        var retVal = this.Factory.CreateCommand() as TCommand ?? throw new ApplicationException($"Unable to create an instance of {typeof(TCommand)}.");

        retVal.Connection = connection;
        retVal.CommandText = commandText;
        retVal.CommandType = commandType;
        retVal.CommandTimeout = commandTimeout ?? retVal.CommandTimeout;

        (parameters ?? Enumerable.Empty<TParameter>()).ForEach(i => retVal.Parameters.Add(i));

        return retVal;
    }

    private T Execute<T>(Func<TCommand, T> action, string commandText, CommandType commandType, int? commandTimeout, IEnumerable<TParameter>? parameters)
    {
        var closeConnection = false;

        var _connection = this.GetConnection();
        using var command = this.CreateCommand(_connection, commandText, commandType, commandTimeout, parameters);

#pragma warning disable CA2254 // Template should be a static expression
        this.Logger.LogDebug(command.Dump());
#pragma warning restore CA2254 // Template should be a static expression

        try
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
                closeConnection = true;
            }

            return action(command);
        }
        catch (Exception ex)
        {
#pragma warning disable CA2254 // Template should be a static expression
            this.Logger.LogError(ex, command.Dump());
#pragma warning restore CA2254 // Template should be a static expression
            throw;
        }
        finally
        {
            if (closeConnection)
            {
                _connection.Close();
                this.DisposeConnection(_connection);
            }
        }
    }

    private Task<T> ExecuteAsync<T>(Func<TCommand, Task<T>> action, string commandText, CommandType commandType, int? commandTimeout, IEnumerable<TParameter>? parameters, CancellationToken cancellationToken)
        => this.ExecuteAsync(action, this.GetConnection(), commandText, commandType, commandTimeout, parameters, cancellationToken);

    private async Task<T> ExecuteAsync<T>(Func<TCommand, Task<T>> action, TConnection connection, string commandText, CommandType commandType, int? commandTimeout, IEnumerable<TParameter>? parameters, CancellationToken cancellationToken)
    {
        var closeConnection = false;

        using var command = this.CreateCommand(connection, commandText, commandType, commandTimeout, parameters);

#pragma warning disable CA2254 // Template should be a static expression
        this.Logger.LogDebug(command.Dump());
#pragma warning restore CA2254 // Template should be a static expression

        try
        {
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync(cancellationToken);
                closeConnection = true;
            }

            return await action(command);
        }
        catch (Exception ex)
        {
#pragma warning disable CA2254 // Template should be a static expression
            this.Logger.LogError(ex, command.Dump());
#pragma warning restore CA2254 // Template should be a static expression
            throw;
        }
        finally
        {
            if (closeConnection)
            {
                await connection.CloseAsync();
                this.DisposeConnection(connection);
            }
        }
    }
}

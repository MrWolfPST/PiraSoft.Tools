using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;

namespace PiraSoft.Tools.Data;

public abstract class ConnectionManagerImplementation<TConnection, TDataReader, TDataAdapter, TCommand, TParameter>
    : IConnectionManager<TDataReader, TParameter>
        where TConnection : DbConnection
        where TDataReader : DbDataReader
        where TDataAdapter : DbDataAdapter
        where TCommand : DbCommand
        where TParameter : DbParameter
{
    protected ConnectionManagerImplementation(ILogger logger, DbProviderFactory factory)
    {
        this.Logger = logger;
        this.Factory = factory;
    }

    public DbProviderFactory Factory { get; }

    protected ILogger Logger { get; }

    public int ExecuteNonQuery(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default)
        => this.Execute((c) => c.ExecuteNonQuery(), commandText, commandType, commandTimeout, parameters);

    public T? ExecuteScalar<T>(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default)
        => this.Execute((c) => (T?)c.ExecuteScalar(), commandText, commandType, commandTimeout, parameters);

    public TDataReader ExecuteReader(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CommandBehavior behavior = CommandBehavior.Default, IEnumerable<TParameter>? parameters = default)
        => this.Execute((c) => (TDataReader)c.ExecuteReader(behavior), commandText, commandType, commandTimeout, parameters);

    public DataSet GetDataSet(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default)
    {
        DataSet? ds = new DataSet();

        return this.Execute((c) =>
        {
            using TDataAdapter? dataAdapter = this.CreateDataAdapter(c);
            dataAdapter.Fill(ds);

            return ds;
        }, commandText, commandType, commandTimeout, parameters);
    }

    public DataTable GetDataTable(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default)
    {
        DataTable dt = new DataTable();

        this.Execute((c) =>
        {
            using TDataAdapter? dataAdapter = this.CreateDataAdapter(c);
            dataAdapter.Fill(dt);
        }, commandText, commandType, commandTimeout, parameters);

        return dt;
    }

    public DataRow? GetDataRow(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default)
    {
        DataTable? dt = this.GetDataTable(commandText, commandType, commandTimeout, parameters);

        return dt.Rows.AsEnumerable().FirstOrDefault();
    }

    public Task<int> ExecuteNonQueryAsync(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default)
        => this.Execute((c) => c.ExecuteNonQueryAsync(cancellationToken), commandText, commandType, commandTimeout, parameters);

    public async Task<T?> ExecuteScalarAsync<T>(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default)
        => await this.ExecuteAsync(async (c) => (T?)(await c.ExecuteScalarAsync(cancellationToken)), commandText, commandType, commandTimeout, parameters);

    public async Task<TDataReader> ExecuteReaderAsync(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CommandBehavior behavior = CommandBehavior.Default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default)
        => await this.ExecuteAsync(async (c) => (TDataReader)(await c.ExecuteReaderAsync(behavior)), commandText, commandType, commandTimeout, parameters);

    public async Task<DataSet> GetDataSetAsync(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default)
    {
        DataSet? ds = new DataSet();
        TDataReader? dr = await this.ExecuteAsync(async (c) => (TDataReader)(await c.ExecuteReaderAsync()), commandText, commandType, commandTimeout, parameters);

        do
        {
            DataTable? dt = new DataTable();

            dt.Load(dr);
            ds.Tables.Add(dt);
        } while (await dr.NextResultAsync(cancellationToken));

        return ds;
    }

    public async Task<DataTable> GetDataTableAsync(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default)
    {
        DataTable dt = new DataTable();
        TDataReader? dr = await this.ExecuteAsync(async (c) => (TDataReader)(await c.ExecuteReaderAsync()), commandText, commandType, commandTimeout, parameters);

        dt.Load(dr);

        return dt;
    }

    public async Task<DataRow?> GetDataRowAsync(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default)
    {
        DataTable? dt = await this.GetDataTableAsync(commandText, commandType, commandTimeout, parameters, cancellationToken);

        return dt.Rows.AsEnumerable().FirstOrDefault();
    }

    protected internal abstract TConnection GetConnection();

    protected virtual void DisposeConnection(TConnection connection)
        => connection.Dispose();

    private TDataAdapter CreateDataAdapter(TCommand command)
    {
        TDataAdapter? retVal = (TDataAdapter?)this.Factory.CreateDataAdapter() ?? throw new ApplicationException($"Unable to create an instance of {typeof(TDataAdapter)}.");

        retVal.SelectCommand = command;

        return retVal;
    }

    private TCommand CreateCommand(TConnection connection, string commandText, CommandType commandType, int? commandTimeout, IEnumerable<TParameter>? parameters)
    {
        if (commandTimeout.GetValueOrDefault() < 0)
            throw new ArgumentOutOfRangeException(nameof(commandTimeout), "Value must be greater than 0.");

        TCommand? retVal = (TCommand?)this.Factory.CreateCommand() ?? throw new ApplicationException($"Unable to create an instance of {typeof(TCommand)}.");

        retVal.Connection = connection;
        retVal.CommandText = commandText;
        retVal.CommandType = commandType;
        retVal.CommandTimeout = commandTimeout ?? retVal.CommandTimeout;

        (parameters ?? Enumerable.Empty<TParameter>()).ForEach(i => retVal.Parameters.Add(i));

        return retVal;
    }

    private void Execute(Action<TCommand> action, string commandText, CommandType commandType, int? commandTimeout, IEnumerable<TParameter>? parameters)
        => this.Execute((c) => { action(c); return 0; }, commandText, commandType, commandTimeout, parameters);

    private T Execute<T>(Func<TCommand, T> action, string commandText, CommandType commandType, int? commandTimeout, IEnumerable<TParameter>? parameters)
    {
        bool closeConnection = false;

        TConnection? _connection = this.GetConnection();
        using TCommand command = this.CreateCommand(_connection, commandText, commandType, commandTimeout, parameters);

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

    private async Task<T> ExecuteAsync<T>(Func<TCommand, Task<T>> action, string commandText, CommandType commandType, int? commandTimeout, IEnumerable<TParameter>? parameters = default)
    {
        bool closeConnection = false;

        TConnection? _connection = this.GetConnection();
        using TCommand command = this.CreateCommand(_connection, commandText, commandType, commandTimeout, parameters);

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
                _connection.Close();
                this.DisposeConnection(_connection);
            }
        }
    }
}

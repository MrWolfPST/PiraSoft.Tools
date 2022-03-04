using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;

namespace PiraSoft.Tools.Data;

public abstract class ConnectionManagerBase<TConnection, TDataReader, TDataAdapter, TCommand, TParameter>
    : ConnectionManagerImplementation<TConnection, TDataReader, TDataAdapter, TCommand, TParameter>
        where TConnection : DbConnection
        where TDataReader : DbDataReader
        where TDataAdapter : DbDataAdapter
        where TCommand : DbCommand
        where TParameter : DbParameter
{
    private readonly string _connectionString;

    protected ConnectionManagerBase(string connectionString, ILogger logger, DbProviderFactory factory)
        : base(logger, factory)
        => _connectionString = connectionString;

    protected internal sealed override TConnection GetConnection()
        => this.GetConnection(_connectionString);

    protected virtual TConnection GetConnection(string connectionstring)
    {
        TConnection? retVal = (TConnection?)this.Factory.CreateConnection() ?? throw new ApplicationException($"Unable to create an instance of {typeof(TConnection)}.");

        retVal.ConnectionString = connectionstring;

        return retVal;
    }

    public TransactionalConnectionManager<TConnection, TDataReader, TDataAdapter, TCommand, TParameter> BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        => new TransactionalConnectionManager<TConnection, TDataReader, TDataAdapter, TCommand, TParameter>(this, isolationLevel, this.Logger);

    public ScopedConnectionManager<TConnection, TDataReader, TDataAdapter, TCommand, TParameter> OpenConnection()
        => new ScopedConnectionManager<TConnection, TDataReader, TDataAdapter, TCommand, TParameter>(this, this.Logger);
}

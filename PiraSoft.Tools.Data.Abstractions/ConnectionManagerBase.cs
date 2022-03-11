using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;

namespace PiraSoft.Tools.Data;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable S2436 // Methods should not have too many parameters
#pragma warning restore IDE0079 // Remove unnecessary suppression
public abstract class ConnectionManagerBase<TConnection, TDataReader, TDataAdapter, TCommand, TParameter>
    : ConnectionManagerImplementation<TConnection, TDataReader, TDataAdapter, TCommand, TParameter>
        where TConnection : DbConnection
        where TDataReader : DbDataReader
        where TDataAdapter : DbDataAdapter
        where TCommand : DbCommand
        where TParameter : DbParameter
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning restore S2436 // Methods should not have too many parameters
#pragma warning restore IDE0079 // Remove unnecessary suppression
{
    private readonly string _connectionString;

    protected ConnectionManagerBase(string connectionString, ILogger logger, DbProviderFactory factory)
        : base(logger, factory)
        => _connectionString = connectionString;

    protected internal sealed override TConnection GetConnection()
        => this.GetConnection(_connectionString);

    protected virtual TConnection GetConnection(string connectionstring)
    {
        var retVal = this.Factory.CreateConnection() as TConnection ?? throw new ApplicationException($"Unable to create an instance of {typeof(TConnection)}.");

        retVal.ConnectionString = connectionstring;

        return retVal;
    }

    public TransactionConnectionManager<TConnection, TDataReader, TDataAdapter, TCommand, TParameter> BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        => new TransactionConnectionManager<TConnection, TDataReader, TDataAdapter, TCommand, TParameter>(this, isolationLevel, this.Logger);

    public SessionConnectionManager<TConnection, TDataReader, TDataAdapter, TCommand, TParameter> BeginSession()
        => new SessionConnectionManager<TConnection, TDataReader, TDataAdapter, TCommand, TParameter>(this, this.Logger);
}

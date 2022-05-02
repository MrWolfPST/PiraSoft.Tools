using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;

namespace PiraSoft.Tools.Data;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable S2436 // Methods should not have too many parameters
#pragma warning restore IDE0079 // Remove unnecessary suppression
/// <summary>
/// The abstract base class for provider specific database interaction. Every interaction open a new connection.
/// </summary>
/// <typeparam name="TConnection">The type of connection.</typeparam>
/// <typeparam name="TDataReader">The type of data reader.</typeparam>
/// <typeparam name="TDataAdapter">The type of data adapter</typeparam>
/// <typeparam name="TCommand">The type of command</typeparam>
/// <typeparam name="TParameter">The type of parameters.</typeparam>
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

    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionManagerBase{TConnection, TDataReader, TDataAdapter, TCommand, TParameter}"/> class.
    /// </summary>
    /// <param name="connectionString">The connection used to connect the database.</param>
    /// <param name="logger">An instance of <see cref="ILogger"/>.</param>
    /// <param name="factory">An instance of <see cref="DbProviderFactory"/> used for creating instance of a provider's implementation of the data source classes.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="logger"/> is null.
    /// <paramref name="factory"/> is null.
    /// </exception>
    protected ConnectionManagerBase(string connectionString, ILogger logger, DbProviderFactory factory)
        : base(logger, factory)
        => _connectionString = connectionString;

    /// <summary>
    /// Returns a new connection to database.
    /// </summary>
    /// <returns>Connection used for database interaction.</returns>
    protected internal sealed override TConnection GetConnection()
        => this.GetConnection(_connectionString);

    /// <summary>
    /// Returns a new connection to database.
    /// </summary>
    /// <param name="connectionstring">The connection used to connect the database.</param>
    /// <returns>Connection used for database interaction.</returns>
    /// <exception cref="ApplicationException">
    /// Type of connection object returned from <see cref="ConnectionManagerImplementation{TConnection, TDataReader, TDataAdapter, TCommand, TParameter}.Factory"/> is not of <typeparamref name="TConnection"/> type.
    /// </exception>
    protected virtual TConnection GetConnection(string connectionstring)
    {
        var retVal = this.Factory.CreateConnection() as TConnection ?? throw new ApplicationException($"Unable to create an instance of {typeof(TConnection)}.");

        retVal.ConnectionString = connectionstring;

        return retVal;
    }

    /// <summary>
    /// Begin a new transactional database interation.
    /// </summary>
    /// <param name="isolationLevel">One of the enumeration values that specifies the isolation level for the transaction to use.</param>
    /// <returns>A new instance of <see cref="TransactionConnectionManager{TConnection, TDataReader, TDataAdapter, TCommand, TParameter}"/>.</returns>
    public TransactionConnectionManager<TConnection, TDataReader, TDataAdapter, TCommand, TParameter> BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        => new TransactionConnectionManager<TConnection, TDataReader, TDataAdapter, TCommand, TParameter>(this, isolationLevel, this.Logger);

    /// <summary>
    /// Begin a new database interaction on the same connection.
    /// </summary>
    /// <returns>A new instance of <see cref="SessionConnectionManager{TConnection, TDataReader, TDataAdapter, TCommand, TParameter}"/>.</returns>
    public SessionConnectionManager<TConnection, TDataReader, TDataAdapter, TCommand, TParameter> BeginSession()
        => new SessionConnectionManager<TConnection, TDataReader, TDataAdapter, TCommand, TParameter>(this, this.Logger);
}

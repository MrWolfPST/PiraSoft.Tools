using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;

namespace PiraSoft.Tools.Data;

/// <summary>
/// A  transactional database interation.
/// </summary>
/// <typeparam name="TConnection">The type of connection.</typeparam>
/// <typeparam name="TDataReader">The type of data reader.</typeparam>
/// <typeparam name="TDataAdapter">The type of data adapter</typeparam>
/// <typeparam name="TCommand">The type of command</typeparam>
/// <typeparam name="TParameter">The type of parameters.</typeparam>
public sealed class TransactionConnectionManager<TConnection, TDataReader, TDataAdapter, TCommand, TParameter>
    : ConnectionManagerImplementation<TConnection, TDataReader, TDataAdapter, TCommand, TParameter>, IDisposable
        where TConnection : DbConnection
        where TDataReader : DbDataReader
        where TDataAdapter : DbDataAdapter
        where TCommand : DbCommand
        where TParameter : DbParameter
{
    private readonly TConnection _connection;
    private readonly DbTransaction _transaction;
    private bool _disposedValue;

    internal TransactionConnectionManager(ConnectionManagerBase<TConnection, TDataReader, TDataAdapter, TCommand, TParameter> connectionManager, IsolationLevel isolationLevel, ILogger logger)
        : base(logger, connectionManager.Factory)
    {
        _connection = connectionManager.GetConnection();
        _connection.Open();
        _transaction = _connection.BeginTransaction(isolationLevel);
    }

    /// <summary>
    /// Commits the database transaction.
    /// </summary>
    public void Commit()
    {
        this.CheckTransaction();

        _transaction.Commit();

        this.Cleanup();
    }

    /// <summary>
    /// Rolls back a transaction from a pending state.
    /// </summary>
    public void Rollback()
    {
        this.CheckTransaction();

        _transaction.Rollback();

        this.Cleanup();
    }

    /// <summary>
    /// Returns the connection used by transaction.
    /// </summary>
    /// <returns>The connection used by transaction.</returns>
    /// <exception cref="ObjectDisposedException">
    /// The transaction and the connection are closed and the object is disposed.
    /// </exception>
    protected internal sealed override TConnection GetConnection()
    {
        if (_disposedValue)
        {
            throw new ObjectDisposedException(nameof(TransactionConnectionManager<TConnection, TDataReader, TDataAdapter, TCommand, TParameter>));
        }

        return _connection;
    }

    /// <summary>
    /// No action performed.
    /// </summary>
    /// <param name="connection">The connection to dispose.</param>
    protected override void DisposeConnection(TConnection connection)
    {
        //This class manage connection internally
    }

    private void Cleanup()
    {
        _connection.Close();

        _transaction.Dispose();
        _connection.Dispose();

        _disposedValue = true;
    }

    private void CheckTransaction()
    {
        if (_disposedValue)
        {
            throw new ObjectDisposedException(nameof(TransactionConnectionManager<TConnection, TDataReader, TDataAdapter, TCommand, TParameter>));
        }
    }

    #region Dispose Pattern

    private void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing && _transaction != null)
            {
                this.Rollback();
            }

            _disposedValue = true;
        }
    }

    /// <summary>
    /// Releases used resources.
    /// </summary>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    #endregion
}

using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.Common;

namespace PiraSoft.Tools.Data;

public sealed class TransactionalConnectionManager<TConnection, TDataReader, TDataAdapter, TCommand, TParameter>
    : ConnectionManagerImplementation<TConnection, TDataReader, TDataAdapter, TCommand, TParameter>, IDisposable
        where TConnection : DbConnection
        where TDataReader : DbDataReader
        where TDataAdapter : DbDataAdapter
        where TCommand : DbCommand
        where TParameter : DbParameter
{
    private readonly TConnection _connection;
    private DbTransaction _transaction;
    private bool _disposedValue;

    internal TransactionalConnectionManager(ConnectionManagerBase<TConnection, TDataReader, TDataAdapter, TCommand, TParameter> connectionManager, IsolationLevel isolationLevel, ILogger logger)
        : base(logger, connectionManager.Factory)
    {
        _connection = connectionManager.GetConnection();
        _connection.Open();
        _transaction = _connection.BeginTransaction(isolationLevel);
    }

    public void Commit()
    {
        this.CheckTransaction();

        _transaction.Commit();

        this.Cleanup();
    }

    public void Rollback()
    {
        this.CheckTransaction();

        _transaction.Rollback();

        this.Cleanup();
    }

    protected internal override TConnection GetConnection()
        => _connection;

    protected override void DisposeConnection(TConnection connection)
    {
        //This class manage connection internally
    }

    private void Cleanup()
    {
        _connection.Close();

        _transaction.Dispose();
        _connection.Dispose();

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        _transaction = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }

    private void CheckTransaction()
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("Transaction is not longer available.");
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

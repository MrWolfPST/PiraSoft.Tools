using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace PiraSoft.Tools.Data;

public sealed class ScopedConnectionManager<TConnection, TDataReader, TDataAdapter, TCommand, TParameter>
    : ConnectionManagerImplementation<TConnection, TDataReader, TDataAdapter, TCommand, TParameter>, IDisposable
        where TConnection : DbConnection
        where TDataReader : DbDataReader
        where TDataAdapter : DbDataAdapter
        where TCommand : DbCommand
        where TParameter : DbParameter
{
    private readonly TConnection _connection;
    private bool _disposedValue;

    internal ScopedConnectionManager(ConnectionManagerBase<TConnection, TDataReader, TDataAdapter, TCommand, TParameter> connectionManager, ILogger logger)
        : base(logger, connectionManager.Factory)
    {
        _connection = connectionManager.GetConnection();
        _connection.Open();
    }

    protected internal override TConnection GetConnection()
        => _connection;

    protected override void DisposeConnection(TConnection connection)
    {
        //This class manage connection internally
    }

    public void Close()
    {
        _connection.Close();
        _connection.Dispose();
    }

    #region Dispose Pattern

    private void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                this.Close();
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

using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace PiraSoft.Tools.Data;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable S2436 // Methods should not have too many parameters
#pragma warning restore IDE0079 // Remove unnecessary suppression
public sealed class SessionConnectionManager<TConnection, TDataReader, TDataAdapter, TCommand, TParameter>
    : ConnectionManagerImplementation<TConnection, TDataReader, TDataAdapter, TCommand, TParameter>, IDisposable
        where TConnection : DbConnection
        where TDataReader : DbDataReader
        where TDataAdapter : DbDataAdapter
        where TCommand : DbCommand
        where TParameter : DbParameter
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning restore S2436 // Methods should not have too many parameters
#pragma warning restore IDE0079 // Remove unnecessary suppression
{
    private readonly TConnection _connection;
    private bool _disposedValue;

    internal SessionConnectionManager(ConnectionManagerBase<TConnection, TDataReader, TDataAdapter, TCommand, TParameter> connectionManager, ILogger logger)
        : base(logger, connectionManager.Factory)
    {
        _connection = connectionManager.GetConnection();
        _connection.Open();
    }

    protected internal override TConnection GetConnection()
    {
        if (_disposedValue)
            throw new ObjectDisposedException(nameof(SessionConnectionManager<TConnection, TDataReader, TDataAdapter, TCommand, TParameter>));

        return _connection;
    }

    protected override void DisposeConnection(TConnection connection)
    {
        //This class manage connection internally
    }

    public void Close()
    {
        _connection.Close();
        _connection.Dispose();
        _disposedValue = true;
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

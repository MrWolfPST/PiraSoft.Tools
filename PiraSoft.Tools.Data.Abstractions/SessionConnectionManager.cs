using Microsoft.Extensions.Logging;
using System.Data.Common;

namespace PiraSoft.Tools.Data;

/// <summary>
/// A database interaction on the same connection.
/// </summary>
/// <typeparam name="TConnection">The type of connection.</typeparam>
/// <typeparam name="TDataReader">The type of data reader.</typeparam>
/// <typeparam name="TDataAdapter">The type of data adapter</typeparam>
/// <typeparam name="TCommand">The type of command</typeparam>
/// <typeparam name="TParameter">The type of parameters.</typeparam>
public sealed class SessionConnectionManager<TConnection, TDataReader, TDataAdapter, TCommand, TParameter>
    : ConnectionManagerImplementation<TConnection, TDataReader, TDataAdapter, TCommand, TParameter>, IDisposable
        where TConnection : DbConnection
        where TDataReader : DbDataReader
        where TDataAdapter : DbDataAdapter
        where TCommand : DbCommand
        where TParameter : DbParameter
{
    private readonly TConnection _connection;
    private bool _disposedValue;

    internal SessionConnectionManager(ConnectionManagerBase<TConnection, TDataReader, TDataAdapter, TCommand, TParameter> connectionManager, ILogger logger)
        : base(logger, connectionManager.Factory)
    {
        _connection = connectionManager.GetConnection();
        _connection.Open();
    }

    /// <summary>
    /// Returns the connection.
    /// </summary>
    /// <returns>The connection.</returns>
    /// <exception cref="ObjectDisposedException">
    /// The connection is closed and the object is disposed.
    /// </exception>
    protected internal sealed override TConnection GetConnection()
    {
        if (_disposedValue)
        {
            throw new ObjectDisposedException(nameof(SessionConnectionManager<TConnection, TDataReader, TDataAdapter, TCommand, TParameter>));
        }

        return _connection;
    }

    /// <summary>
    /// No action performed.
    /// </summary>
    /// <param name="connection">The connection to dispose.</param>
    protected override sealed void DisposeConnection(TConnection connection)
    {
        //This class manage connection internally
    }

    /// <summary>
    /// Close the connection.
    /// </summary>
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

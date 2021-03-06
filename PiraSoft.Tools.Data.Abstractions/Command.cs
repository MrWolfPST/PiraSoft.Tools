using System.Data;
using System.Data.Common;

namespace PiraSoft.Tools.Data;

public class Command<TDataReader, TParameter>
        where TDataReader : DbDataReader
        where TParameter : DbParameter
{
    private readonly IConnectionManager<TDataReader, TParameter> _connectionManager;
    private readonly string _commandText;
    private readonly CommandType _commandType;
    private readonly IList<TParameter> _parameters;
    private readonly IList<(TParameter, Action<object?>)> _outputParameters;
    private int? _commandTimeout;
    private Action<DataSet>? _configurationDelegate;

    internal Command(IConnectionManager<TDataReader, TParameter> connectionManager, string commandText, CommandType commandType)
    {
        _connectionManager = connectionManager;
        _commandText = commandText;
        _commandType = commandType;
        _parameters = new List<TParameter>();
        _outputParameters = new List<(TParameter, Action<object?>)>();
    }

    public string ReturnParameterName => "@return";

    #region Fluent

    /// <summary>
    /// Sets the wait time before terminating the attempt to execute a command and generating an error. Value must be greater than 0.
    /// </summary>
    /// <param name="commandTimeout">The time in seconds to wait for the command to execute.</param>
    /// <returns>The same instance of the <see cref="Command{TConnection, TDataReader, TDataAdapter, TCommand, TParameter}"/> for chaining.</returns>
    public Command<TDataReader, TParameter> WithTimeOut(int commandTimeout)
    {
        if (commandTimeout < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(commandTimeout), "Value must be greater than 0.");
        }

        _commandTimeout = commandTimeout;
        return this;
    }

    public Command<TDataReader, TParameter> WithParameter(string parameterName, object? value, bool dbNullIfNull = true)
    {
        this.AddParameter(parameterName, dbNullIfNull ? value ?? DBNull.Value : value);

        return this;
    }

    public Command<TDataReader, TParameter> WithOutputParameter(string parameterName, Action<object?> returnDelegate)
    {
        _outputParameters.Add((this.AddParameter(parameterName, DBNull.Value, false, ParameterDirection.Output), returnDelegate));

        return this;
    }

    public Command<TDataReader, TParameter> WithOutputParameter(string parameterName, Action<object?> returnDelegate, object? value)
    {
        _outputParameters.Add((this.AddParameter(parameterName, value, true, ParameterDirection.InputOutput), returnDelegate));

        return this;
    }

    public Command<TDataReader, TParameter> WithReturn(Action<object?> returnDelegate)
    {
        _outputParameters.Add((this.AddParameter(this.ReturnParameterName, DBNull.Value, false, ParameterDirection.ReturnValue), returnDelegate));

        return this;
    }

    public Command<TDataReader, TParameter> WithParameters(params (string, object?)[] parameters)
        => this.WithParameters(true, parameters);

    public Command<TDataReader, TParameter> WithParameters(bool dbNullIfNull, params (string, object?)[] parameters)
        => this.WithParameters(parameters, dbNullIfNull);

    public Command<TDataReader, TParameter> WithParameters(IEnumerable<(string, object?)> parameters, bool dbNullIfNull = true)
    {
        parameters.ForEach(i => this.WithParameter(i.Item1, i.Item2, dbNullIfNull));

        return this;
    }

    public Command<TDataReader, TParameter> WithConfiguration(Action<DataSet> configurationDelegate)
    {
        _configurationDelegate = configurationDelegate;

        return this;
    }

    private TParameter AddParameter(string parameterName, object? value, bool dbNullIfNull = false, ParameterDirection direction = ParameterDirection.Input)
    {
        var parameter = _connectionManager.Factory.CreateParameter() as TParameter ?? throw new ApplicationException($"Unable to create an instance of {typeof(TParameter)}.");
        parameter.ParameterName = parameterName;
        parameter.Value = dbNullIfNull ? value ?? DBNull.Value : value;
        parameter.Direction = direction;

        _parameters.Add(parameter);

        return parameter;
    }

    #endregion

    #region Execution

    public int ExecuteNonQuery()
    {
        var retVal = _connectionManager.ExecuteNonQuery(_commandText, _commandType, _commandTimeout, _parameters);

        this.RetrieveOutputValues();

        return retVal;
    }

    public T? ExecuteScalar<T>()
    {
        var retVal = _connectionManager.ExecuteScalar<T>(_commandText, _commandType, _commandTimeout, _parameters);

        this.RetrieveOutputValues();

        return retVal;
    }

    public TDataReader ExecuteReader(CommandBehavior behavior = CommandBehavior.Default)
    {
        var retVal = _connectionManager.ExecuteReader(_commandText, _commandType, _commandTimeout, behavior, _parameters);

        this.RetrieveOutputValues();

        return retVal;
    }

    public DataSet GetDataSet()
    {
        var retVal = _connectionManager.GetDataSet(_commandText, _commandType, _commandTimeout, _parameters);

        _configurationDelegate?.Invoke(retVal);

        this.RetrieveOutputValues();

        return retVal;
    }

    public DataTable GetDataTable()
    {
        var retVal = _connectionManager.GetDataTable(_commandText, _commandType, _commandTimeout, _parameters);

        if (retVal.DataSet != null)
        {
            _configurationDelegate?.Invoke(retVal.DataSet);
        }

        this.RetrieveOutputValues();

        return retVal;
    }

    public DataRow? GetDataRow()
    {
        var retVal = _connectionManager.GetDataRow(_commandText, _commandType, _commandTimeout, _parameters);

        if (retVal?.Table?.DataSet != null)
        {
            _configurationDelegate?.Invoke(retVal.Table.DataSet);
        }

        this.RetrieveOutputValues();

        return retVal;
    }

    public async Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken = default)
    {
        var retVal = await _connectionManager.ExecuteNonQueryAsync(_commandText, _commandType, _commandTimeout, _parameters, cancellationToken);

        this.RetrieveOutputValues();

        return retVal;
    }

    public async Task<T?> ExecuteScalarAsync<T>(CancellationToken cancellationToken = default)
    {
        var retVal = await _connectionManager.ExecuteScalarAsync<T>(_commandText, _commandType, _commandTimeout, _parameters, cancellationToken);

        this.RetrieveOutputValues();

        return retVal;
    }

    public async Task<TDataReader> ExecuteReaderAsync(CommandBehavior behavior = CommandBehavior.Default, CancellationToken cancellationToken = default)
    {
        var retVal = await _connectionManager.ExecuteReaderAsync(_commandText, _commandType, _commandTimeout, behavior, _parameters, cancellationToken);

        this.RetrieveOutputValues();

        return retVal;
    }

    public async Task<DataSet> GetDataSetAsync(CancellationToken cancellationToken = default)
    {
        var retVal = await _connectionManager.GetDataSetAsync(_commandText, _commandType, _commandTimeout, _parameters, cancellationToken);

        _configurationDelegate?.Invoke(retVal);

        this.RetrieveOutputValues();

        return retVal;
    }

    public async Task<DataTable> GetDataTableAsync(CancellationToken cancellationToken = default)
    {
        var retVal = await _connectionManager.GetDataTableAsync(_commandText, _commandType, _commandTimeout, _parameters, cancellationToken);

        if (retVal.DataSet != null)
        {
            _configurationDelegate?.Invoke(retVal.DataSet);
        }

        this.RetrieveOutputValues();

        return retVal;
    }

    public async Task<DataRow?> GetDataRowAsync(CancellationToken cancellationToken = default)
    {
        var retVal = await _connectionManager.GetDataRowAsync(_commandText, _commandType, _commandTimeout, _parameters, cancellationToken);

        if (retVal?.Table?.DataSet != null)
        {
            _configurationDelegate?.Invoke(retVal.Table.DataSet);
        }

        this.RetrieveOutputValues();

        return retVal;
    }

    private void RetrieveOutputValues()
        => _outputParameters.ForEach(i => i.Item2(i.Item1.Value));

    #endregion
}

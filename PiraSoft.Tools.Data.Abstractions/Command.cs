using System.Data;
using System.Data.Common;

namespace PiraSoft.Tools.Data;

/// <summary>
/// Provides the class to build and execute against a database a statement or stored procedure.
/// </summary>
/// <typeparam name="TDataReader">The type of data reader.</typeparam>
/// <typeparam name="TParameter">The type of parameters.</typeparam>
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

    /// <summary>
    /// Gets the name used for the return parameter
    /// </summary>
    public string ReturnParameterName => "@return";

    #region Fluent

    /// <summary>
    /// Sets the wait time before terminating the attempt to execute a command and generating an error. Value must be greater than 0.
    /// </summary>
    /// <param name="commandTimeout">The time in seconds to wait for the command to execute.</param>
    /// <returns>The same instance of the <see cref="Command{TDataReader, TDataAdapter}"/> for chaining.</returns>
    public Command<TDataReader, TParameter> WithTimeOut(int commandTimeout)
    {
        if (commandTimeout < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(commandTimeout), "Value must be greater than 0.");
        }

        _commandTimeout = commandTimeout;
        return this;
    }

    /// <summary>
    /// Add an input parameter to the command.
    /// </summary>
    /// <param name="parameterName">Name of parameter.</param>
    /// <param name="value">Value of paramenter.</param>
    /// <param name="dbNullIfNull">Defines if null value must be converted to <see cref="DBNull.Value"/>. A parameter with null value is ignored. Default value is true.</param>
    /// <returns>The same instance of the <see cref="Command{TDataReader, TDataAdapter}"/> for chaining.</returns>
    public Command<TDataReader, TParameter> WithParameter(string parameterName, object? value, bool dbNullIfNull = true)
        => this.ExecuteAndReturn(t => t.AddParameter(parameterName, dbNullIfNull ? value ?? DBNull.Value : value));

    /// <summary>
    /// Add an output parameter to the command.
    /// </summary>
    /// <param name="parameterName">Name of parameter.</param>
    /// <param name="returnDelegate">Delegate for retrieve returned value. Returned value is passed as argument to the delegate.</param>
    /// <returns>The same instance of the <see cref="Command{TDataReader, TDataAdapter}"/> for chaining.</returns>
    public Command<TDataReader, TParameter> WithOutputParameter(string parameterName, Action<object?> returnDelegate)
        => this.ExecuteAndReturn(t => t._outputParameters.Add((t.AddParameter(parameterName, DBNull.Value, false, ParameterDirection.Output), returnDelegate)));

    /// <summary>
    /// Add an input/output parameter to the command.
    /// </summary>
    /// <param name="parameterName">Name of parameter.</param>
    /// <param name="returnDelegate">Delegate for retrieve returned value. Returned value is passed as argument to the delegate.</param>
    /// <param name="value">Value of paramenter. Null value is converted to <see cref="DBNull.Value"/>.</param>
    /// <returns>The same instance of the <see cref="Command{TDataReader, TDataAdapter}"/> for chaining.</returns>
    public Command<TDataReader, TParameter> WithOutputParameter(string parameterName, Action<object?> returnDelegate, object? value)
        => this.ExecuteAndReturn(t => t._outputParameters.Add((t.AddParameter(parameterName, value, true, ParameterDirection.InputOutput), returnDelegate)));

    /// <summary>
    /// Add return parameter to the command.
    /// </summary>
    /// <param name="returnDelegate">Delegate for retrieve returned value. Returned value is passed as argument to the delegate.</param>
    /// <returns>The same instance of the <see cref="Command{TDataReader, TDataAdapter}"/> for chaining.</returns>
    public Command<TDataReader, TParameter> WithReturn(Action<object?> returnDelegate)
        => this.ExecuteAndReturn(t => t._outputParameters.Add((t.AddParameter(t.ReturnParameterName, DBNull.Value, false, ParameterDirection.ReturnValue), returnDelegate)));

    /// <summary>
    /// Add a list of parameters to che command.
    /// </summary>
    /// <param name="parameters">The list of tuple used to build parameters to add. First field of tuple represents the parameter name; second field represent the parameter value. Null value is converted to <see cref="DBNull.Value"/>.</param>
    /// <returns>The same instance of the <see cref="Command{TDataReader, TDataAdapter}"/> for chaining.</returns>
    public Command<TDataReader, TParameter> WithParameters(params (string Name, object? Value)[] parameters)
        => this.WithParameters(true, parameters);

    /// <summary>
    /// Add a list of parameters to che command.
    /// </summary>
    /// <param name="dbNullIfNull">Defines if null value must be converted to <see cref="DBNull.Value"/>. A parameter with null value is ignored. Default value is true.</param>
    /// <param name="parameters">The list of tuple used to build parameters to add. First field of tuple represents the parameter name; second field represent the parameter value.</param>
    /// <returns>The same instance of the <see cref="Command{TDataReader, TDataAdapter}"/> for chaining.</returns>
    public Command<TDataReader, TParameter> WithParameters(bool dbNullIfNull, params (string Name, object? Value)[] parameters)
        => this.WithParameters(parameters, dbNullIfNull);

    /// <summary>
    /// Add a list of parameters to che command.
    /// </summary>
    /// <param name="parameters">The list of tuple used to build parameters to add. First field of tuple represents the parameter name; second field represent the parameter value.</param>
    /// <param name="dbNullIfNull">Defines if null value must be converted to <see cref="DBNull.Value"/>. A parameter with null value is ignored. Default value is true.</param>
    /// <returns>The same instance of the <see cref="Command{TDataReader, TDataAdapter}"/> for chaining.</returns>
    public Command<TDataReader, TParameter> WithParameters(IEnumerable<(string Name, object? Value)> parameters, bool dbNullIfNull = true)
        => this.ExecuteAndReturn(t => parameters.ForEach(i => t.WithParameter(i.Item1, i.Item2, dbNullIfNull)));

    /// <summary>
    /// Sets an action to perform on <see cref="DataSet"/> after data loading.
    /// </summary>
    /// <param name="configurationDelegate"></param>
    /// <remarks>
    /// Action is invoked only by:
    /// <list type="bullet">
    /// <item><see cref="Command{TDataReader, TParameter}.GetDataSet"/></item>
    /// <item><see cref="Command{TDataReader, TParameter}.GetDataTable"/></item>
    /// <item><see cref="Command{TDataReader, TParameter}.GetDataRow"/></item>
    /// <item><see cref="Command{TDataReader, TParameter}.GetDataSetAsync(CancellationToken)"/></item>
    /// <item><see cref="Command{TDataReader, TParameter}.GetDataTableAsync(CancellationToken)"/></item>
    /// <item><see cref="Command{TDataReader, TParameter}.GetDataRowAsync(CancellationToken)"/></item>
    /// </list>
    /// </remarks>
    /// <returns>The same instance of the <see cref="Command{TDataReader, TDataAdapter}"/> for chaining.</returns>
    public Command<TDataReader, TParameter> WithConfiguration(Action<DataSet> configurationDelegate)
        => this.ExecuteAndReturn(t=> t._configurationDelegate = configurationDelegate);

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

    /// <summary>
    /// Executes the SQL statement represented by the <see cref="Command{TDataReader, TParameter}"/> object.
    /// </summary>
    /// <returns>The number of rows affected.</returns>
    public int ExecuteNonQuery()
    {
        var retVal = _connectionManager.ExecuteNonQuery(_commandText, _commandType, _commandTimeout, _parameters);

        this.RetrieveOutputValues();

        return retVal;
    }

    /// <summary>
    /// Executes the SQL statement represented by the <see cref="Command{TDataReader, TParameter}"/> object and returns the first column of the first row in the result set returned by the query. All other columns and rows are ignored. 
    /// </summary>
    /// <typeparam name="T">Type of returned value.</typeparam>
    /// <returns>The casted value of first column of the first row in the result set returned by the query.</returns>
    public T? ExecuteScalar<T>()
    {
        var retVal = _connectionManager.ExecuteScalar<T>(_commandText, _commandType, _commandTimeout, _parameters);

        this.RetrieveOutputValues();

        return retVal;
    }

    /// <summary>
    /// Executes the SQL statement represented by the <see cref="Command{TDataReader, TParameter}"/> object and returns an <see cref="DbDataReader"/>.
    /// </summary>
    /// <param name="behavior">One of the enumeration values that specifies the command behavior.</param>
    /// <returns>An <see cref="DbDataReader"/> object.</returns>
    public TDataReader ExecuteReader(CommandBehavior behavior = CommandBehavior.Default)
    {
        var retVal = _connectionManager.ExecuteReader(_commandText, _commandType, _commandTimeout, behavior, _parameters);

        this.RetrieveOutputValues();

        return retVal;
    }

    /// <summary>
    /// Executes the SQL statement represented by the <see cref="Command{TDataReader, TParameter}"/> object and returns an <see cref="DataSet"/>.
    /// </summary>
    /// <returns>An <see cref="DataSet"/> object.</returns>
    public DataSet GetDataSet()
    {
        var retVal = _connectionManager.GetDataSet(_commandText, _commandType, _commandTimeout, _parameters);

        _configurationDelegate?.Invoke(retVal);

        this.RetrieveOutputValues();

        return retVal;
    }

    /// <summary>
    /// Executes the SQL statement represented by the <see cref="Command{TDataReader, TParameter}"/> object and returns an <see cref="DataTable"/>.
    /// </summary>
    /// <returns>An <see cref="DataTable"/> object.</returns>
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

    /// <summary>
    /// Executes the SQL statement represented by the <see cref="Command{TDataReader, TParameter}"/> object and returns an <see cref="DataRow"/>.
    /// </summary>
    /// <returns>An <see cref="DataRow"/> object.</returns>
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

    /// <summary>
    /// This is the asynchronous version of <see cref="Command{TDataReader, TParameter}.ExecuteNonQuery"/>.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken = default)
    {
        var retVal = await _connectionManager.ExecuteNonQueryAsync(_commandText, _commandType, _commandTimeout, _parameters, cancellationToken);

        this.RetrieveOutputValues();

        return retVal;
    }

    /// <summary>
    /// This is the asynchronous version of <see cref="Command{TDataReader, TParameter}.ExecuteScalar{T}"/>.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<T?> ExecuteScalarAsync<T>(CancellationToken cancellationToken = default)
    {
        var retVal = await _connectionManager.ExecuteScalarAsync<T>(_commandText, _commandType, _commandTimeout, _parameters, cancellationToken);

        this.RetrieveOutputValues();

        return retVal;
    }

    /// <summary>
    /// This is the asynchronous version of <see cref="Command{TDataReader, TParameter}.ExecuteReader(CommandBehavior)"/>.
    /// </summary>
    /// <param name="behavior">One of the enumeration values that specifies the command behavior.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<TDataReader> ExecuteReaderAsync(CommandBehavior behavior = CommandBehavior.Default, CancellationToken cancellationToken = default)
    {
        var retVal = await _connectionManager.ExecuteReaderAsync(_commandText, _commandType, _commandTimeout, behavior, _parameters, cancellationToken);

        this.RetrieveOutputValues();

        return retVal;
    }

    /// <summary>
    /// This is the asynchronous version of <see cref="Command{TDataReader, TParameter}.GetDataSet"/>.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<DataSet> GetDataSetAsync(CancellationToken cancellationToken = default)
    {
        var retVal = await _connectionManager.GetDataSetAsync(_commandText, _commandType, _commandTimeout, _parameters, cancellationToken);

        _configurationDelegate?.Invoke(retVal);

        this.RetrieveOutputValues();

        return retVal;
    }

    /// <summary>
    /// This is the asynchronous version of <see cref="Command{TDataReader, TParameter}.GetDataTable"/>.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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

    /// <summary>
    /// This is the asynchronous version of <see cref="Command{TDataReader, TParameter}.GetDataRow"/>.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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

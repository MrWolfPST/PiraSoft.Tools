using System.Data;
using System.Data.Common;

namespace PiraSoft.Tools.Data;

public interface IConnectionManager<TDataReader, TParameter>
        where TDataReader : DbDataReader
        where TParameter : DbParameter
{
    DbProviderFactory Factory { get; }

    int ExecuteNonQuery(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default);

    T? ExecuteScalar<T>(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default);

    TDataReader ExecuteReader(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CommandBehavior behavior = CommandBehavior.Default, IEnumerable<TParameter>? parameters = default);

    DataSet GetDataSet(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default);

    DataTable GetDataTable(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default);

    DataRow? GetDataRow(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default);

    Task<int> ExecuteNonQueryAsync(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default);

    Task<T?> ExecuteScalarAsync<T>(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default);

    Task<TDataReader> ExecuteReaderAsync(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, CommandBehavior behavior = CommandBehavior.Default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default);

    Task<DataSet> GetDataSetAsync(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default);

    Task<DataTable> GetDataTableAsync(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default);

    Task<DataRow?> GetDataRowAsync(string commandText, CommandType commandType = CommandType.StoredProcedure, int? commandTimeout = default, IEnumerable<TParameter>? parameters = default, CancellationToken cancellationToken = default);
}

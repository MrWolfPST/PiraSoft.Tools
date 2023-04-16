using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace PiraSoft.Tools.Data.SqlClient;

/// <summary>
/// The class for Sql Server database interaction. Every interaction open a new connection.
/// </summary>
public class SqlConnectionManager
     : ConnectionManagerBase<SqlConnection, SqlDataReader, SqlDataAdapter, SqlCommand, SqlParameter>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SqlConnectionManager"/> class.
    /// </summary>
    /// <param name="connectionString">The connection used to connect the database.</param>
    /// <param name="logger">An instance of <see cref="ILogger"/>.</param>
    public SqlConnectionManager(string connectionString, ILogger<SqlConnectionManager> logger)
        : base(connectionString, logger, SqlClientFactory.Instance)
    { }
}

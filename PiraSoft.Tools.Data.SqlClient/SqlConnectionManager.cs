using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace PiraSoft.Tools.Data.SqlClient;

public class SqlConnectionManager
     : ConnectionManagerBase<SqlConnection, SqlDataReader, SqlDataAdapter, SqlCommand, SqlParameter>
{
    public SqlConnectionManager(string connectionString, ILogger<SqlConnectionManager> logger)
        : base(connectionString, logger, SqlClientFactory.Instance)
    { }
}

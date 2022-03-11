using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PiraSoft.Tools.Data.SqlClient;
using System.Data.SqlClient;

namespace PiraSoft.Tools.UnitTest.Data;

[TestClass]
public class SqlConnectionManagerUnitTest
{
    [TestMethod]
    public void ProviderFactory()
    {
        var loggerMock = Substitute.For<ILogger<SqlConnectionManager>>();
        var connectionManager = new SqlConnectionManager("", loggerMock);

        Assert.IsInstanceOfType(connectionManager.Factory, typeof(SqlClientFactory));
    }
}
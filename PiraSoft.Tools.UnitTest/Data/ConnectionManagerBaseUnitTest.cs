using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PiraSoft.Tools.Data;
using System.Data;
using System.Data.Common;

namespace PiraSoft.Tools.UnitTest.Data;

[TestClass]
public class ConnectionManagerBaseUnitTest
{
    internal class FakeConnectionManager : ConnectionManagerBase<DbConnection, DbDataReader, DbDataAdapter, DbCommand, DbParameter>
    {
        public FakeConnectionManager(string connectionString, ILogger logger)
        : base(connectionString, logger, GetDbProviderFactoryMock())
        { }

        private static DbProviderFactory GetDbProviderFactoryMock()
        {
            var retVal = Substitute.For<DbProviderFactory>();
            var dbConnectionMock = Substitute.For<DbConnection>();
            var dbTranscationMock = Substitute.For<DbTransaction>();

#pragma warning disable NS1000 // Non-virtual setup specification.
#pragma warning disable NS1004 // Argument matcher used with a non-virtual member of a class.
            dbConnectionMock.BeginTransaction(Arg.Any<IsolationLevel>()).Returns(dbTranscationMock);
#pragma warning restore NS1004 // Argument matcher used with a non-virtual member of a class.
#pragma warning restore NS1000 // Non-virtual setup specification.
            retVal.CreateConnection().Returns(dbConnectionMock);

            return retVal;
        }
    }

    [TestMethod]
    public void TransactionConnectionManager()
    {
        var loggerMock = Substitute.For<ILogger>();
        var connectionManager = new FakeConnectionManager("", loggerMock);

        Assert.IsInstanceOfType(connectionManager.BeginTransaction(), typeof(TransactionConnectionManager<DbConnection, DbDataReader, DbDataAdapter, DbCommand, DbParameter>));
    }

    [TestMethod]
    public void ScopedConnectionManager()
    {
        var loggerMock = Substitute.For<ILogger>();
        var connectionManager = new FakeConnectionManager("", loggerMock);

        Assert.IsInstanceOfType(connectionManager.BeginSession(), typeof(SessionConnectionManager<DbConnection, DbDataReader, DbDataAdapter, DbCommand, DbParameter>));
    }
}
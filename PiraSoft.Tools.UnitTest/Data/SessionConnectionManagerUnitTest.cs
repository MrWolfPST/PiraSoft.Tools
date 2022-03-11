using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PiraSoft.Tools.Data;
using System;
using System.Data.Common;

namespace PiraSoft.Tools.UnitTest.Data;

[TestClass]
public class SessionConnectionManagerUnitTest
{
    internal class FakeConnectionManager : ConnectionManagerBase<DbConnection, DbDataReader, DbDataAdapter, DbCommand, DbParameter>
    {
        public FakeConnectionManager(string connectionString, ILogger logger)
            : base(connectionString, logger, GetDbProviderFactoryMock())
        { }

        private static DbProviderFactory GetDbProviderFactoryMock()
        {
            var retVal = Substitute.For<DbProviderFactory>();
            var commandMock = Substitute.For<DbCommand>();

#pragma warning disable NS1000 // Non-virtual setup specification.
            commandMock.Parameters.Returns(Substitute.For<DbParameterCollection>());
#pragma warning restore NS1000 // Non-virtual setup specification.

            //retVal.CreateConnection().Returns(CreateConnection);
            retVal.CreateConnection().Returns(x => Substitute.For<DbConnection>());
            retVal.CreateCommand().Returns(commandMock);

            return retVal;
        }
    }

    [TestMethod]
    public void Connection()
    {
        var loggerMock = Substitute.For<ILogger>();
        var connectionManager = new FakeConnectionManager("", loggerMock);
        var session = connectionManager.BeginSession();

        Assert.AreNotSame(connectionManager.GetConnection(), session.GetConnection());
    }

    [TestMethod]
    public void Close()
    {
        var loggerMock = Substitute.For<ILogger>();
        var connectionManager = new FakeConnectionManager("", loggerMock);
        var session = connectionManager.BeginSession();

        session.Close();

        Assert.ThrowsException<ObjectDisposedException>(() => session.ExecuteNonQuery(""));
    }

    [TestMethod]
    public void Dispose()
    {
        var loggerMock = Substitute.For<ILogger>();
        var connectionManager = new FakeConnectionManager("", loggerMock);
        var session = connectionManager.BeginSession();

        session.ExecuteNonQuery("");

        session.Dispose();

        Assert.ThrowsException<ObjectDisposedException>(() => session.ExecuteNonQuery(""));
    }
}
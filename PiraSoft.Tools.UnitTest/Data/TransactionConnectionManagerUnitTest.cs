using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Core;
using PiraSoft.Tools.Data;
using System;
using System.Data.Common;

namespace PiraSoft.Tools.UnitTest.Data;

[TestClass]
public class TransactionConnectionManagerUnitTest
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

            retVal.CreateConnection().Returns(CreateConnection);
            retVal.CreateCommand().Returns(commandMock);

            return retVal;
        }

        private static DbConnection CreateConnection(CallInfo info)
        {
            var retVal = Substitute.For<DbConnection>();

#pragma warning disable NS1000 // Non-virtual setup specification.
            retVal.BeginTransaction().Returns(Substitute.For<DbTransaction>());
#pragma warning restore NS1000 // Non-virtual setup specification.

            return retVal;
        }
    }

    [TestMethod]
    public void Connection()
    {
        var loggerMock = Substitute.For<ILogger>();
        var connectionManager = new FakeConnectionManager("", loggerMock);
        var transction = connectionManager.BeginTransaction();

        Assert.AreNotSame(connectionManager.GetConnection(), transction.GetConnection());
    }

    [TestMethod]
    public void Commit()
    {
        var loggerMock = Substitute.For<ILogger>();
        var connectionManager = new FakeConnectionManager("", loggerMock);
        var transaction = connectionManager.BeginTransaction();

        transaction.Commit();

        Assert.ThrowsException<ObjectDisposedException>(() => transaction.ExecuteNonQuery(""));
    }

    [TestMethod]
    public void Rollback()
    {
        var loggerMock = Substitute.For<ILogger>();
        var connectionManager = new FakeConnectionManager("", loggerMock);
        var transaction = connectionManager.BeginTransaction();

        transaction.Rollback();

        Assert.ThrowsException<ObjectDisposedException>(() => transaction.Commit());
    }

    [TestMethod]
    public void Dispose()
    {
        var loggerMock = Substitute.For<ILogger>();
        var connectionManager = new FakeConnectionManager("", loggerMock);
        var transaction = connectionManager.BeginTransaction();

        transaction.ExecuteNonQuery("");

        transaction.Dispose();

        Assert.ThrowsException<ObjectDisposedException>(() => transaction.ExecuteNonQuery(""));
    }
}
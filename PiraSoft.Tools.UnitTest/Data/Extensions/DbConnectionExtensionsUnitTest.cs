using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Common;
using System.Data.SqlClient;

namespace PiraSoft.Tools.UnitTest.Data.Extensions;

[TestClass]
public class DbConnectionExtensionsUnitTest
{
    [TestMethod]
    public void Dump()
    {
        var cnnStrBulder = new SqlConnectionStringBuilder() { DataSource = "ServerAddress", InitialCatalog = "DbName" };
        var target = new SqlConnection(cnnStrBulder.ConnectionString);

        Assert.AreEqual("ServerAddress.DbName", target.Dump(), "Dump simple");
    }
}

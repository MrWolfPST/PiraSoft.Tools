using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace PiraSoft.Tools.UnitTest.Data.Extensions;

[TestClass]
public class DbCommandExtensionsUnitTest
{
    [TestMethod]
    public void Dump()
    {
        var cnnStrBulder = new SqlConnectionStringBuilder() { DataSource = "ServerAddress", InitialCatalog = "DbName" };
        var cnn = new SqlConnection(cnnStrBulder.ConnectionString);
        var target = new SqlCommand("StoredProcedure", cnn) { CommandType = CommandType.StoredProcedure };

        Assert.AreEqual("ServerAddress.DbName => StoredProcedure", target.Dump(), "Dump simple");

        target.Parameters.Add(new SqlParameter() { ParameterName = "@return", Direction = ParameterDirection.ReturnValue });

        Assert.AreEqual("ServerAddress.DbName => @return = StoredProcedure", target.Dump(), "Dump with return");

        target.Parameters.AddWithValue("@parameter1", "StringValue");

        Assert.AreEqual("ServerAddress.DbName => @return = StoredProcedure @parameter1 = 'StringValue'", target.Dump(), "Dump with return and one parameter");

        target.Parameters.AddWithValue("@parameter2", 2);

        Assert.AreEqual("ServerAddress.DbName => @return = StoredProcedure @parameter1 = 'StringValue', @parameter2 = 2", target.Dump(), "Dump with return and two parameters");
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace PiraSoft.Tools.UnitTest.Data.Extensions;

[TestClass]
public class DbParameterExtensionsUnitTest
{
    [TestMethod]
    public void Dump()
    {
        var target = new SqlParameter("@parameter", null);
        Assert.AreEqual(null, target.Dump(), "Dump null");

        target = new SqlParameter("@parameter", DBNull.Value);
        Assert.AreEqual("@parameter = NULL", target.Dump(), "Dump DBNull");

        target = new SqlParameter("@parameter", CommandType.StoredProcedure);
        Assert.AreEqual("@parameter = 4 /*StoredProcedure*/", target.Dump(), "Dump Enum");

        target = new SqlParameter("@parameter", 5);
        Assert.AreEqual("@parameter = 5", target.Dump(), "Dump int");

        target = new SqlParameter("@parameter", 8.6);
        Assert.AreEqual("@parameter = 8.6", target.Dump(), "Dump decimal");

        target = new SqlParameter("@parameter", "string");
        Assert.AreEqual("@parameter = 'string'", target.Dump(), "Dump string");

        target = new SqlParameter("@parameter", new DateTime(2022, 2, 15, 15, 6, 37));
        Assert.AreEqual("@parameter = '2022-02-15 15:06:37'", target.Dump(), "Dump DateTime");

        target = new SqlParameter("@parameter", "string") { Direction = ParameterDirection.InputOutput };
        Assert.AreEqual("@parameter = 'string' output", target.Dump(), "Dump string output");

        target = new SqlParameter("@parameter", "string") { Direction = ParameterDirection.Output };
        Assert.AreEqual("@parameter = @parameter output", target.Dump(), "Dump output");

        target = new SqlParameter("@parameter", "string") { Direction = ParameterDirection.ReturnValue };
        Assert.AreEqual("@parameter = ", target.Dump(), "Dump return");
    }
}

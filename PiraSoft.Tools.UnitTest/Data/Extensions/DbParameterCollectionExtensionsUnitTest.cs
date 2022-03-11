using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace PiraSoft.Tools.UnitTest.Data.Extensions;

[TestClass]
public class DbParameterCollectionExtensionsUnitTest
{
    [TestMethod]
    public void AsEnumerable()
    {
        var cmd = new SqlCommand();

        cmd.Parameters.AddWithValue("par1", null);
        cmd.Parameters.AddWithValue("par2", null);
        cmd.Parameters.AddWithValue("par3", null);

        var target = cmd.Parameters.AsEnumerable().ToArray();

        Assert.AreEqual(cmd.Parameters.Count, target.Length);
    }
}

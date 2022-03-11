using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Linq;

namespace PiraSoft.Tools.UnitTest.Data.Extensions;

[TestClass]
public class DataRowCollectionExtensionsUnitTest
{
    [TestMethod]
    public void AsEnumerable()
    {
        var dt = new DataTable();

        dt.Columns.Add(new DataColumn("Col1"));

        dt.Rows.Add("val1");
        dt.Rows.Add("val2");
        dt.Rows.Add("val3");

        var target = dt.Rows.AsEnumerable().ToArray();

        Assert.AreEqual(dt.Rows.Count, target.Length);
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiraSoft.Tools.UnitTest.Data.Extensions;

[TestClass]
public class DataTableCollectionExtensionsUnitTest
{
    [TestMethod]
    public void AsEnumerable()
    {
        var ds = new DataSet();

        ds.Tables.Add(new DataTable());
        ds.Tables.Add(new DataTable());
        ds.Tables.Add(new DataTable());

        var target = ds.Tables.AsEnumerable().ToArray();

        Assert.AreEqual(ds.Tables.Count, target.Length);
    }
}

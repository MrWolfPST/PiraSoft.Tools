using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Threading.Tasks;

namespace PiraSoft.Tools.UnitTest.Data.Extensions;

[TestClass]
public class DataTableExtensionsUnitTest
{
    [TestMethod]
    public async Task LoadAsync()
    {
        var sourceTable = new DataTable();

        sourceTable.Columns.Add(new DataColumn("Col1", typeof(string)));
        sourceTable.Columns.Add(new DataColumn("Col2", typeof(int)));

        sourceTable.Rows.Add("Val1", 1);
        sourceTable.Rows.Add("Val2", 2);

        var targetTable = new DataTable();
        var dataReader = sourceTable.CreateDataReader();
        
        await targetTable.LoadAsync(dataReader);

        Assert.AreEqual(sourceTable.Columns.Count, targetTable.Columns.Count);
        Assert.AreEqual(sourceTable.Rows.Count, targetTable.Rows.Count);
    }
}

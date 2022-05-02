using System.Data;

namespace PiraSoft.Tools.UnitTest.Data.Mapping;

internal class TestDataTable : DataTable
{
    public TestDataTable()
    {
        this.Columns.Add(new DataColumn() { ColumnName = "StringProperty", DataType = typeof(string), AllowDBNull = false });
        this.Columns.Add(new DataColumn() { ColumnName = "AnotherStringColumn", DataType = typeof(string), AllowDBNull = true });

        var row = this.NewRow();
        row["StringProperty"] = "StringValue";
        row["AnotherStringColumn"] = "AnotherStringValue";

        this.Rows.Add(row);

        row = this.NewRow();
        row["StringProperty"] = "StringValue";
        row["AnotherStringColumn"] = "AnotherStringValue";

        this.Rows.Add(row);
    }
}

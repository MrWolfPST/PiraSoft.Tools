using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiraSoft.Tools.UnitTest.Data.Mapping;

internal class TestDataTable : DataTable
{
    public TestDataTable()
    {
        this.Columns.Add(new DataColumn() { ColumnName = "StringProperty", DataType = typeof(string), AllowDBNull = false });
        this.Columns.Add(new DataColumn() { ColumnName = "IntProperty", DataType = typeof(int), AllowDBNull = false });
        this.Columns.Add(new DataColumn() { ColumnName = "DateTimeProperty", DataType = typeof(DateTime), AllowDBNull = false });
        this.Columns.Add(new DataColumn() { ColumnName = "BooleanColumn", DataType = typeof(bool), AllowDBNull = false });
        this.Columns.Add(new DataColumn() { ColumnName = "NullableIntProperty", DataType = typeof(int), AllowDBNull = true });
        this.Columns.Add(new DataColumn() { ColumnName = "FromStringEnumeration", DataType = typeof(string), AllowDBNull = false });
        this.Columns.Add(new DataColumn() { ColumnName = "FromIntEnumeration", DataType = typeof(int), AllowDBNull = false });
        this.Columns.Add(new DataColumn() { ColumnName = "NullableLongProperty", DataType = typeof(long), AllowDBNull = true });
        this.Columns.Add(new DataColumn() { ColumnName = "DbOnlyColumn", DataType = typeof(long), AllowDBNull = true });
        this.Columns.Add(new DataColumn() { ColumnName = "AnotherStringColumn", DataType = typeof(string), AllowDBNull = true });

        var row = this.NewRow();
        row["StringProperty"] = "StringValue";
        row["IntProperty"] = 1;
        row["DateTimeProperty"] = new DateTime(2022, 03, 10, 17, 35, 41);
        row["BooleanColumn"] = true;
        row["NullableIntProperty"] = DBNull.Value;
        row["FromStringEnumeration"] = "Value1";
        row["FromIntEnumeration"] = 2;
        row["NullableLongProperty"] = DBNull.Value;
        row["DbOnlyColumn"] = DBNull.Value;
        row["AnotherStringColumn"] = "AnotherStringValue";

        this.Rows.Add(row);
    }
}

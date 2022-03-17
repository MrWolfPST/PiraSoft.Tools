using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Data;
using System.Data.Common;

namespace PiraSoft.Tools.UnitTest.Data.Extensions;

[TestClass]
public class DbDataReaderExtensionsUnitTest
{
    [TestMethod]
    public void ToDataRow()
    {
        var date = DateTime.Now;
        var dataReaderMock = Substitute.For<DbDataReader>();
        var schemaTable = new DataTable();

        schemaTable.Columns.Add(new DataColumn(SchemaTableColumn.ColumnName, typeof(string)));
        schemaTable.Columns.Add(new DataColumn(SchemaTableColumn.ColumnOrdinal, typeof(int)));
        schemaTable.Columns.Add(new DataColumn(SchemaTableColumn.ColumnSize, typeof(int)));
        schemaTable.Columns.Add(new DataColumn(SchemaTableColumn.NumericPrecision, typeof(short)));
        schemaTable.Columns.Add(new DataColumn(SchemaTableColumn.NumericScale, typeof(short)));
        schemaTable.Columns.Add(new DataColumn(SchemaTableColumn.DataType, typeof(Type)));
        schemaTable.Columns.Add(new DataColumn(SchemaTableOptionalColumn.ProviderSpecificDataType, typeof(Type)));
        schemaTable.Columns.Add(new DataColumn(SchemaTableColumn.NonVersionedProviderType, typeof(int)));
        schemaTable.Columns.Add(new DataColumn(SchemaTableColumn.ProviderType, typeof(int)));
        schemaTable.Columns.Add(new DataColumn(SchemaTableColumn.IsLong, typeof(bool)));
        schemaTable.Columns.Add(new DataColumn(SchemaTableColumn.AllowDBNull, typeof(bool)));
        schemaTable.Columns.Add(new DataColumn(SchemaTableOptionalColumn.IsReadOnly, typeof(bool)));
        schemaTable.Columns.Add(new DataColumn(SchemaTableOptionalColumn.IsRowVersion, typeof(bool)));
        schemaTable.Columns.Add(new DataColumn(SchemaTableColumn.IsUnique, typeof(bool)));
        schemaTable.Columns.Add(new DataColumn(SchemaTableColumn.IsKey, typeof(bool)));
        schemaTable.Columns.Add(new DataColumn(SchemaTableOptionalColumn.IsAutoIncrement, typeof(bool)));
        schemaTable.Columns.Add(new DataColumn(SchemaTableOptionalColumn.IsHidden, typeof(bool)));
        schemaTable.Columns.Add(new DataColumn(SchemaTableOptionalColumn.BaseCatalogName, typeof(string)));
        schemaTable.Columns.Add(new DataColumn(SchemaTableColumn.BaseSchemaName, typeof(string)));
        schemaTable.Columns.Add(new DataColumn(SchemaTableColumn.BaseTableName, typeof(string)));
        schemaTable.Columns.Add(new DataColumn(SchemaTableColumn.BaseColumnName, typeof(string)));

        var col1 = schemaTable.NewRow();
        col1[SchemaTableColumn.ColumnName] = "column1";
        col1[SchemaTableColumn.DataType] = typeof(string);
        col1[SchemaTableColumn.AllowDBNull] = true;
        schemaTable.Rows.Add(col1);

        col1 = schemaTable.NewRow();
        col1[SchemaTableColumn.ColumnName] = "column2";
        col1[SchemaTableColumn.DataType] = typeof(int);
        col1[SchemaTableColumn.AllowDBNull] = true;
        schemaTable.Rows.Add(col1);

        col1 = schemaTable.NewRow();
        col1[SchemaTableColumn.ColumnName] = "column3";
        col1[SchemaTableColumn.DataType] = typeof(DateTime);
        col1[SchemaTableColumn.AllowDBNull] = true;
        schemaTable.Rows.Add(col1);

        dataReaderMock.GetSchemaTable().Returns(schemaTable);
        dataReaderMock[Arg.Is("column1")].Returns("value1");
        dataReaderMock[Arg.Is("column2")].Returns(3);
        dataReaderMock[Arg.Is("column3")].Returns(date);

        var row = dataReaderMock.ToDataRow();

        Assert.AreEqual(3, row.Table.Columns.Count);
        Assert.IsInstanceOfType(row["column1"], typeof(string), "column1 type");
        Assert.IsInstanceOfType(row["column2"], typeof(int), "column2 type");
        Assert.IsInstanceOfType(row["column3"], typeof(DateTime), "column3 type");
        Assert.AreEqual("value1", row["column1"], "column1 value");
        Assert.AreEqual(3, row["column2"], "column2 value");
        Assert.AreEqual(date, row["column3"], "column3 value");
    }
}

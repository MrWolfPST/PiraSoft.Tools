using System.Data;

namespace PiraSoft.Tools.Data.Mapping;

/// <summary>
/// The class for child list mapping based on a <see cref="DataTable"/>.
/// </summary>
public class ChildTableMapping : ChildMappingBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChildTableMapping"/> class.
    /// </summary>
    /// <param name="tableName">The name of the table.</param>
    /// <exception cref="ArgumentException"><paramref name="tableName"/> must be contains a value.</exception>
    public ChildTableMapping(string? tableName) : this(tableName, null)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChildTableMapping"/> class.
    /// </summary>
    /// <param name="tableName">The name of the table.</param>
    /// <param name="mappings"></param>
    /// <exception cref="ArgumentException"><paramref name="tableName"/> must be contains a value.</exception>
    public ChildTableMapping(string? tableName, TypeMappings? mappings) : this(tableName, mappings, null)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChildTableMapping"/> class.
    /// </summary>
    /// <param name="tableName">The name of the table.</param>
    /// <param name="mappings">Mapping data for child objects type.</param>
    /// <param name="factory">Delegate for child objects construction.</param>
    /// <exception cref="ArgumentException"><paramref name="tableName"/> must be contains a value.</exception>
    public ChildTableMapping(string? tableName, TypeMappings? mappings, Func<object>? factory)
        : base(mappings, factory)
    {
        if (string.IsNullOrWhiteSpace(tableName))
        {
            throw new ArgumentException($"Parameter {nameof(tableName)} must contains a value.", nameof(tableName));
        }

        this.TableName = tableName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChildTableMapping"/> class.
    /// </summary>
    /// <param name="tableIndex">The index of the table.</param>
    public ChildTableMapping(int tableIndex) : this(tableIndex, null)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChildTableMapping"/> class.
    /// </summary>
    /// <param name="tableIndex">The index of the table.</param>
    /// <param name="mappings">Mapping data for child objects type.</param>
    public ChildTableMapping(int tableIndex, TypeMappings? mappings) : this(tableIndex, mappings, null)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChildTableMapping"/> class.
    /// </summary>
    /// <param name="tableIndex">The index of the table.</param>
    /// <param name="mappings">Mapping data for child objects type.</param>
    /// <param name="factory">Delegate for child objects construction.</param>
    public ChildTableMapping(int tableIndex, TypeMappings? mappings, Func<object>? factory)
        : base(mappings, factory) => this.TableIndex = tableIndex;

    /// <summary>
    /// The name of the table.
    /// </summary>
    public string? TableName { get; }

    /// <summary>
    /// The index of the table.
    /// </summary>
    public int? TableIndex { get; }

    /// <summary>
    /// Retrieve list of child rows.
    /// </summary>
    /// <param name="row">Current <see cref="DataRow"/>.</param>
    /// <returns><see cref="IEnumerable{DataRow}"/> the contains child rows.</returns>
    /// <exception cref="ArgumentException">Cannot find a table with name specified in <see cref="TableName"/> or at index specified in <see cref="TableIndex"/>.</exception>
    protected override IEnumerable<DataRow> GetChildRows(DataRow row)
    {
        if (this.TableIndex.HasValue)
        {
            try
            {
                return row.Table?.DataSet?.Tables[this.TableIndex.Value]?.Rows?.AsEnumerable()
                    ?? throw new ArgumentException($"Cannot find the table at index {this.TableIndex}.");
            }
            catch (IndexOutOfRangeException)
            {
                throw new ArgumentException($"Cannot find the table at index {this.TableIndex}.");
            }
        }
        else
        {
            return row.Table?.DataSet?.Tables[this.TableName]?.Rows?.AsEnumerable()
                ?? throw new ArgumentException($"Cannot find a table named {this.TableName}.");
        }
    }
}

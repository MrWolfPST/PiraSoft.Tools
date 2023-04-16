using System.Data;
using System.Reflection;

namespace PiraSoft.Tools.Data.Mapping;

/// <summary>
/// The class for composition mapping.
/// Composition mapping allow populate a property with an object whith values of several columns.
/// </summary>
public class CompositionMapping : MappingBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CompositionMapping"/> class.
    /// </summary>
    public CompositionMapping() : this(null, null, null)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositionMapping"/> class.
    /// </summary>
    /// <param name="factory">Delegate for child objects construction.</param>
    public CompositionMapping(Func<object>? factory) : this(null, null, factory)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositionMapping"/> class.
    /// </summary>
    /// <param name="prefix">Prefix to add to the column name.</param>
    public CompositionMapping(string? prefix) : this(prefix, null, null)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositionMapping"/> class.
    /// </summary>
    /// <param name="prefix">Prefix to add to the column name.</param>
    /// <param name="factory">Delegate for child objects construction.</param>
    public CompositionMapping(string? prefix, Func<object>? factory) : this(prefix, null, factory)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositionMapping"/> class.
    /// </summary>
    /// <param name="prefix">Prefix to add to the column name.</param>
    /// <param name="suffix">Suffix to append to the column name.</param>
    public CompositionMapping(string? prefix, string? suffix) : this(prefix, suffix, null)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositionMapping"/> class.
    /// </summary>
    /// <param name="prefix">Prefix to add to the column name.</param>
    /// <param name="suffix">Suffix to append to the column name.</param>
    /// <param name="factory">Delegate for child objects construction.</param>
    /// <param name="columnCheckLogic">Type of column presence and valorization check.</param>
    /// <param name="columnsToCheck">List of columns names to check for presence ad not null value.</param>
    /// <remarks>If <paramref name="columnCheckLogic"/> is not an empty list, target property will be set only if all or any specified columns are present and not have an empty value, in accordin with logic specified in <paramref name="columnCheckLogic"/>.</remarks>
    public CompositionMapping(string? prefix, string? suffix, Func<object>? factory, ColumnsCheckLogic columnCheckLogic = ColumnsCheckLogic.All, params string[] columnsToCheck)
    {
        this.Prefix = prefix;
        this.Suffix = suffix;
        this.Factory = factory;
        this.ColumnCheckLogic = columnCheckLogic;

        if (columnsToCheck != null && columnsToCheck.Any())
        {
            this.ColumnsToCheck = new HashSet<string>(columnsToCheck);
        }
    }

    /// <summary>
    /// Prefix to add to the column name.
    /// </summary>
    public string? Prefix { get; }

    /// <summary>
    /// Suffix to append to the column name.
    /// </summary>
    public string? Suffix { get; }

    /// <summary>
    /// Type of column presence and valorization check.
    /// </summary>
    public HashSet<string>? ColumnsToCheck { get; }

    /// <summary>
    /// List of columns names to check for presence ad not null value.
    /// </summary>
    public ColumnsCheckLogic ColumnCheckLogic { get; }

    /// <summary>
    /// Delegate for child objects construction.
    /// </summary>
    public Func<object>? Factory { get; }

    /// <summary>
    /// Map data into property of target.
    /// </summary>
    /// <param name="target">Target object to popolate.</param>
    /// <param name="propertyInfo"><see cref="PropertyInfo"/> that identity property to popolate.</param>
    /// <param name="row"><see cref="DataRow"/> that contains data.</param>
    /// <exception cref="InvalidOperationException">Unable to create an instance of target property type.</exception>
    protected internal override void Map(object target, PropertyInfo propertyInfo, DataRow row)
    {
        var factory = this.Factory
                ?? (() => Activator.CreateInstance(propertyInfo.PropertyType)
                ?? throw new InvalidOperationException($"Unable to create an instance of {propertyInfo.PropertyType} type."));
        var mappings = TypeMappings.Generate(propertyInfo.PropertyType);

        if (this.Prefix != null || this.Suffix != null)
        {
            var pattern = this.Prefix + "{0}" + this.Suffix;

            mappings.Mappings.Select(m => m.Mapping).OfType<ColumnMapping>().ForEach(m => m.ColumnName = string.Format(pattern, m.ColumnName));
        }

        var checkColumns = this.ColumnsToCheck ?? new HashSet<string>(mappings.Mappings.Select(m => m.Mapping).OfType<ColumnMapping>().Select(i => i.ColumnName));

        if ((this.ColumnCheckLogic == ColumnsCheckLogic.All && checkColumns.All(i => row.Table.Columns.Contains(i) && !row.IsNull(i)))
            || (this.ColumnCheckLogic == ColumnsCheckLogic.Any && !checkColumns.Any(i => row.Table.Columns.Contains(i) && row.IsNull(i))))
        {
            this.SetValue(target, propertyInfo, Mapper.Map(factory, row, mappings));
        }
    }
}
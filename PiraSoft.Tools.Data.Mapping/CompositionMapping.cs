using System.Data;
using System.Reflection;

namespace PiraSoft.Tools.Data.Mapping;

public class CompositionMapping : MappingBase
{
    public CompositionMapping() : this(null, null, null)
    { }

    public CompositionMapping(Func<object>? factory) : this(null, null, factory)
    { }

    public CompositionMapping(string? prefix) : this(prefix, null, null)
    { }

    public CompositionMapping(string? prefix, Func<object>? factory) : this(prefix, null, factory)
    { }

    public CompositionMapping(string? prefix, string? suffix) : this(prefix, suffix, null)
    { }

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

    public string? Prefix { get; }

    public string? Suffix { get; }

    public HashSet<string>? ColumnsToCheck { get; }

    public ColumnsCheckLogic ColumnCheckLogic { get; }

    public Func<object>? Factory { get; }

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
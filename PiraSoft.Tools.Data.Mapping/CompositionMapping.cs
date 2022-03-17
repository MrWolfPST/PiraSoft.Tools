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

    public CompositionMapping(string? prefix, string? suffix, Func<object>? factory)
    {
        this.Prefix = prefix;
        this.Suffix = suffix;
        this.Factory = factory;
    }

    public string? Prefix { get; }

    public string? Suffix { get; }

    public Func<object>? Factory { get; }

    protected internal override void Map(object target, PropertyInfo propertyInfo, DataRow row)
    {
        var factory = this.Factory
                ?? (() => Activator.CreateInstance(propertyInfo.PropertyType)
                ?? throw new InvalidOperationException($"Unable to create an instance of {propertyInfo.PropertyType} type."));

        if (this.Prefix == null && this.Suffix == null)
        {
            this.SetValue(target, propertyInfo, Mapper.Map(factory, row));
        }
        else
        {
            var pattern = this.Prefix + "{0}" + this.Suffix;
            var mappings = TypeMappings.Generate(propertyInfo.PropertyType);

            mappings.Mappings.OfType<ColumnMapping>().ForEach(m => m.ColumnName = string.Format(pattern, m.ColumnName));

            this.SetValue(target, propertyInfo, Mapper.Map(factory, row, mappings));
        }
    }
}

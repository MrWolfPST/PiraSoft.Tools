using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PiraSoft.Tools.Data.Mapping;

public class RelationMapping : MappingBase
{
    public RelationMapping(string relationName) : this(relationName, null)
    { }

    public RelationMapping(string relationName, Dictionary<string, MappingBase>? mappings)
    {
        this.RelationName = relationName;
        this.Mappings = mappings;
    }

    public RelationMapping(string relationName, Dictionary<string, MappingBase>? mappings, Func<object>? factory)
    {
        this.RelationName = relationName ?? throw new ArgumentNullException(relationName);
        this.Mappings = mappings;
        this.Factory = factory;
    }

    public string RelationName { get; }

    public Dictionary<string, MappingBase>? Mappings { get; }

    public Func<object>? Factory { get; }

    protected internal override void Map(object target, PropertyInfo propertyInfo, DataRow row)
    {
        var relatedRow = row.GetParentRow(this.RelationName);
        var factory = this.Factory
                ?? (() => Activator.CreateInstance(propertyInfo.PropertyType)
                ?? throw new InvalidOperationException($"Unable to create an instance of {propertyInfo.PropertyType} type."));

        if (relatedRow != null)
        {
            this.SetValue(target, propertyInfo, Mapper.Map(factory, relatedRow, this.Mappings));
        }
    }
}

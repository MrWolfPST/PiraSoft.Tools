using System.Data;
using System.Reflection;

namespace PiraSoft.Tools.Data.Mapping;

internal class PropertyMapping
{
    public PropertyMapping(PropertyInfo propertyInfo, MappingBase mapping)
    {
        this.PropertyInfo = propertyInfo;
        this.Mapping = mapping;
    }

    public PropertyInfo PropertyInfo { get; }

    public MappingBase Mapping { get; }

    public void Map(object target, DataRow row)
        => this.Mapping.Map(target, this.PropertyInfo, row);
}

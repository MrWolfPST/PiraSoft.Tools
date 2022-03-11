using PiraSoft.Tools.Data.Mapping.Attributes;
using System.Data;
using System.Reflection;

namespace PiraSoft.Tools.Data.Mapping;

internal class Mapping
{
    private readonly MappingBase _mapping;

    public Mapping(PropertyInfo propertyInfo, MappingBase mapping)
    {
        this.PropertyInfo = propertyInfo;
        _mapping = mapping;
    }

    public PropertyInfo PropertyInfo { get; }

    public void Map(object target, DataRow row)
        => _mapping.Map(target, this.PropertyInfo, row);
}

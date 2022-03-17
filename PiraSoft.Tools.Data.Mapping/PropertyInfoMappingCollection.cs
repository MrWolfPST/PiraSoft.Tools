using System.Collections.ObjectModel;
using System.Reflection;

namespace PiraSoft.Tools.Data.Mapping;

internal class PropertyInfoMappingCollection : KeyedCollection<PropertyInfo, PropertyMapping>
{
    protected override PropertyInfo GetKeyForItem(PropertyMapping item)
        => item.PropertyInfo;

    public void SetItem(PropertyInfo propertyInfo, MappingBase mapping)
        => this.SetItem(this.IndexOf(this[propertyInfo]), new PropertyMapping(propertyInfo, mapping));
}

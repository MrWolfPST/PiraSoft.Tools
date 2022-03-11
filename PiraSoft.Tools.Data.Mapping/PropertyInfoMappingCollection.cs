using PiraSoft.Tools.Data.Mapping.Attributes;
using System.Collections.ObjectModel;
using System.Reflection;

namespace PiraSoft.Tools.Data.Mapping;

internal class PropertyInfoMappingCollection : KeyedCollection<PropertyInfo, Mapping>
{
    protected override PropertyInfo GetKeyForItem(Mapping item)
        => item.PropertyInfo;

    public void AddMapping(PropertyInfo propertyInfo, Dictionary<string, MappingBase>? mappings = null, string fieldNamePattern = "{0}")
    {
        var mapping = mappings?.TryGetValue(propertyInfo.Name);

        if (mapping == null && propertyInfo.GetCustomAttribute<IgnoreMappingAttribute>() == null)
        {
            mapping = propertyInfo.GetCustomAttribute<MappingAttribute>()?.GetMapping()
                ?? new ColumnMapping(string.Format(fieldNamePattern, propertyInfo.Name));
        }

        if (mapping != null)
            this.Add(new Mapping(propertyInfo, mapping));
    }
}

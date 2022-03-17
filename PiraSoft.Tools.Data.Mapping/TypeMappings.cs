using PiraSoft.Tools.Data.Mapping.Attributes;
using System.Linq.Expressions;
using System.Reflection;

namespace PiraSoft.Tools.Data.Mapping;

public class TypeMappings
{
    public TypeMappings(Type type)
        => this.Type = type;

    public Type Type { get; }

    internal PropertyInfoMappingCollection Mappings { get; } = new PropertyInfoMappingCollection();

    public MappingBase? GetMapping<T>(Expression<Func<T>> property)
        => this.GetMapping(((MemberExpression)property.Body).Member.Name);

    public MappingBase? GetMapping(string propertyName)
        => (from i in this.Mappings where i.PropertyInfo.Name == propertyName select i.Mapping).FirstOrDefault();

    public void AddMapping<T>(Expression<Func<T>> property, MappingBase mapping)
        => this.AddMapping(((MemberExpression)property.Body).Member.Name, mapping);

    public void AddMapping(string propertyName, MappingBase mapping)
    {
        var propertyInfo = this.Type.GetProperty(propertyName);

        if (propertyInfo == null)
        {
            throw new InvalidOperationException($"The {propertyName} property not exists in {this.Type.FullName} type.");
        }

        this.AddMapping(propertyInfo, mapping);
    }

    private void AddMapping(PropertyInfo property, MappingBase mapping)
        => this.Mappings.Add(new PropertyMapping(property, mapping));

    public void UpdateMapping<T>(Expression<Func<T>> property, MappingBase mapping)
        => this.UpdateMapping(((MemberExpression)property.Body).Member.Name, mapping);

    public void UpdateMapping(string propertyName, MappingBase mapping)
    {
        var propertyInfo = this.Type.GetProperty(propertyName);

        if (propertyInfo == null)
        {
            throw new InvalidOperationException($"The {propertyName} property not exists in {this.Type.FullName} type.");
        }

        this.UpdateMapping(propertyInfo, mapping);
    }

    private void UpdateMapping(PropertyInfo property, MappingBase mapping)
        => this.Mappings.SetItem(property, mapping);

    public void AddOrUpdateMapping<T>(Expression<Func<T>> property, MappingBase mapping)
        => this.AddOrUpdateMapping(((MemberExpression)property.Body).Member.Name, mapping);

    public void AddOrUpdateMapping(string propertyName, MappingBase mapping)
    {
        var propertyInfo = this.Type.GetProperty(propertyName);

        if (propertyInfo == null)
        {
            throw new InvalidOperationException($"The {propertyName} property not exists in {this.Type.FullName} type.");
        }

        this.AddOrUpdateMapping(propertyInfo, mapping);
    }

    private void AddOrUpdateMapping(PropertyInfo property, MappingBase mapping)
    {
        if (this.Mappings.Contains(property))
        {
            this.UpdateMapping(property, mapping);
        }
        else
        {
            this.AddMapping(property, mapping);
        }
    }

    public static TypeMappings Generate<T>()
        => Generate(typeof(T));

    public static TypeMappings Generate(Type type)
    {
        var retVal = new TypeMappings(type);

        type.GetProperties()
            .Where(i => i.CanWrite && i.GetCustomAttribute<IgnoreMappingAttribute>() == null && (i.GetCustomAttribute<MappingAttribute>() != null || IsImplicitMappingSupprotedType(i.PropertyType)))
            .ForEach(i => retVal.AddMapping(i, i.GetCustomAttribute<MappingAttribute>()?.GetMapping() ?? new ColumnMapping(i.Name)));

        return retVal;
    }

    private static bool IsImplicitMappingSupprotedType(Type type)
        => (int)Type.GetTypeCode(Nullable.GetUnderlyingType(type) ?? type) > 2;
}

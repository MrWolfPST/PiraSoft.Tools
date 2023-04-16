using PiraSoft.Tools.Data.Mapping.Attributes;
using System.Linq.Expressions;
using System.Reflection;

namespace PiraSoft.Tools.Data.Mapping;

/// <summary>
/// Represents a type's mappings informations.
/// </summary>
public class TypeMappings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeMappings"/> class.
    /// </summary>
    /// <param name="type">Target type.</param>
    public TypeMappings(Type type)
        => this.Type = type;

    /// <summary>
    /// Target type.
    /// </summary>
    public Type Type { get; }

    internal PropertyInfoMappingCollection Mappings { get; } = new PropertyInfoMappingCollection();

    /// <summary>
    /// Get the mapping informations for specified property.
    /// </summary>
    /// <typeparam name="T">Type of object.</typeparam>
    /// <param name="property"><see cref="Expression{Func}"/> tha represents the property.</param>
    /// <returns>Mapping information for specified property.</returns>
    public MappingBase? GetMapping<T>(Expression<Func<T>> property)
        => this.GetMapping(((MemberExpression)property.Body).Member.Name);

    /// <summary>
    /// Get the mapping informations for specified property.
    /// </summary>
    /// <param name="propertyName">The name of property.</param>
    /// <returns>Mapping information for specified property.</returns>
    public MappingBase? GetMapping(string propertyName)
        => (from i in this.Mappings where i.PropertyInfo.Name == propertyName select i.Mapping).FirstOrDefault();

    /// <summary>
    /// Add mapping informations for specified property.
    /// </summary>
    /// <typeparam name="T">Type of object.</typeparam>
    /// <param name="property"><see cref="Expression{Func}"/> tha represents the property.</param>
    /// <param name="mapping">The mapping informations.</param>
    public void AddMapping<T>(Expression<Func<T>> property, MappingBase mapping)
        => this.AddMapping(((MemberExpression)property.Body).Member.Name, mapping);

    /// <summary>
    /// Add mapping informations for specified property.
    /// </summary>
    /// <param name="propertyName">The property name.</param>
    /// <param name="mapping">The mapping informations.</param>
    /// <exception cref="InvalidOperationException">A property with <paramref name="propertyName"/> not found in the target type.</exception>
    public void AddMapping(string propertyName, MappingBase mapping)
    {
        var propertyInfo = this.Type.GetProperty(propertyName);

        if (propertyInfo == null)
        {
            throw new InvalidOperationException($"The {propertyName} property not exists in {this.Type.FullName} type.");
        }

        this.AddMapping(propertyInfo, mapping);
    }

    /// <summary>
    /// Add mapping informations for specified property.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <param name="mapping">The mapping informations.</param>
    private void AddMapping(PropertyInfo property, MappingBase mapping)
        => this.Mappings.Add(new PropertyMapping(property, mapping));

    /// <summary>
    /// Update mapping informations for specified property.
    /// </summary>
    /// <typeparam name="T">Type of object.</typeparam>
    /// <param name="property"><see cref="Expression{Func}"/> tha represents the property.</param>
    /// <param name="mapping">The mapping informations.</param>
    public void UpdateMapping<T>(Expression<Func<T>> property, MappingBase mapping)
        => this.UpdateMapping(((MemberExpression)property.Body).Member.Name, mapping);

    /// <summary>
    /// Update mapping informations for specified property.
    /// </summary>
    /// <param name="propertyName">The property name.</param>
    /// <param name="mapping">The mapping informations.</param>
    /// <exception cref="InvalidOperationException">A property with <paramref name="propertyName"/> not found in the target type.</exception>
    public void UpdateMapping(string propertyName, MappingBase mapping)
    {
        var propertyInfo = this.Type.GetProperty(propertyName);

        if (propertyInfo == null)
        {
            throw new InvalidOperationException($"The {propertyName} property not exists in {this.Type.FullName} type.");
        }

        this.UpdateMapping(propertyInfo, mapping);
    }

    /// <summary>
    /// Update mapping informations for specified property.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <param name="mapping">The mapping informations.</param>
    private void UpdateMapping(PropertyInfo property, MappingBase mapping)
        => this.Mappings.SetItem(property, mapping);

    /// <summary>
    /// Add or update mapping informations for specified property.
    /// </summary>
    /// <typeparam name="T">Type of object.</typeparam>
    /// <param name="property"><see cref="Expression{Func}"/> tha represents the property.</param>
    /// <param name="mapping">The mapping informations.</param>
    public void AddOrUpdateMapping<T>(Expression<Func<T>> property, MappingBase mapping)
        => this.AddOrUpdateMapping(((MemberExpression)property.Body).Member.Name, mapping);

    /// <summary>
    /// Add or update mapping informations for specified property.
    /// </summary>
    /// <param name="propertyName">The property name.</param>
    /// <param name="mapping">The mapping informations.</param>
    /// <exception cref="InvalidOperationException">A property with <paramref name="propertyName"/> not found in the target type.</exception>
    public void AddOrUpdateMapping(string propertyName, MappingBase mapping)
    {
        var propertyInfo = this.Type.GetProperty(propertyName);

        if (propertyInfo == null)
        {
            throw new InvalidOperationException($"The {propertyName} property not exists in {this.Type.FullName} type.");
        }

        this.AddOrUpdateMapping(propertyInfo, mapping);
    }

    /// <summary>
    /// Add or update mapping informations for specified property.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <param name="mapping">The mapping informations.</param>
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

    /// <summary>
    /// Generate mappings informations for specified type.
    /// </summary>
    /// <typeparam name="T">Target type.</typeparam>
    /// <returns>An instance of <see cref="TypeMappings"/> that represents inferred mappings informations</returns>
    public static TypeMappings Generate<T>()
        => Generate(typeof(T));

    /// <summary>
    /// Generate mappings informations for specified type.
    /// </summary>
    /// <param name="type">Target type.</param>
    /// <returns>An instance of <see cref="TypeMappings"/> that represents inferred mappings informations</returns>
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

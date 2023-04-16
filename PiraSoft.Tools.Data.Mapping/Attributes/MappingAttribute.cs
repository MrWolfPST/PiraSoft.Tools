namespace PiraSoft.Tools.Data.Mapping.Attributes;

/// <summary>
/// Abstract base class for mappings attributes.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public abstract class MappingAttribute : Attribute
{
    /// <summary>
    /// Get the mapping informations.
    /// </summary>
    /// <returns>The mapping informations</returns>
    protected internal abstract MappingBase GetMapping();
}

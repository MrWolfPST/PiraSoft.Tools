namespace PiraSoft.Tools.Data.Mapping.Attributes;

/// <summary>
/// Specifies that property must be ignored in mapping process.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class IgnoreMappingAttribute : Attribute
{ }

namespace PiraSoft.Tools.Data.Mapping.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public abstract class MappingAttribute : Attribute
{
    protected internal abstract MappingBase GetMapping();
}

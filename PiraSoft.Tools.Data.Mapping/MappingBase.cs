using System.Data;
using System.Reflection;

namespace PiraSoft.Tools.Data.Mapping;

public abstract class MappingBase
{
    protected internal abstract void Map(object target, PropertyInfo propertyInfo, DataRow row);

    protected virtual object? Convert(object? value, Type targetType)
    {
        if (value == null || value == DBNull.Value)
        {
            return null;
        }

        try
        {
            if (targetType.IsEnum)
            {
                if (value is string stringValue)
                {
                    return Enum.Parse(targetType, stringValue);
                }
                else
                {
                    return Enum.ToObject(targetType, value);
                }
            }
            else if (targetType == typeof(Stream) || targetType.IsSubclassOf(typeof(Stream)))
            {
                return new MemoryStream((byte[])value);
            }
            else
            {
                return System.Convert.ChangeType(value, targetType);
            }
        }
        catch (InvalidCastException)
        {
            throw new InvalidCastException($"Unable cast value '{value}' on type {targetType}");
        }
    }

    protected void SetValue(object target, PropertyInfo propertyInfo, object? value)
    {
        try
        {
            propertyInfo.SetValue(target, this.Convert(value, propertyInfo.PropertyType));
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException(ex.Message, propertyInfo.Name, ex.InnerException);
        }
    }
}
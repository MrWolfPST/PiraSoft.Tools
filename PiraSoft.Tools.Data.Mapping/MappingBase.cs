using System.Data;
using System.Reflection;

namespace PiraSoft.Tools.Data.Mapping;

/// <summary>
/// The abstract base class for mapping.
/// </summary>
public abstract class MappingBase
{
    /// <summary>
    /// Map data into property of target.
    /// </summary>
    /// <param name="target">Target object to popolate.</param>
    /// <param name="propertyInfo"><see cref="PropertyInfo"/> that identity property to popolate.</param>
    /// <param name="row"><see cref="DataRow"/> that contains data.</param>
    protected internal abstract void Map(object target, PropertyInfo propertyInfo, DataRow row);

    /// <summary>
    /// Convert value to desidered type.
    /// Default convertions rules are:
    /// <list type="bullet">
    /// <item><paramref name="targetType"/> is an enumeration, <paramref name="value"/> can be a <see cref="string"/> or any numeric value, if the numeric value is not compatible with the enumeration underlying type an <see cref="InvalidCastException"/> was throw.</item>
    /// <item><paramref name="targetType"/> is a <see cref="Stream"/> or a subclass of <see cref="Stream"/>, returned type is always an instance of <see cref="MemoryStream"/>, value must be an array of <see cref="byte"/> othewise an <see cref="InvalidCastException"/> was throw.</item>
    /// <item>In all other cases is used <see cref="System.Convert.ChangeType(object?, Type)"/>.</item>
    /// </list>
    /// </summary>
    /// <param name="value">Value to be converted.</param>
    /// <param name="targetType"><see cref="Type"/> that rapresent desidered target type.</param>
    /// <returns>The converted value</returns>
    /// <exception cref="InvalidCastException"><paramref name="value"/>can't be converted into <paramref name="targetType"/> type.</exception>
    protected virtual object? Convert(object? value, Type targetType)
    {
        if (value == null || value == DBNull.Value)
        {
            return null;
        }

        targetType = Nullable.GetUnderlyingType(targetType) ?? targetType;

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

    /// <summary>
    /// Set property of target object with specified value.
    /// </summary>
    /// <param name="target">The target object.</param>
    /// <param name="propertyInfo"><see cref="PropertyInfo"/> that identity property to popolate.</param>
    /// <param name="value">The value to set.</param>
    /// <exception cref="ArgumentException">The property's set accessor is not found or value cannot be converted to the type of property.</exception>
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
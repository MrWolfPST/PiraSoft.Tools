using System.Collections;

namespace System;

/// <summary>
/// A set of <see cref="Type"/> extension methods
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Gets a value that indicates whether the current <see cref="Type"/> implements the <see cref="IEnumerable"/> interface.
    /// </summary>
    /// <param name="target">Current <see cref="Type"/>.</param>
    /// <returns>True if the type implements the <see cref="IEnumerable"/> interface; otherwise, false.</returns>
    public static bool IsEnumerable(this Type target)
        => typeof(IEnumerable).IsAssignableFrom(target);

    /// <summary>
    /// Gets a value that indicates whether the current <see cref="Type"/> represents a numeric data type.
    /// </summary>
    /// <param name="target">Current <see cref="Type"/>.</param>
    /// <returns>True if the type represents a numeric data type; otherwise, false.</returns>
    public static bool IsNumericDatatype(this Type target)
        => Type.GetTypeCode(target) switch
        {
            TypeCode.Byte or
            TypeCode.SByte or
            TypeCode.UInt16 or
            TypeCode.UInt32 or
            TypeCode.UInt64 or
            TypeCode.Int16 or
            TypeCode.Int32 or
            TypeCode.Int64 or
            TypeCode.Decimal or
            TypeCode.Double or
            TypeCode.Single => true,
            _ => false,
        };

    /// <summary>
    /// Determines whether the current <see cref="Type"/> is or derives from the specified <see cref="Type"/>.
    /// </summary>
    /// <typeparam name="T">The type to compare with the current type.</typeparam>
    /// <param name="target">Current <see cref="Type"/>.</param>
    /// <returns>True if the current Type are equal or derives from type specified in <typeparamref name="T"/>; otherwise, false.</returns>
    public static bool IsSameOrSubclass<T>(this Type target)
        => target.IsSameOrSubclass(typeof(T));

    /// <summary>
    /// Determines whether the current <see cref="Type"/> is or derives from the specified <see cref="Type"/>.
    /// </summary>
    /// <param name="target">Current <see cref="Type"/>.</param>
    /// <param name="baseType">The type to compare with the current type.</param>
    /// <returns>True if the current Type are equal or derives from <paramref name="baseType"/>; otherwise, false.</returns>
    public static bool IsSameOrSubclass(this Type target, Type baseType)
        => target.IsSubclassOf(baseType) || target == baseType;

    /// <summary>
    /// Determines whether the current <see cref="Type"/> is a <see cref="Nullable{T}"/> type.
    /// </summary>
    /// <param name="target">Current <see cref="Type"/>.</param>
    /// <returns>True if the current Type is a <see cref="Nullable{T}"/> type; otherwise, false.</returns>
    public static bool IsNullableType(this Type target)
        => Nullable.GetUnderlyingType(target) != null;
}

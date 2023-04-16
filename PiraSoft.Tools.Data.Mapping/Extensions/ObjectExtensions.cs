using PiraSoft.Tools.Data.Mapping;
using System.Data;

namespace System;

/// <summary>
/// A set of <see cref="object"/> extension methods that use <see cref="Mapper"/>.
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Populate the object with data.
    /// </summary>
    /// <param name="target">The object to populate.</param>
    /// <param name="row">The data to populate the object.</param>
    /// <exception cref="ArgumentNullException"><paramref name="target"/> is null or <paramref name="row"/> is null.</exception>
    public static void Map(this object target, DataRow row)
        => target.Map(row, null);

    /// <summary>
    /// Populate the object with data.
    /// </summary>
    /// <param name="target">The object to populate.</param>
    /// <param name="row">The data to populate the object.</param>
    /// <param name="mappings">Type mappings informations.</param>
    /// <exception cref="ArgumentNullException"><paramref name="target"/> is null or <paramref name="row"/> is null.</exception>
    public static void Map(this object target, DataRow row, TypeMappings? mappings)
        => row.Map(target, mappings);
}

using System.Collections.ObjectModel;
using System.Data;

namespace PiraSoft.Tools.Data.Mapping;

/// <summary>
/// Allow to map an <see cref="DataRow"/> into an object of configured type.
/// </summary>
public static class Mapper
{
    private static readonly IDictionary<Type, TypeMappings> typeMappingsCatalog = new Dictionary<Type, TypeMappings>();

    /// <summary>
    /// The type's mappings catalog.
    /// </summary>
    public static IReadOnlyDictionary<Type, TypeMappings> TypeMappingsCatalog { get; } = new ReadOnlyDictionary<Type, TypeMappings>(typeMappingsCatalog);

    /// <summary>
    /// Add and return an inferred type's mappings to the catalog.
    /// </summary>
    /// <typeparam name="T">Target type.</typeparam>
    /// <returns>An instance of <see cref="TypeMappings"/> with inferred type's mappings</returns>
    public static TypeMappings AddTypeMappings<T>()
        => AddTypeMappings(typeof(T));

    /// <summary>
    /// Add and return an inferred type's mappings to the catalog.
    /// </summary>
    /// <param name="type">Target type.</param>
    /// <returns>An instance of <see cref="TypeMappings"/> with inferred type's mappings</returns>
    public static TypeMappings AddTypeMappings(Type type)
        => AddTypeMappings(TypeMappings.Generate(type));

    /// <summary>
    /// Add and return a type's mappings to the catalog.
    /// </summary>
    /// <param name="typeMappings">Type's mappings to add.</param>
    /// <returns>Value of <paramref name="typeMappings"/>.</returns>
    public static TypeMappings AddTypeMappings(TypeMappings typeMappings)
    {
        typeMappingsCatalog.AddOrUpdate(typeMappings.Type, typeMappings);
        return typeMappings;
    }

    /// <summary>
    /// Returns the type's mappings for specified type, if not present in the catalog, a new inferred type's mappings is added to the catalog.
    /// </summary>
    /// <param name="type">The type to retrieve mappings for.</param>
    /// <returns>The <see cref="TypeMappings"/> for requested type.</returns>
    public static TypeMappings EnsureTypeMappings(Type type)
        => typeMappingsCatalog.TryGetValue(type) ?? AddTypeMappings(type);

    /// <summary>
    /// Create a new instance and populate an object with data.
    /// </summary>
    /// <param name="factory">Delegate for object construction.</param>
    /// <param name="row">The data to populate the object.</param>
    /// <returns>An instance of an object populated with data.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="factory"/> is null.</exception>
    public static object? Map(Func<object> factory, DataRow? row)
        => Map(factory, row, null);

    /// <summary>
    /// Create a new instance and populate an object with data.
    /// </summary>
    /// <param name="factory">Delegate for object construction.</param>
    /// <param name="row">The data to populate the object.</param>
    /// <param name="mappings">Type's mappings informations.</param>
    /// <returns>An instance of an object populated with data.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="factory"/> is null.</exception>
    public static object? Map(Func<object> factory, DataRow? row, TypeMappings? mappings)
        => Map<object>(factory, row, mappings);

    /// <summary>
    /// Populate the object with data.
    /// </summary>
    /// <param name="target">The object to populate.</param>
    /// <param name="row">The data to populate the object.</param>
    /// <exception cref="ArgumentNullException"><paramref name="target"/> is null or <paramref name="row"/> is null.</exception>
    public static void Map(object target, DataRow? row)
        => Map(target, row, null);

    /// <summary>
    /// Populate the object with data.
    /// </summary>
    /// <param name="target">The object to populate.</param>
    /// <param name="row">The data to populate the object.</param>
    /// <param name="mappings">Type mappings informations.</param>
    /// <exception cref="ArgumentException"><paramref name="target"/> is null or <paramref name="row"/> is null.</exception>
    public static void Map(object target, DataRow? row, TypeMappings? mappings)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentNullException.ThrowIfNull(row);

        mappings ??= EnsureTypeMappings(target.GetType());

        if (target.GetType() != mappings.Type)
        {
            throw new ArgumentException($"Type of {nameof(target)} is not the same of type specified in {nameof(mappings)}.");
        }

        mappings.Mappings.ForEach(i => i.Map(target, row));
    }

    /// <summary>
    /// Create a new instance and populate an instance of specified type with data.
    /// </summary>
    /// <typeparam name="T">The type of object to create. The type must have a parameterless constructor.</typeparam>
    /// <param name="row">The data to populate the object.</param>
    /// <returns>An instance of <typeparamref name="T"/> populated with data.</returns>
    public static T? Map<T>(DataRow? row)
        where T : new()
        => Map<T>(() => new T(), row, null);

    /// <summary>
    /// Create a new instance and populate an instance of specified type with data.
    /// </summary>
    /// <typeparam name="T">The type of object to create. The type must have a parameterless constructor.</typeparam>
    /// <param name="row">The data to populate the object.</param>
    /// <param name="mappings">Type's mappings informations.</param>
    /// <returns>An instance of <typeparamref name="T"/> populated with data.</returns>
    public static T? Map<T>(DataRow? row, TypeMappings? mappings)
        where T : new()
        => Map<T>(() => new T(), row, mappings);

    /// <summary>
    /// Create a new instance and populate an instance of specified type with data.
    /// </summary>
    /// <typeparam name="T">The type of object to create.</typeparam>
    /// <param name="factory">Delegate for object construction.</param>
    /// <param name="row">The data to populate the object.</param>
    /// <returns>An instance of <typeparamref name="T"/> populated with data.</returns>
    public static T? Map<T>(Func<T> factory, DataRow? row)
        => Map<T>(factory, row, null);

    /// <summary>
    /// Create a new instance and populate an instance of specified type with data.
    /// </summary>
    /// <typeparam name="T">The type of object to create.</typeparam>
    /// <param name="factory">Delegate for object construction.</param>
    /// <param name="row">The data to populate the object.</param>
    /// <param name="mappings">Type's mappings informations.</param>
    /// <returns>An instance of <typeparamref name="T"/> populated with data.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="factory"/> is null.</exception>
    /// <exception cref="InvalidOperationException"><paramref name="factory"/> returns null.</exception>
    public static T? Map<T>(Func<T> factory, DataRow? row, TypeMappings? mappings)
    {
        ArgumentNullException.ThrowIfNull(factory);

        if (row == null)
        {
            return default;
        }

        var retVal = factory();

        if (retVal == null)
        {
            throw new InvalidOperationException($"{nameof(factory)} returns a null object.");
        }

        Map(retVal, row, mappings);

        return retVal;
    }
}

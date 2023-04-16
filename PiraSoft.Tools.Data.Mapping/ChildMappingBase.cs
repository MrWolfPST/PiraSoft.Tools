using System.Collections;
using System.Data;
using System.Reflection;

namespace PiraSoft.Tools.Data.Mapping;

/// <summary>
/// The abstract base class for child list mapping.
/// </summary>
public abstract class ChildMappingBase : MappingBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChildMappingBase"/> class.
    /// </summary>
    /// <param name="mappings">Mapping data for child objects type.</param>
    /// <param name="factory">Delegate for child objects construction.</param>
    protected ChildMappingBase(TypeMappings? mappings, Func<object>? factory)
    {
        this.Mappings = mappings;
        this.Factory = factory;
    }

    /// <summary>
    /// Mapping data for child objects type.
    /// </summary>
    public TypeMappings? Mappings { get; }

    /// <summary>
    /// Delegate for child objects construction.
    /// </summary>
    public Func<object>? Factory { get; }

    /// <summary>
    /// Map data into property of target.
    /// </summary>
    /// <param name="target">Target object to popolate.</param>
    /// <param name="propertyInfo"><see cref="PropertyInfo"/> that identity property to popolate.</param>
    /// <param name="row"><see cref="DataRow"/> that contains data.</param>
    protected internal sealed override void Map(object target, PropertyInfo propertyInfo, DataRow row)
    {
        Type? itemsType = null;

        if (propertyInfo.PropertyType.IsArray)
        {
            itemsType = propertyInfo.PropertyType.GetElementType();
        }
        else if (propertyInfo.PropertyType.IsEnumerable())
        {
            itemsType = propertyInfo.PropertyType.GetGenericArguments().FirstOrDefault();
        }

        if (itemsType == null)
        {
            throw new InvalidOperationException($"Unable to detect elements type of {propertyInfo.PropertyType}.");
        }

        if (this.Mappings != null && itemsType != this.Mappings.Type)
        {
            throw new ArgumentException($"Type of {nameof(target)} is not the same of type specified in {nameof(this.Mappings)}.");
        }

        var factory = this.Factory
                ?? (() => Activator.CreateInstance(itemsType)
                ?? throw new InvalidOperationException($"Unable to create an instance of {itemsType} type."));
        var mapping = Mapper.EnsureTypeMappings(itemsType);
        var childRows = this.GetChildRows(row);
        var items = (IList?)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemsType))
            ?? throw new InvalidOperationException($"Unable to create an instance of List<{itemsType}> type.");

        childRows.ForEach(i => items.Add(Mapper.Map(factory, i, mapping)));

        this.SetValue(target, propertyInfo, items);
    }

    /// <summary>
    /// Convert value to desidered type.
    /// Convertions rules are:
    /// <list type="bullet">
    /// <item><paramref name="targetType"/> is an array, <paramref name="value"/> must be an <see cref="IList"/> of array elements type, if unable to detect elents type of <paramref name="targetType"/> an <see cref="InvalidOperationException"/> was throw.</item>
    /// <item>In all other cases <paramref name="value"/>.</item>
    /// </list>
    /// </summary>
    /// <param name="value">Value to be converted.</param>
    /// <param name="targetType"><see cref="Type"/> that rapresent desidered target type.</param>
    /// <returns>The converted value</returns>
    /// <exception cref="InvalidOperationException">Unable to detect elements type of <paramref name="targetType"/>.</exception>
    protected override object? Convert(object? value, Type targetType)
    {
        if (targetType.IsArray)
        {
            return ((IList?)value)?.ToArray(targetType.GetElementType() ?? throw new InvalidOperationException($"Unable to detect elements type of {targetType}."));
        }
        else
        {
            return value;
        }
    }

    /// <summary>
    /// Retrieve list of child rows.
    /// </summary>
    /// <param name="row">Current <see cref="DataRow"/>.</param>
    /// <returns><see cref="IEnumerable{DataRow}"/> the contains child rows.</returns>
    protected abstract IEnumerable<DataRow> GetChildRows(DataRow row);
}

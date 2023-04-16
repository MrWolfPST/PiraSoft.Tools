using System.Data;

namespace PiraSoft.Tools.Data.Mapping;

/// <summary>
/// The class for child list mapping based on a <see cref="DataRelation"/>.
/// </summary>
public sealed class ChildRelationMapping : ChildMappingBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChildRelationMapping"/> class.
    /// </summary>
    /// <param name="relationName">The name of relation.</param>
    /// <exception cref="ArgumentException"><paramref name="relationName"/> not contains a value.</exception>
    public ChildRelationMapping(string? relationName) : this(relationName, null)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChildRelationMapping"/> class.
    /// </summary>
    /// <param name="relationName">The name of relation.</param>
    /// <param name="mappings">Mapping data for child objects type.</param>
    /// <exception cref="ArgumentException"><paramref name="relationName"/> not contains a value.</exception>
    public ChildRelationMapping(string? relationName, TypeMappings? mappings) : this(relationName, mappings, null)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChildRelationMapping"/> class.
    /// </summary>
    /// <param name="relationName">The name of relation.</param>
    /// <param name="mappings">Mapping data for child objects type.</param>
    /// <param name="factory">Delegate for child objects construction.</param>
    /// <exception cref="ArgumentException"><paramref name="relationName"/> not contains a value.</exception>
    public ChildRelationMapping(string? relationName, TypeMappings? mappings, Func<object>? factory)
        : base(mappings, factory)
    {
        if (string.IsNullOrWhiteSpace(relationName))
        {
            throw new ArgumentException($"Parameter {nameof(relationName)} must contains a value.", nameof(relationName));
        }

        this.RelationName = relationName;
    }

    /// <summary>
    /// The name of relation.
    /// </summary>
    public string RelationName { get; }

    /// <summary>
    /// Retrieve list of child rows.
    /// </summary>
    /// <param name="row">Current <see cref="DataRow"/>.</param>
    /// <returns><see cref="IEnumerable{DataRow}"/> the contains child rows.</returns>
    protected sealed override IEnumerable<DataRow> GetChildRows(DataRow row)
    {
        if (!row.Table.ChildRelations.Contains(this.RelationName))
        {
            throw new ArgumentException($"Unable to find the {this.RelationName} relation.");
        }

        return row.GetChildRows(this.RelationName);
    }
}

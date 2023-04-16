namespace PiraSoft.Tools.Data.Mapping;

/// <summary>
/// Type of column presence and valorization check.
/// </summary>
public enum ColumnsCheckLogic
{
    /// <summary>
    /// All columns must be pesent and must have a not null value.
    /// </summary>
    All,
    /// <summary>
    /// At least one column must be present and must have a not null value.
    /// </summary>
    Any
}
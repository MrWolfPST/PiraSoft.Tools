namespace PiraSoft.Tools.Core.Collections;

/// <summary>
/// Provides the class for a collection whose keys are embedded in the values.
/// </summary>
/// <typeparam name="TKey">The type of keys in the collection.</typeparam>
/// <typeparam name="TItem">The type of items in the collection.</typeparam>
public sealed class KeyedCollection<TKey, TItem> : System.Collections.ObjectModel.KeyedCollection<TKey, TItem>
    where TKey : notnull
{
    private const int DefaultThreshold = 0;

    private readonly Func<TItem, TKey> _getKeyForItemDelegate;

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyedCollection{TKey, TItem}"/> class that uses the default equality comparer.
    /// </summary>
    /// <param name="getKeyForItemDelegate">Extracts the key from the specified element.</param>
    /// <exception cref="ArgumentNullException"><paramref name="getKeyForItemDelegate"/> is null</exception>
    public KeyedCollection(Func<TItem, TKey> getKeyForItemDelegate) : this(getKeyForItemDelegate, null)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyedCollection{TKey, TItem}"/> class that uses the default equality comparer.
    /// </summary>
    /// <param name="getKeyForItemDelegate">Extracts the key from the specified element.</param>
    /// <param name="comparer">The implementation of the <see cref="IEqualityComparer{T}"/> generic interface to use when comparing keys, or null to use the default equality comparer for the type of the key, obtained from <see cref="EqualityComparer{T}.Default"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="getKeyForItemDelegate"/> is null</exception>
    public KeyedCollection(Func<TItem, TKey> getKeyForItemDelegate, IEqualityComparer<TKey>? comparer) : this(getKeyForItemDelegate, comparer, DefaultThreshold)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyedCollection{TKey, TItem}"/> class that uses the default equality comparer.
    /// </summary>
    /// <param name="getKeyForItemDelegate">Extracts the key from the specified element.</param>
    /// <param name="comparer">The implementation of the <see cref="IEqualityComparer{T}"/> generic interface to use when comparing keys, or null to use the default equality comparer for the type of the key, obtained from <see cref="EqualityComparer{T}.Default"/>.</param>
    /// <param name="dictionaryCreationThreshold">The number of elements the collection can hold without creating a lookup dictionary (0 creates the lookup dictionary when the first item is added), or -1 to specify that a lookup dictionary is never created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="getKeyForItemDelegate"/> is null</exception>
    /// <exception cref="ArgumentOutOfRangeException">dictionaryCreationThreshold is less than -1.</exception>
    public KeyedCollection(Func<TItem, TKey> getKeyForItemDelegate, IEqualityComparer<TKey>? comparer, int dictionaryCreationThreshold) : base(comparer, dictionaryCreationThreshold)
        => _getKeyForItemDelegate = getKeyForItemDelegate ?? throw new ArgumentNullException(nameof(getKeyForItemDelegate));

    /// <summary>
    /// When implemented in a derived class, extracts the key from the specified element.
    /// </summary>
    /// <param name="item">The element from which to extract the key.</param>
    /// <returns>The key for the specified element.</returns>
    protected sealed override TKey GetKeyForItem(TItem item)
        => _getKeyForItemDelegate(item);
}

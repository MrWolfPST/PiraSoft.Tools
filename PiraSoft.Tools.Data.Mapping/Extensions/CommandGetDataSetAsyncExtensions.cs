using PiraSoft.Tools.Data.Mapping;
using System.Data;
using System.Data.Common;

namespace PiraSoft.Tools.Data;

/// <summary>
/// A set of <see cref="Command{TDataReader, TParameter}"/> extension methods that use <see cref="Mapper"/>.
/// </summary>
public static class CommandGetDataSetAsyncExtensions
{
    /// <summary>
    /// This is the asynchronous version of <see cref="CommandGetDataSetExtensions.List{TDataReader, TParameter}(Command{TDataReader, TParameter}, Func{object})"/>
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="factory">Method used for get instance of returning object.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task<IEnumerable<object>> ListAsync<TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<object> factory, CancellationToken cancellationToken = default)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ListAsync(factory, null, cancellationToken);

    /// <summary>
    /// This is the asynchronous version of <see cref="CommandGetDataSetExtensions.List{TDataReader, TParameter}(Command{TDataReader, TParameter}, Func{object}, TypeMappings?)"/>
    /// </summary>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="factory">Method used for get instance of returning object.</param>
    /// <param name="mappings">Type mappings informations.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task<IEnumerable<object>> ListAsync<TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<object> factory, TypeMappings? mappings, CancellationToken cancellationToken = default)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => from r in (await target.GetDataTableAsync(cancellationToken)).AsEnumerable() select Mapper.Map(factory, r, mappings);

    /// <summary>
    /// This is the asynchronous version of <see cref="CommandGetDataSetExtensions.List{T, TDataReader, TParameter}(Command{TDataReader, TParameter})"/>
    /// </summary>
    /// <typeparam name="T">Type of the object to return. The type must be implement a parameterless constructor.</typeparam>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task<IEnumerable<T>> ListAsync<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target, CancellationToken cancellationToken = default)
        where T : new()
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ListAsync(() => new T(), null, cancellationToken);

    /// <summary>
    /// This is the asynchronous version of <see cref="CommandGetDataSetExtensions.List{T, TDataReader, TParameter}(Command{TDataReader, TParameter}, TypeMappings?)"/>
    /// </summary>
    /// <typeparam name="T">Type of the object to return. The type must be implement a parameterless constructor.</typeparam>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="mappings">Type mappings informations.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task<IEnumerable<T>> ListAsync<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target, TypeMappings? mappings, CancellationToken cancellationToken = default)
        where T : new()
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ListAsync(() => new T(), mappings, cancellationToken);

    /// <summary>
    /// This is the asynchronous version of <see cref="CommandGetDataSetExtensions.List{T, TDataReader, TParameter}(Command{TDataReader, TParameter}, Func{T})"/>
    /// </summary>
    /// <typeparam name="T">Type of the object to return.</typeparam>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="factory">Method used for get instance of returning object.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static Task<IEnumerable<T>> ListAsync<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<T> factory, CancellationToken cancellationToken = default)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => target.ListAsync(factory, null, cancellationToken);

    /// <summary>
    /// This is the asynchronous version of <see cref="CommandGetDataSetExtensions.List{T, TDataReader, TParameter}(Command{TDataReader, TParameter}, Func{T}, TypeMappings?)"/>
    /// </summary>
    /// <typeparam name="T">Type of the object to return.</typeparam>
    /// <typeparam name="TDataReader">The type of data reader.</typeparam>
    /// <typeparam name="TParameter">The type of parameters.</typeparam>
    /// <param name="target">Object used for interact with database.</param>
    /// <param name="factory">Method used for get instance of returning object.</param>
    /// <param name="mappings">Type mappings informations.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task<IEnumerable<T>> ListAsync<T, TDataReader, TParameter>(this Command<TDataReader, TParameter> target, Func<T> factory, TypeMappings? mappings, CancellationToken cancellationToken = default)
        where TDataReader : DbDataReader
        where TParameter : DbParameter
        => from r in (await target.GetDataTableAsync(cancellationToken)).AsEnumerable() select Mapper.Map(factory, target.GetDataRow(), mappings);
}

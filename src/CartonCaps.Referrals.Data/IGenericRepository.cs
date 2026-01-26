using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace CartonCaps.Referrals.Data;

/// <summary>
/// Provides a generic repository for querying entities of a specified type from the database context.
/// </summary>
/// <typeparam name="EntityType">The type of one of the Model classes.</typeparam>
/// <param name="context">The database context.</param>
public interface IGenericRepository<EntityType> where EntityType : class
{
    /// <summary>
    /// Gets the queryable collection of entities of type <typeparamref name="EntityType"/>.
    /// </summary>
    IQueryable<EntityType> DbSet { get; }

    /// <summary>
    /// Insert and entity into the database. Needs to call Save to persist the changes.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task Insert(EntityType entity);

    /// <summary>
    /// Updates entities in bulk based on the specified filter and property setters.
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="propertySetter"></param>
    /// <returns>Total number of rows updated in the database</returns>
    Task<int> BulkUpdate(Expression<Func<EntityType, bool>> filter, Action<UpdateSettersBuilder<EntityType>> propertySetter);

    /// <summary>
    /// Asynchronously saves the current changes to the underlying data store.
    /// </summary>
    Task Save();

    /// <summary>
    /// Database count.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the query.</typeparam>
    /// <param name="query">The query whose elements are to be counted.</param>
    /// <returns>Returns the number of elements in the specified query.</returns>
    Task<int> Count<T>(IQueryable<T> query);

    /// <summary>
    /// Databse query.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the query.</typeparam>
    /// <param name="query">The query element.</param>
    /// <returns>A list with the elements from the result of the execution of the query.</returns>
    Task<List<T>> ToList<T>(IQueryable<T> query);
}

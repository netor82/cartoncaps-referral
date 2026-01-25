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
    /// Database count.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the query.</typeparam>
    /// <param name="query">The query whose elements are to be counted.</param>
    /// <returns>Returns the number of elements in the specified query.</returns>
    Task<int> CountAsync<T>(IQueryable<T> query);
    /// <summary>
    /// Databse query.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the query.</typeparam>
    /// <param name="query">The query element.</param>
    /// <returns>A list with the elements from the result of the execution of the query.</returns>
    Task<List<T>> ToListAsync<T>(IQueryable<T> query);
}

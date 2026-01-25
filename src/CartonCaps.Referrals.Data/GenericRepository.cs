using Microsoft.EntityFrameworkCore;

namespace CartonCaps.Referrals.Data;

/// <inheritdoc/>
public class GenericRepository<EntityType>(ReferralDbContext context) : IGenericRepository<EntityType> where EntityType : class
{
    private readonly DbSet<EntityType> dbSet = context.Set<EntityType>();
    public IQueryable<EntityType> DbSet => dbSet;

    /// <inheritdoc/>
    public async Task<int> CountAsync<T>(IQueryable<T> query)
    {
        return await query.CountAsync();
    }

    /// <inheritdoc/>
    public async Task<List<T>> ToListAsync<T>(IQueryable<T> query)
    {
        return await query.ToListAsync();
    }
}



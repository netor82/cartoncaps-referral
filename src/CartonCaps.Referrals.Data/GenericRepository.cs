using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace CartonCaps.Referrals.Data;

/// <inheritdoc/>
public class GenericRepository<EntityType>(ReferralDbContext context) : IGenericRepository<EntityType> where EntityType : class
{
    private readonly DbSet<EntityType> dbSet = context.Set<EntityType>();
    public IQueryable<EntityType> DbSet => dbSet;

    /// <inheritdoc/>
    public async Task<int> Count<T>(IQueryable<T> query)
    {
        return await query.CountAsync();
    }

    /// <inheritdoc/>
    public async Task<List<T>> ToList<T>(IQueryable<T> query)
    {
        return await query.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task Insert(EntityType entity)
    {
        await dbSet.AddAsync(entity);
    }

    /// <inheritdoc/>
    public async Task Save()
    {
        await context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task<int> BulkUpdate(Expression<Func<EntityType, bool>> filter, Expression<Func<SetPropertyCalls<EntityType>, SetPropertyCalls<EntityType>>> propertySetter)
    {
        var result = await dbSet.Where(filter).ExecuteUpdateAsync(propertySetter);
        return result;
    }
}



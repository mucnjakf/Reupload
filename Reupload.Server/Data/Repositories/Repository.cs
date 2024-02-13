using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Reupload.Server.Data.Repositories.Contracts;

namespace Reupload.Server.Data.Repositories;

public class Repository<TContext, TEntity> : IRepository<TEntity> where TContext : DbContext where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet;

    protected TContext DbContext { get; }

    public Repository(TContext dbContext)
    {
        DbContext = dbContext;
        _dbSet = DbContext.Set<TEntity>();
    }

    public async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>>? expression)
    {
        IQueryable<TEntity> query = _dbSet;

        if (expression is not null)
        {
            query = query.Where(expression);
        }

        return await query.ToListAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }
}
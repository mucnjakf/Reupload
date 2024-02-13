using System.Linq.Expressions;

namespace Reupload.Server.Data.Repositories.Contracts;

public interface IRepository<TEntity> where TEntity : class
{
    Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>>? expression = default!);

    Task AddAsync(TEntity entity);

    void Remove(TEntity entity);
}
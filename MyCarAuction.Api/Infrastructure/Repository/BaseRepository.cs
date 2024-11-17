
using Microsoft.EntityFrameworkCore;
using MyCarAuction.Api.Infrastructure.Data;

namespace MyCarAuction.Api.Infrastructure.Repository;

internal class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly IDbContextFactory<ApiDbContext> _dbContextFactory;

    public IDbContextFactory<ApiDbContext> DbContextFactory => _dbContextFactory;

    public BaseRepository(IDbContextFactory<ApiDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    public async Task<T> Get(Guid id, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        var entity = await dbContext.Set<T>().FindAsync([id], cancellationToken);
        return entity;
    }

    public async Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.Set<T>().ToListAsync(cancellationToken);
    }

    public async Task<T> Create(T entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        await dbContext.Set<T>().AddAsync(entity, cancellationToken);
        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result > 0 ? entity : throw new InvalidOperationException("The create operation did not affect any rows in the database.");
    }

    public async Task<T> Update(T entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        dbContext.Set<T>().Update(entity);
        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result > 0 ? entity : throw new InvalidOperationException("The update operation did not affect any rows in the database.");
    }

    public async Task<bool> Delete(T entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        dbContext.Set<T>().Remove(entity);
        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result > 0 ? true : throw new InvalidOperationException("The delete operation did not affect any rows in the database.");
    }
}

namespace MyCarAuctionAPI.Infrastructure.Repository
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> Get(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken);
        Task<T> Create(T entity, CancellationToken cancellationToken);
        Task<T> Update(T entity, CancellationToken cancellationToken);
        Task<bool> Delete(T entity, CancellationToken cancellationToken);
    }
}

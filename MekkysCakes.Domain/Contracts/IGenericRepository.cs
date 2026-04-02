using MekkysCakes.Domain.Entities;

namespace MekkysCakes.Domain.Contracts
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Task AddAsync(TEntity entity);
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<TEntity?> GetByIdAsync(ISpecification<TEntity, TKey> specification);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> specification);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<int> CountAsync(ISpecification<TEntity, TKey> specification);
    }
}

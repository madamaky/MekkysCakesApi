using MekkysCakes.Domain.Entities;

namespace MekkysCakes.Domain.Contracts
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
        Task<bool> SaveChangesAsync();
    }
}

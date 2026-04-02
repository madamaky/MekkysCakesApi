using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities;
using MekkysCakes.Persistence.Data.DbContexts;

namespace MekkysCakes.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = [];

        public UnitOfWork(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var entityType = typeof(TEntity);

            if (_repositories.TryGetValue(entityType, out var repository))
                return (IGenericRepository<TEntity, TKey>)repository;

            var newRepository = new GenericRepository<TEntity, TKey>(_dbContext);
            _repositories[entityType] = newRepository;

            return newRepository;
        }

        public async Task<bool> SaveChangesAsync() => await _dbContext.SaveChangesAsync() > 0;
    }
}

using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities;
using MekkysCakes.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace MekkysCakes.Persistence.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext _dbContext;

        public GenericRepository(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(TEntity entity) => await _dbContext.Set<TEntity>().AddAsync(entity);

        public async Task<TEntity?> GetByIdAsync(TKey id) => await _dbContext.Set<TEntity>().FindAsync(id);
        public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity, TKey> specification)
            => await SpecificationsEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specification).FirstOrDefaultAsync();

        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbContext.Set<TEntity>().ToListAsync();
        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> specification)
            => await SpecificationsEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specification).ToListAsync();

        public void Update(TEntity entity) => _dbContext.Set<TEntity>().Update(entity);
        public void Delete(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);

        public async Task<int> CountAsync(ISpecification<TEntity, TKey> specification)
            => await SpecificationsEvaluator.CreateQuery(_dbContext.Set<TEntity>(), specification).CountAsync();
    }
}

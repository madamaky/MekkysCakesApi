using System.Linq.Expressions;
using MekkysCakes.Domain.Entities;

namespace MekkysCakes.Domain.Contracts
{
    public interface ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public Expression<Func<TEntity, bool>> Criteria { get; }
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; }

        public List<Func<IQueryable<TEntity>, IQueryable<TEntity>>> ThenIncludeExpressions { get; }

        public Expression<Func<TEntity, object>> OrderBy { get; }
        public Expression<Func<TEntity, object>> OrderByDescending { get; }
        public int Take { get; }
        public int Skip { get; }
        public bool IsPaginated { get; }

    }
}

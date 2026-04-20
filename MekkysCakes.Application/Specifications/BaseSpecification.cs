using System.Linq.Expressions;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities;

namespace MekkysCakes.Application.Specifications
{
    public abstract class BaseSpecification<TEntity, TKey> : ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public Expression<Func<TEntity, bool>> Criteria { get; }
        protected BaseSpecification(Expression<Func<TEntity, bool>> criteria) => Criteria = criteria;

        public ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];
        protected void AddInclude(Expression<Func<TEntity, object>> includeExp) => IncludeExpressions.Add(includeExp);



        public List<Func<IQueryable<TEntity>, IQueryable<TEntity>>> ThenIncludeExpressions { get; } = new();
        protected void AddThenInclude(Func<IQueryable<TEntity>, IQueryable<TEntity>> thenIncludeExp) => ThenIncludeExpressions.Add(thenIncludeExp);



        public Expression<Func<TEntity, object>> OrderBy { get; private set; }
        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExp) => OrderBy = orderByExp;

        public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }
        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescExp) => OrderByDescending = orderByDescExp;

        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPaginated { get; private set; }
        protected void ApplyPagination(int pageSize, int pageIndex)
        {
            IsPaginated = true;
            Take = pageSize;
            Skip = pageSize * (pageIndex - 1);
        }
    }
}

using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MekkysCakes.Persistence
{
    internal static class SpecificationsEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(
            IQueryable<TEntity> entryPoint,
            ISpecification<TEntity, TKey> specification) where TEntity : BaseEntity<TKey>
        {
            var query = entryPoint;

            if (specification is not null)
            {
                if (specification.Criteria is not null)
                    query = query.Where(specification.Criteria);

                if (specification.IncludeExpressions is not null && specification.IncludeExpressions.Any())
                    query = specification.IncludeExpressions.Aggregate(query, (currentQuery, includeExp) => currentQuery.Include(includeExp));



                if (specification.ThenIncludeExpressions is not null && specification.ThenIncludeExpressions.Any())
                    query = specification.ThenIncludeExpressions.Aggregate(query, (currentQuery, includeExp) => includeExp(currentQuery));



                if (specification.OrderBy is not null)
                    query = query.OrderBy(specification.OrderBy);

                if (specification.OrderByDescending is not null)
                    query = query.OrderByDescending(specification.OrderByDescending);

                if (specification.IsPaginated)
                    query = query.Skip(specification.Skip).Take(specification.Take);
            }

            return query;
        }
    }
}

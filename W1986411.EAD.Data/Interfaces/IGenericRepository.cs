using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace W1986411.EAD.Data;

public interface IGenericRepository<T> where T : class
{
    /// <summary>
    /// Adds the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>Returns add entity.</returns>
    Task<EntityEntry<T>> Add(T entity);

    /// <summary>
    /// Finds the by identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Returns find entity.</returns>
    Task<T> FindById(int id);

    /// <summary>
    /// Deletes the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Returns delete entity.</returns>
    EntityEntry<T> Delete(int id);

    /// <summary>
    /// Updates the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>Returns update entity.</returns>
    EntityEntry<T> Update(T entity);

    /// <summary>
    /// Gets the queryable.
    /// </summary>
    /// <param name="predicate">The predicate.</param>
    /// <param name="orderBy">The order by.</param>
    /// <returns>Returns list of entities.</returns>
    IQueryable<T> GetQueryable(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);

    /// <summary>
    /// Gets the one.
    /// </summary>
    /// <param name="predicate">The predicate.</param>
    /// <returns>Returns one entity.</returns>
    Task<T> GetOneAsync(Expression<Func<T, bool>> predicate);
}

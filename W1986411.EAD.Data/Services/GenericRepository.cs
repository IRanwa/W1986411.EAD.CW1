using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace W1986411.EAD.Data;

/// <summary>
/// Generic repository.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="IGenericRepository&lt;T&gt;" />
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    /// <summary>
    /// The unit of work
    /// </summary>
    private readonly IUnitOfWork unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericRepository{T}"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    public GenericRepository(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Adds the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>Returns add entity.</returns>
    public async Task<EntityEntry<T>> Add(T entity)
    {
        return await unitOfWork.GetDBContext().Set<T>().AddAsync(entity);
    }

    /// <summary>
    /// Finds the by identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Returns find entity.</returns>
    public async Task<T> FindById(int id)
    {
        return await unitOfWork.GetDBContext().Set<T>().FindAsync(id);
    }

    /// <summary>
    /// Deletes the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Returns delete entity.</returns>
    public EntityEntry<T> Delete(int id)
    {
        var entity = FindById(id).Result;
        if (entity != null)
            return unitOfWork.GetDBContext().Set<T>().Remove(entity);
        return null;
    }

    /// <summary>
    /// Updates the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>Returns update entity.</returns>
    public EntityEntry<T> Update(T entity)
    {
        return unitOfWork.GetDBContext().Set<T>().Update(entity);
    }

    /// <summary>
    /// Gets the queryable.
    /// </summary>
    /// <param name="predicate">The predicate.</param>
    /// <param name="orderBy">The order by.</param>
    /// <returns>Returns list of entities.</returns>
    public IQueryable<T> GetQueryable(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
    {
        IQueryable<T> query = unitOfWork.GetDBContext().Set<T>();
        if (predicate != null)
            query = query.Where(predicate);

        if (orderBy != null)
            query = orderBy(query);

        return query.AsQueryable();
    }

    /// <summary>
    /// Gets the one.
    /// </summary>
    /// <param name="predicate">The predicate.</param>
    /// <returns>Returns one entity.</returns>
    public async Task<T> GetOneAsync(Expression<Func<T, bool>> predicate)
    {
        IQueryable<T> query = unitOfWork.GetDBContext().Set<T>();
        if (predicate != null)
            query = query.Where(predicate);
        return await query.FirstOrDefaultAsync();
    }
}

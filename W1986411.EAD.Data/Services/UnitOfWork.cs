using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace W1986411.EAD.Data;

/// <summary>
/// Unit of work.
/// </summary>
/// <seealso cref="IUnitOfWork" />
public class UnitOfWork : IUnitOfWork
{
    /// <summary>
    /// The context
    /// </summary>
    private readonly APIDBContext context;

    /// <summary>
    /// The repositories asynchronous
    /// </summary>
    private Dictionary<Type, object> repositoriesAsync;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="user">The user.</param>
    public UnitOfWork(APIDBContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Gets the generic repository.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>Returns generic repo.</returns>
    public IGenericRepository<T> GetGenericRepository<T>() where T : class
    {
        if (repositoriesAsync == null)
            repositoriesAsync = new Dictionary<Type, object>();
        var type = typeof(T);
        if (!repositoriesAsync.ContainsKey(type))
            repositoriesAsync.Add(type, new GenericRepository<T>(this));
        return (IGenericRepository<T>)repositoriesAsync[type];
    }

    /// <summary>
    /// Saves the changes.
    /// </summary>
    /// <returns>Returns saved count.</returns>
    public int SaveChanges(IPrincipal user)
    {
        var addingEntries = context.ChangeTracker.Entries().Where(entry =>
            entry.Entity is EntityBase &&
            entry.State == EntityState.Added).ToList();
        foreach (var entry in addingEntries)
        {
            ((EntityBase)entry.Entity).CreatedDateTime = DateTime.UtcNow;
            ((EntityBase)entry.Entity).CreatedUser = user.Identity == null ? null : user.Identity?.Name;
        }

        var updatingEntries = context.ChangeTracker.Entries().Where(entry =>
            entry.Entity is EntityBase &&
            entry.State == EntityState.Modified ||
            entry.State == EntityState.Deleted).ToList();
        foreach (var entry in updatingEntries)
        {
            ((EntityBase)entry.Entity).ModifiedDateTime = DateTime.UtcNow;
            ((EntityBase)entry.Entity).ModifiedUser = user.Identity == null ? null : user.Identity?.Name;
        }
        return context.SaveChanges();
    }

    /// <summary>
    /// Gets the database context.
    /// </summary>
    /// <returns>Returns db context.</returns>
    public APIDBContext GetDBContext()
    {
        return context;
    }
}

namespace W1986411.EAD.Data;

/// <summary>
/// Unit of work.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Gets the generic repository.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>Returns generic repo.</returns>
    IGenericRepository<T> GetGenericRepository<T>() where T : class;

    /// <summary>
    /// Saves the changes.
    /// </summary>
    /// <returns>Returns saved count.</returns>
    int SaveChanges();

    /// <summary>
    /// Gets the database context.
    /// </summary>
    /// <returns>Returns db context.</returns>
    APIDBContext GetDBContext();
}

using CQRSStarter.DAL.Entities;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace CQRSStarter.DAL
{
    /// <summary>
    /// Interface for database access.
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Gets the <see cref="BlogEntry">Todos</see>.
        /// </summary>
        IDbSet<Todo> Todos { get; }

        DbSet<T> Set<T>() where T : class;

        DbEntityEntry Entry(object entity);

        /// <summary>
        /// Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous save operation.
        /// The task result contains the number of objects written to the underlying database.
        /// </returns>
        /// <remarks>
        /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        /// that any asynchronous operations have completed before calling another method on this context.
        /// </remarks>
        Task<int> SaveChangesAsync();
    }
}

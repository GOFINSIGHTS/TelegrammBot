using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegrammBot.Domain.Abstractions;
using TelegrammBot.Services.Abstractions.Interfaces;
using TelegrammBot.Services.Abstractions.Repositories.Interfaces.Abstractions;

namespace TelegrammBot.Infrastructure.Implementation.Repositories.Abstractions
{
    /// <summary>
    /// Base repository for reading and writing entities of type T.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public abstract class BaseRepository<T> : IRepository<T>
        where T : BaseEntity
    {
        protected readonly IApplicationContext _context;
        protected readonly DbSet<T> _entitySet;

        /// <summary>
        /// Initializes a new instance of the BaseRepository class.
        /// </summary>
        /// <param name="context">The application context.</param>
        protected BaseRepository(IApplicationContext context)
        {
            _context = context;
            _entitySet = _context.Set<T>();
        }

        /// <inheritdoc />
        public virtual T Add(T item)
        {
            return _entitySet.Add(item).Entity;
        }

        /// <inheritdoc />
        public virtual void Delete(T item)
        {
            item.Deleted = true;
            _entitySet.Update(item);
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entitySet.AsNoTracking().ToListAsync();
        }

        /// <inheritdoc />
        public virtual async Task<T?> GetByIdAsync(Guid id, bool noTracking = false)
        {
            return noTracking ? await _entitySet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id) :
                await _entitySet.FindAsync(id);
        }

        /// <inheritdoc />
        public virtual void Update(T item)
        {
            _entitySet.Update(item);
        }

        /// <summary>
        /// Saves changes asynchronously to the underlying context.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
        /// <returns>A task representing the asynchronous save operation.</returns>
        public virtual async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

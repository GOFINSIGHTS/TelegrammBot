using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegrammBot.Domain.Abstractions;

namespace TelegrammBot.Services.Abstractions.Repositories.Interfaces.Abstractions
{
    /// <summary>
    /// Repository for reading and writing entities of type T.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    public interface IRepository<T>
       where T : BaseEntity
    {
        /// <summary>
        /// Retrieves all entities of type T asynchronously.
        /// </summary>
        /// <returns>An enumerable collection of entities.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Retrieves an entity of type T by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <param name="noTracking">If true, the entity will be retrieved without tracking changes.</param>
        /// <returns>The entity with the specified identifier.</returns>
        Task<T?> GetByIdAsync(Guid id, bool noTracking = false);

        /// <summary>
        /// Adds a new entity of type T to the repository.
        /// </summary>
        /// <param name="item">The entity to be added.</param>
        T Add(T item);

        /// <summary>
        /// Updates an existing entity of type T in the repository.
        /// </summary>
        /// <param name="item">The entity to be updated.</param>
        void Update(T item);

        /// <summary>
        /// Deletes an entity of type T from the repository.
        /// </summary>
        /// <param name="item">The entity to be deleted.</param>
        void Delete(T item);
    }
}

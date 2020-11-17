using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StoreManager.Library.Entity;

namespace StoreManager.Library.Database.DbSetInterfacer
{
    internal interface IDbSetInterfacer<T>
        where T : SEntity
    {
        Task<bool> Any();

        // Create
        Task CreateManyAsync(List<T> items);
        Task CreateOneAsync(T item);

        // Read
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetManyAsync(List<int> ids);
        Task<T> GetOneAsync(int id);

        // Update
        Task UpdateManyAsync(List<T> items);
        Task UpdateOneAsync(T item);

        // Delete
        Task DeleteManyAsync(List<T> items);
        Task DeleteOneAsync(T item);
    }
}

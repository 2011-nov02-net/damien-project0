using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StoreManager.Library.Data;
using StoreManager.Library.Entity;

namespace StoreManager.Library.Database.DbSetInterfacer
{
    internal interface IInterfacer { }

    internal interface IDbSetInterfacer<T> : IInterfacer
        where T : SEntity
    {
        Task<bool> Any();
        Task<bool> IdExistsAsync(int id);

        Task<int> MaxIdAsync();

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

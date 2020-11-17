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
        Task CreateSomeAsync(List<T> items);
        Task CreateOneAsync(T item);

        // Read
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetSomeAsync(List<int> ids);
        Task<T> GetOneAsync(int id);

        // Update
        Task UpdateAllAsync(List<T> items);
        Task UpdateSomeAsync(List<T> items);
        Task UpdateOneAsync(T item);

        // Delete
        Task DeleteAllAsync();
        Task DeleteSomeAsync(List<T> items);
        Task DeleteOneAsync(T item);
    }
}

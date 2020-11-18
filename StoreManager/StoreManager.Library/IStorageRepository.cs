using System.Collections.Generic;
using System.Threading.Tasks;

using StoreManager.Library.Data;
using StoreManager.Library.Entity;

namespace StoreManager.Library
{
    public interface IStorageRepository
    {
        void Configure(IConfigurationOptions configurationOptions);

        Task<bool> AnyAsync<T>() where T : SEntity;
        Task<bool> IdExistsAsync<T>(int id) where T : SEntity;
        Task<int> MaxIdAsync<T>() where T : SEntity;

        // Create
        Task CreateManyAsync<T>(List<T> entities) where T : SEntity;
        Task CreateOneAsync<T>(T entity) where T : SEntity;

        // Read
        Task<List<T>> GetAllAsync<T>() where T : SEntity;
        Task<List<T>> GetManyAsync<T>(List<int> ids) where T : SEntity;
        Task<T> GetOneAsync<T>(int id) where T : SEntity;
        
        // Update
        Task UpdateManyAsync<T>(List<T> items) where T : SEntity;
        Task UpdateOneAsync<T>(T entity) where T : SEntity;
        
        // Delete
        Task DeleteManyAsync<T>(List<T> entities) where T : SEntity;
        Task DeleteOneAsync<T>(T entity) where T : SEntity;
    }
}

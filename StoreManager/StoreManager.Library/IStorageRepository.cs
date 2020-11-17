using System.Collections.Generic;
using System.Threading.Tasks;

using StoreManager.Library.Data;
using StoreManager.Library.Entity;

namespace StoreManager.Library
{
    public interface IStorageRepository
    {
        void Configure(IConfigurationOptions configurationOptions);

        Task<DataBundle> ReadAsync();
        Task WriteAsync(DataBundle dataBundle);

        // Create
        Task CreateSomeAsync<T>(List<SEntity> entities) where T : SEntity;
        Task CreateOneAsync<T>(SEntity entity) where T : SEntity;

        // Read
        Task<List<T>> GetAllAsync<T>() where T : SEntity;
        Task<List<T>> GetSomeAsync<T>(List<int> ids) where T : SEntity;
        Task<T> GetOneAsync<T>(int id) where T : SEntity;
        
        // Update
        Task UpdateAllAsync<T>(List<SEntity> items) where T : SEntity;
        Task UpdateSomeAsync<T>(List<SEntity> items) where T : SEntity;
        Task UpdateOneAsync<T>(SEntity entity) where T : SEntity;
        
        // Delete
        Task DeleteAllAsync<T>() where T : SEntity;
        Task DeleteSomeAsync<T>(List<SEntity> entities) where T : SEntity;
        Task DeleteOneAsync<T>(SEntity entity) where T : SEntity;
    }
}

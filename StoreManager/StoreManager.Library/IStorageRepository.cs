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
        Task CreateManyAsync<T>(List<SEntity> entities) where T : SEntity;
        Task CreateOneAsync<T>(SEntity entity) where T : SEntity;

        // Read
        Task<List<T>> GetAllAsync<T>() where T : SEntity;
        Task<List<T>> GetManyAsync<T>(List<int> ids) where T : SEntity;
        Task<T> GetOneAsync<T>(int id) where T : SEntity;
        
        // Update
        Task UpdateManyAsync<T>(List<SEntity> items) where T : SEntity;
        Task UpdateOneAsync<T>(SEntity entity) where T : SEntity;
        
        // Delete
        Task DeleteManyAsync<T>(List<SEntity> entities) where T : SEntity;
        Task DeleteOneAsync<T>(SEntity entity) where T : SEntity;
    }
}

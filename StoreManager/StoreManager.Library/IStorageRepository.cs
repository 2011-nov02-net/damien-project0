using System.Collections.Generic;
using System.Threading.Tasks;

using StoreManager.Library.Data;
using StoreManager.Library.Entity;

namespace StoreManager.Library
{
    public interface IStorageRepository
    {
        void Configure(IConfigurationOptions configurationOptions);

        Task<DataBundle> Read();
        Task Write(DataBundle dataBundle);

        // Create
        Task<List<int>> CreateAll<T>(List<IData> data) where T : SEntity;
        Task<List<int>> CreateSome<T>(List<IData> data) where T : SEntity;
        Task<int> Create<T>(IData data) where T : SEntity;

        // Read
        Task<List<T>> GetAll<T>() where T : SEntity;
        Task<List<T>> GetSome<T>(List<int> ids) where T : SEntity;
        Task<T> GetOne<T>(int id) where T : SEntity;
        
        // Update
        Task UpdateAll<T>(List<T> items) where T : SEntity;
        Task UpdateSome<T>(List<T> items) where T : SEntity;
        Task UpdateOne<T>(T item) where T : SEntity;
        
        // Delete
        Task DeleteAll<T>(List<T> items) where T : SEntity;
        Task DeleteSome<T>(List<T> items) where T : SEntity;
        Task DeleteOne<T>(T item) where T : SEntity;
    }
}

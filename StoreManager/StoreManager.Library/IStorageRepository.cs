using System.Collections.Generic;
using System.Threading.Tasks;

using StoreManager.Library.Data;
using StoreManager.Library.Entity;

namespace StoreManager.Library
{
    public interface IStorageRepository
    {
        void Configure(IConfigurationOptions configurationOptions);

        // Read Operations (All)
        Task<DataBundle> Read();

        // Read Operations (Generic)
        Task<List<T>> ReadAll<T>() where T : SEntity;
        Task<List<T>> ReadSome<T>(int[] ids) where T : SEntity;
        Task<T> ReadOne<T>(int id) where T : SEntity;

        // Write Operations (All)
        Task Write(DataBundle data);

        // Write Operations (Generic)
        Task WriteAll<T>(List<T> dataItems) where T : SEntity;
        Task WriteSome<T>(int[] ids, List<T> dataItems) where T : SEntity;
        Task WriteOne<T>(int id, T item) where T : SEntity;
    }
}

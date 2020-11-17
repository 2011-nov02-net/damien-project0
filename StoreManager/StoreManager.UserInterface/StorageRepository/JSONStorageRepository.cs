using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

using StoreManager.Library;
using StoreManager.Library.Data;
using StoreManager.Library.Entity;

namespace StoreManager.UserInterface.StorageRepository
{
    class JSONStorageRepository : IStorageRepository
    {
        private string _filepath;
        public string Filepath
        {
            get => _filepath;
            protected set => _filepath = value;
        }

        public JSONStorageRepository(string filepath) {
            Filepath = filepath;
        }

        // public async Task<DataBundle> Read() {
        //     string json = File.ReadAllText(_filepath);
        //     var stores = JsonSerializer.Deserialize<DataBundle>(json);
        //     return await Task.Run(() => stores);
        // }

        public void Configure(IConfigurationOptions configurationOptions) {
            if (configurationOptions is not JSONConfigurationOptions)
                return;

            /*var configOptions */ _ = configurationOptions as JSONConfigurationOptions;
            // TODO: Set up how the configuration options will work here
        }

        public Task<DataBundle> ReadAsync() {
            throw new System.NotImplementedException();
        }

        public Task WriteAsync(DataBundle dataBundle) {
            throw new System.NotImplementedException();
        }

        public Task CreateManyAsync<T>(List<SEntity> entities) where T : SEntity {
            throw new System.NotImplementedException();
        }

        public Task CreateOneAsync<T>(SEntity entity) where T : SEntity {
            throw new System.NotImplementedException();
        }

        public Task<List<T>> GetAllAsync<T>() where T : SEntity {
            throw new System.NotImplementedException();
        }

        public Task<List<T>> GetManyAsync<T>(List<int> ids) where T : SEntity {
            throw new System.NotImplementedException();
        }

        public Task<T> GetOneAsync<T>(int id) where T : SEntity {
            throw new System.NotImplementedException();
        }

        public Task UpdateManyAsync<T>(List<SEntity> items) where T : SEntity {
            throw new System.NotImplementedException();
        }

        public Task UpdateOneAsync<T>(SEntity entity) where T : SEntity {
            throw new System.NotImplementedException();
        }

        public Task DeleteManyAsync<T>(List<SEntity> entities) where T : SEntity {
            throw new System.NotImplementedException();
        }

        public Task DeleteOneAsync<T>(SEntity entity) where T : SEntity {
            throw new System.NotImplementedException();
        }
    }
}

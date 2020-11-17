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

        public void Configure(IConfigurationOptions configurationOptions) {
            if (configurationOptions is not JSONConfigurationOptions)
                return;

            /*var configOptions */ _ = configurationOptions as JSONConfigurationOptions;
            // TODO: Set up how the configuration options will work here
        }

        public async Task<DataBundle> Read() {
            string json = File.ReadAllText(_filepath);
            var stores = JsonSerializer.Deserialize<DataBundle>(json);
            return await Task.Run(() => stores);
        }

        public async Task Write(DataBundle dataBundle) {
            await Task.Run(() => { });
        }

        public Task<List<T>> ReadAll<T>() where T : SEntity {
            throw new System.NotImplementedException();
        }

        public Task<List<T>> ReadSome<T>(int[] ids) where T : SEntity {
            throw new System.NotImplementedException();
        }

        public Task<T> ReadOne<T>(int id) where T : SEntity {
            throw new System.NotImplementedException();
        }

        public Task WriteAll<T>(List<T> dataItems) where T : SEntity {
            throw new System.NotImplementedException();
        }

        public Task WriteSome<T>(int[] ids, List<T> dataItems) where T : SEntity {
            throw new System.NotImplementedException();
        }

        public Task WriteOne<T>(int id, T item) where T : SEntity {
            throw new System.NotImplementedException();
        }
    }
}

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

        // public async Task<DataBundle> Read() {
        //     string json = File.ReadAllText(_filepath);
        //     var stores = JsonSerializer.Deserialize<DataBundle>(json);
        //     return await Task.Run(() => stores);
        // }

        public Task<DataBundle> Read() {
            throw new System.NotImplementedException();
        }

        public Task Write(DataBundle dataBundle) {
            throw new System.NotImplementedException();
        }

        public Task<List<int>> CreateAll<T>(List<IData> data) where T : SEntity {
            throw new System.NotImplementedException();
        }

        public Task<List<int>> CreateSome<T>(List<IData> data) where T : SEntity {
            throw new System.NotImplementedException();
        }

        public Task<int> Create<T>(IData data) where T : SEntity {
            throw new System.NotImplementedException();
        }

        public Task<List<T>> GetAll<T>() where T : SEntity {
            throw new System.NotImplementedException();
        }

        public Task<List<T>> GetSome<T>(List<int> ids) where T : SEntity {
            throw new System.NotImplementedException();
        }

        public Task<T> GetOne<T>(int id) where T : SEntity {
            throw new System.NotImplementedException();
        }

        public Task UpdateAll<T>(List<T> items) where T : SEntity {
            throw new System.NotImplementedException();
        }

        public Task UpdateSome<T>(List<T> items) where T : SEntity {
            throw new System.NotImplementedException();
        }

        public Task UpdateOne<T>(T item) where T : SEntity {
            throw new System.NotImplementedException();
        }

        public Task DeleteAll<T>(List<T> items) where T : SEntity {
            throw new System.NotImplementedException();
        }

        public Task DeleteSome<T>(List<T> items) where T : SEntity {
            throw new System.NotImplementedException();
        }

        public Task DeleteOne<T>(T item) where T : SEntity {
            throw new System.NotImplementedException();
        }
    }
}

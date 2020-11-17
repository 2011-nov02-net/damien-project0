using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StoreManager.Library;
using StoreManager.Library.Data;
using StoreManager.Library.Entity;

namespace StoreManager.UserInterface.StorageRepository
{
    class XMLStorageRepository : IStorageRepository
    {
        private string _filepath;
        public string Filepath
        {
            get => _filepath;
            protected set => _filepath = value;
        }

        public XMLStorageRepository(string filepath) {
            Filepath = filepath;
        }

        public void Configure(IConfigurationOptions configurationOptions) {
            if (configurationOptions is not XMLConfigurationOptions)
                return;

            /*var configOptions */ _ = configurationOptions as XMLConfigurationOptions;
            // TODO: Set up how the configuration options will work here
        }

        public Task<DataBundle> Read() {
            throw new NotImplementedException();
        }

        public Task Write(DataBundle dataBundle) {
            throw new NotImplementedException();
        }

        public Task<List<int>> CreateAll<T>(List<IData> data) where T : SEntity {
            throw new NotImplementedException();
        }

        public Task<List<int>> CreateSome<T>(List<IData> data) where T : SEntity {
            throw new NotImplementedException();
        }

        public Task<int> Create<T>(IData data) where T : SEntity {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetAll<T>() where T : SEntity {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetSome<T>(List<int> ids) where T : SEntity {
            throw new NotImplementedException();
        }

        public Task<T> GetOne<T>(int id) where T : SEntity {
            throw new NotImplementedException();
        }

        public Task UpdateAll<T>(List<T> items) where T : SEntity {
            throw new NotImplementedException();
        }

        public Task UpdateSome<T>(List<T> items) where T : SEntity {
            throw new NotImplementedException();
        }

        public Task UpdateOne<T>(T item) where T : SEntity {
            throw new NotImplementedException();
        }

        public Task DeleteAll<T>(List<T> items) where T : SEntity {
            throw new NotImplementedException();
        }

        public Task DeleteSome<T>(List<T> items) where T : SEntity {
            throw new NotImplementedException();
        }

        public Task DeleteOne<T>(T item) where T : SEntity {
            throw new NotImplementedException();
        }
    }
}

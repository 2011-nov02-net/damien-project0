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

        public Task<DataBundle> ReadAsync() {
            throw new NotImplementedException();
        }

        public Task WriteAsync(DataBundle dataBundle) {
            throw new NotImplementedException();
        }

        public Task CreateManyAsync<T>(List<SEntity> entities) where T : SEntity {
            throw new NotImplementedException();
        }

        public Task CreateOneAsync<T>(SEntity entity) where T : SEntity {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetAllAsync<T>() where T : SEntity {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetManyAsync<T>(List<int> ids) where T : SEntity {
            throw new NotImplementedException();
        }

        public Task<T> GetOneAsync<T>(int id) where T : SEntity {
            throw new NotImplementedException();
        }

        public Task UpdateManyAsync<T>(List<SEntity> items) where T : SEntity {
            throw new NotImplementedException();
        }

        public Task UpdateOneAsync<T>(SEntity entity) where T : SEntity {
            throw new NotImplementedException();
        }

        public Task DeleteManyAsync<T>(List<SEntity> entities) where T : SEntity {
            throw new NotImplementedException();
        }

        public Task DeleteOneAsync<T>(SEntity entity) where T : SEntity {
            throw new NotImplementedException();
        }
    }
}

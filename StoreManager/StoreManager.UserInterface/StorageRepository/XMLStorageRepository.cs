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

        public async Task<DataBundle> Read() {
            return await Task.Run(() => new DataBundle());
        }

        public async Task Write(DataBundle dataBundle) {
            await Task.Run(() => { });
        }

        public Task<List<T>> ReadAll<T>() where T : SEntity {
            throw new NotImplementedException();
        }

        public Task<List<T>> ReadSome<T>(int[] ids) where T : SEntity {
            throw new NotImplementedException();
        }

        public Task<T> ReadOne<T>(int id) where T : SEntity {
            throw new NotImplementedException();
        }

        public Task WriteAll<T>(List<T> dataItems) where T : SEntity {
            throw new NotImplementedException();
        }

        public Task WriteSome<T>(int[] ids, List<T> dataItems) where T : SEntity {
            throw new NotImplementedException();
        }

        public Task WriteOne<T>(int id, T item) where T : SEntity {
            throw new NotImplementedException();
        }
    }
}

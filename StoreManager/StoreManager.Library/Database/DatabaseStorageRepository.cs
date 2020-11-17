using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using StoreManager.DataModel;
using StoreManager.Library.Data;
using StoreManager.Library.Entity;

namespace StoreManager.Library.Database
{
    public class DatabaseStorageRepository : IStorageRepository
    {
        private DbSetInterfacerManager _interfacerManager;
        public void Configure(IConfigurationOptions configurationOptions) {
            if (configurationOptions is not DatabaseConfigurationOptions)
                throw new ArgumentException($"Invalid IConfigurationsOptions argument; got '{configurationOptions.GetType()}' instead.");

            var configOptions = configurationOptions as DatabaseConfigurationOptions;
            var optionsBuilder = new DbContextOptionsBuilder<StoreManagerContext>();
            optionsBuilder.UseSqlServer(configOptions.ConnectionString);
            // TODO: Set up the logging structure here

            var contextOptions = optionsBuilder.Options;
            _interfacerManager = new DbSetInterfacerManager(contextOptions);
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

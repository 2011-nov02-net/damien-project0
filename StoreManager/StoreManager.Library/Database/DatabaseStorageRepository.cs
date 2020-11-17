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

        public async Task<DataBundle> ReadAsync() {
            return await Task.Run(() => new DataBundle());
        }

        public async Task WriteAsync(DataBundle dataBundle) {
            await Task.Run(() => { });
        }

        public async Task CreateSomeAsync<T>(List<SEntity> entities) where T : SEntity {
            await _interfacerManager.CreateSomeAsync<T>(entities);
        }

        public async Task CreateOneAsync<T>(SEntity entity) where T : SEntity {
            await _interfacerManager.CreateAsync<T>(entity);
        }

        public async Task<List<T>> GetAllAsync<T>() where T : SEntity {
            return await Task.Run(() => _interfacerManager.GetAllAsync<T>().Result.ConvertAll(t => t as T));
        }

        public async Task<List<T>> GetSomeAsync<T>(List<int> ids) where T : SEntity {
            return await Task.Run(() => _interfacerManager.GetSomeAsync<T>(ids).Result.ConvertAll(t => t as T));
        }

        public async Task<T> GetOneAsync<T>(int id) where T : SEntity {
            return await _interfacerManager.GetOneAsync<T>(id) as T;
        }

        public async Task UpdateAllAsync<T>(List<SEntity> items) where T : SEntity {
            await _interfacerManager.UpdateAllAsync<T>(items);
        }

        public async Task UpdateSomeAsync<T>(List<SEntity> items) where T : SEntity {
            await _interfacerManager.UpdateSomeAsync<T>(items);
        }

        public async Task UpdateOneAsync<T>(SEntity entity) where T : SEntity {
            await _interfacerManager.UpdateOneAsync<T>(entity);
        }

        public async Task DeleteAllAsync<T>() where T : SEntity {
            await _interfacerManager.DeleteAllAsync<T>();
        }

        public async Task DeleteSomeAsync<T>(List<SEntity> entities) where T : SEntity {
            await _interfacerManager.DeleteSomeAsync<T>(entities);
        }

        public async Task DeleteOneAsync<T>(SEntity entity) where T : SEntity {
            await _interfacerManager.DeleteOneAsync<T>(entity);
        }
    }
}

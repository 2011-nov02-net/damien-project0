using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using StoreManagerContext = StoreManager.DataModel.StoreManagerContext;

using StoreManager.Library.Data;
using StoreManager.Library.Entity;
using Microsoft.Extensions.Logging;

namespace StoreManager.Library.Database
{
    public class DatabaseStorageRepository : IStorageRepository, ISerializer
    {
        private DbSetInterfacerManager _interfacerManager;
        public void Configure(IConfigurationOptions configurationOptions) {
            if (configurationOptions is not DatabaseConfigurationOptions)
                throw new ArgumentException($"Invalid IConfigurationsOptions argument; got '{configurationOptions.GetType()}' instead.");

            var configOptions = configurationOptions as DatabaseConfigurationOptions;
            var optionsBuilder = new DbContextOptionsBuilder<StoreManagerContext>();
            optionsBuilder.UseSqlServer(configOptions.ConnectionString);
            optionsBuilder.LogTo(configOptions.Logger.Log, LogLevel.Information);

            var contextOptions = optionsBuilder.Options;
            _interfacerManager = new DbSetInterfacerManager(contextOptions);
        }

        public async Task<DataBundle> ReadAsync() {
            var stores = GetAllAsync<Store>().Result
                .ConvertAll(s => s.Data);
            var orders = GetAllAsync<Order>().Result
                .ConvertAll(o => o.Data);
            var customers = GetAllAsync<Customer>().Result
                .ConvertAll(c => c.Data);
            var addresses = GetAllAsync<Address>().Result
                .ConvertAll(a => a.Data);
            var operatingLocations = GetAllAsync<OperatingLocation>().Result
                .ConvertAll(ol => ol.Data);
            var products = GetAllAsync<Product>().Result
                .ConvertAll(p => p.Data);

            return await Task.Run(() => new DataBundle(stores, orders, customers, addresses, operatingLocations, products));
        }

        public async Task WriteAsync(DataBundle dataBundle) {
            // This is unneccesary since the data is persisted as it's written.
            await Task.Run(() => { });
        }

        public async Task CreateManyAsync<T>(List<T> entities) where T : SEntity {
            await _interfacerManager.CreateManyAsync<T>(entities);
        }

        public async Task CreateOneAsync<T>(T entity) where T : SEntity {
            await _interfacerManager.CreateOneAsync<T>(entity);
        }

        public async Task<List<T>> GetAllAsync<T>() where T : SEntity {
            List<T> result = null;

            if (_interfacerManager.AnyAsync<T>().Result) {
                result = (await _interfacerManager.GetAllAsync<T>())
                    .ConvertAll(t => t as T);
            }

            return await Task.Run(() => result ?? new List<T>());
        }

        public async Task<List<T>> GetManyAsync<T>(List<int> ids) where T : SEntity {
            List<T> result = null;

            if (_interfacerManager.AnyAsync<T>().Result) {
                result = (await _interfacerManager.GetSomeAsync<T>(ids))
                    .ConvertAll(t => t as T);
            }

            return await Task.Run(() => result ?? new List<T>());
        }

        public async Task<T> GetOneAsync<T>(int id) where T : SEntity {
            T result = null;

            if (_interfacerManager.AnyAsync<T>().Result)
                result = await _interfacerManager.GetOneAsync<T>(id) as T;

            return result;
        }

        public async Task UpdateManyAsync<T>(List<T> items) where T : SEntity {
            if (_interfacerManager.AnyAsync<T>().Result) {
                await _interfacerManager.UpdateManyAsync<T>(items);
            } else {
                await _interfacerManager.CreateManyAsync<T>(items);
            }
        }

        public async Task UpdateOneAsync<T>(T entity) where T : SEntity {
            if (_interfacerManager.AnyAsync<T>().Result) {
                await _interfacerManager.UpdateOneAsync<T>(entity);
            } else {
                await CreateOneAsync<T>(entity);
            }
        }

        public async Task DeleteManyAsync<T>(List<T> entities) where T : SEntity {
            if (_interfacerManager.AnyAsync<T>().Result) {
                await _interfacerManager.DeleteSomeAsync<T>(entities);
            }
        }

        public async Task DeleteOneAsync<T>(T entity) where T : SEntity {
            if (_interfacerManager.AnyAsync<T>().Result) {
                await _interfacerManager.DeleteOneAsync<T>(entity);
            }
        }
    }
}

﻿using System;
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
            DataBundle result = null;
            return await Task.Run(() => result);
        }

        public async Task WriteAsync(DataBundle dataBundle) {
            await Task.Run(() => { });
        }

        public async Task CreateManyAsync<T>(List<SEntity> entities) where T : SEntity {
            await _interfacerManager.CreateManyAsync<T>(entities);
        }

        public async Task CreateOneAsync<T>(SEntity entity) where T : SEntity {
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

        public async Task UpdateManyAsync<T>(List<SEntity> items) where T : SEntity {
            if (_interfacerManager.AnyAsync<T>().Result) {
                await _interfacerManager.UpdateManyAsync<T>(items);
            } else {
                await _interfacerManager.CreateManyAsync<T>(items);
            }
        }

        public async Task UpdateOneAsync<T>(SEntity entity) where T : SEntity {
            if (_interfacerManager.AnyAsync<T>().Result) {
                await _interfacerManager.UpdateOneAsync<T>(entity);
            } else {
                await CreateOneAsync<T>(entity);
            }
        }

        public async Task DeleteManyAsync<T>(List<SEntity> entities) where T : SEntity {
            if (_interfacerManager.AnyAsync<T>().Result) {
                await _interfacerManager.DeleteSomeAsync<T>(entities);
            }
        }

        public async Task DeleteOneAsync<T>(SEntity entity) where T : SEntity {
            if (_interfacerManager.AnyAsync<T>().Result) {
                await _interfacerManager.DeleteOneAsync<T>(entity);
            }
        }
    }
}

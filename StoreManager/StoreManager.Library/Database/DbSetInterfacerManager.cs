﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using StoreManagerContext = StoreManager.DataModel.StoreManagerContext;

using StoreManager.Library.Database.DbSetInterfacer;
using StoreManager.Library.Entity;

namespace StoreManager.Library.Database
{
    internal class DbSetInterfacerManager
    {
        private readonly Dictionary<Type, IDbSetInterfacer<SEntity>> _interfacers;

        internal DbSetInterfacerManager(DbContextOptions<StoreManagerContext> contextOptions) {
            _interfacers = new Dictionary<Type, IDbSetInterfacer<SEntity>>
            {
                { typeof(Store), new StoreDbSetInterfacer(contextOptions) as IDbSetInterfacer<SEntity> },
                { typeof(Customer), new CustomerDbSetInterfacer(contextOptions) as IDbSetInterfacer<SEntity> },
                { typeof(OperatingLocation), new OperatingLocationDbSetInterfacer(contextOptions) as IDbSetInterfacer<SEntity> },
                { typeof(Product), new ProductDbSetInterfacer(contextOptions) as IDbSetInterfacer<SEntity> },
                { typeof(Order), new CustomerDbSetInterfacer(contextOptions) as IDbSetInterfacer<SEntity> },
            };
        }

        internal async Task<bool> AnyAsync<T>()
            where T : SEntity {
            return await _interfacers[typeof(T)].Any();
        }

        internal async Task CreateManyAsync<T>(List<SEntity> items)
            where T : SEntity {
            await _interfacers[typeof(T)].CreateManyAsync(items);
        }

        internal async Task CreateOneAsync<T>(SEntity entity)
            where T : SEntity {
            await _interfacers[typeof(T)].CreateOneAsync(entity);
        }

        internal async Task<List<SEntity>> GetAllAsync<T>()
            where T : SEntity {
            return await Task.Run(() => _interfacers[typeof(T)].GetAllAsync());
        }

        internal async Task<List<SEntity>> GetSomeAsync<T>(List<int> ids)
            where T : SEntity {
            return await Task.Run(() => _interfacers[typeof(T)].GetManyAsync(ids));
        }

        internal async Task<SEntity> GetOneAsync<T>(int id)
            where T : SEntity {
            return await _interfacers[typeof(T)].GetOneAsync(id);
        }

        internal async Task UpdateManyAsync<T>(List<SEntity> entities)
            where T : SEntity {
            await _interfacers[typeof(T)].UpdateManyAsync(entities);
        }

        internal async Task UpdateOneAsync<T>(SEntity item)
            where T : SEntity {
            await _interfacers[typeof(T)].UpdateOneAsync(item);
        }

        internal async Task DeleteSomeAsync<T>(List<SEntity> entities)
            where T : SEntity {
            await _interfacers[typeof(T)].DeleteManyAsync(entities);
        }

        internal async Task DeleteOneAsync<T>(SEntity item)
            where T : SEntity {
            await _interfacers[typeof(T)].DeleteOneAsync(item);
        }
    }
}

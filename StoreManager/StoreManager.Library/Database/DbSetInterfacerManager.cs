using System;
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

        internal async Task Create<T>(T item)
            where T : SEntity {
            await _interfacers[typeof(T)].CreateOneAsync(item);
        }

        internal async Task<T> Get<T>(int id)
            where T : SEntity {
            return await _interfacers[typeof(T)].GetOneAsync(id) as T;
        }

        internal async Task Update<T>(T item)
            where T : SEntity {
            await _interfacers[typeof(T)].UpdateOneAsync(item);
        }

        internal async Task Delete<T>(T item)
            where T : SEntity {
            await _interfacers[typeof(T)].DeleteOneAsync(item);
        }
    }
}

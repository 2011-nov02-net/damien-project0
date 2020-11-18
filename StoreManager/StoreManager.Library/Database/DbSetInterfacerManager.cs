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
    internal class DbSetInterfacerManager {
        private readonly Dictionary<Type, IInterfacer> _interfacers;

        internal DbSetInterfacerManager(DbContextOptions<StoreManagerContext> contextOptions) {
            var storeInterfacer = new StoreDbSetInterfacer(contextOptions);
            var customerInterfacer = new CustomerDbSetInterfacer(contextOptions);
            var operatingLocationInterfacer = new OperatingLocationDbSetInterfacer(contextOptions);
            var productInterfacer = new ProductDbSetInterfacer(contextOptions);
            var orderInterfacer = new OrderDbSetInterfacer(contextOptions);
            var addressInterfacer = new AddressDbSetInterfacer(contextOptions);

            _interfacers = new Dictionary<Type, IInterfacer>
            {
                { typeof(Store), storeInterfacer },
                { typeof(Customer), customerInterfacer },
                { typeof(OperatingLocation), operatingLocationInterfacer },
                { typeof(Product), productInterfacer },
                { typeof(Order), orderInterfacer },
                { typeof(Address), addressInterfacer }
            };
        }

        internal IDbSetInterfacer<T> Interfacers<T>()
            where T : SEntity {
            return _interfacers[typeof(T)] as IDbSetInterfacer<T>;
        }

        internal async Task<bool> AnyAsync<T>()
            where T : SEntity {
            return await Interfacers<T>().Any();
        }

        internal async Task CreateManyAsync<T>(List<T> items)
            where T : SEntity {
            await Interfacers<T>().CreateManyAsync(items);
        }

        internal async Task CreateOneAsync<T>(T entity)
            where T : SEntity {
            await Interfacers<T>().CreateOneAsync(entity);
        }

        internal async Task<List<T>> GetAllAsync<T>()
            where T : SEntity {
            return await Task.Run(() => Interfacers<T>().GetAllAsync());
        }

        internal async Task<List<T>> GetSomeAsync<T>(List<int> ids)
            where T : SEntity {
            return await Task.Run(() => Interfacers<T>().GetManyAsync(ids));
        }

        internal async Task<SEntity> GetOneAsync<T>(int id)
            where T : SEntity {
            return await Interfacers<T>().GetOneAsync(id);
        }

        internal async Task UpdateManyAsync<T>(List<T> entities)
            where T : SEntity {
            await Interfacers<T>().UpdateManyAsync(entities);
        }

        internal async Task UpdateOneAsync<T>(T item)
            where T : SEntity {
            await Interfacers<T>().UpdateOneAsync(item);
        }

        internal async Task DeleteSomeAsync<T>(List<T> entities)
            where T : SEntity {
            await Interfacers<T>().DeleteManyAsync(entities);
        }

        internal async Task DeleteOneAsync<T>(T item)
            where T : SEntity {
            await Interfacers<T>().DeleteOneAsync(item);
        }
    }
}

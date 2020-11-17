using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DbStore = StoreManager.DataModel.Store;
using DbStoreInventory = StoreManager.DataModel.StoreInventory;
using StoreManagerContext = StoreManager.DataModel.StoreManagerContext;

using StoreManager.Library.Entity;
using StoreManager.Library.Data;
using Microsoft.EntityFrameworkCore;

namespace StoreManager.Library.Database.DbSetTranslator
{
    internal class StoreDbSetInterfacer : IDbSetInterfacer<Store>
    {
        internal static DbStore ToDbStore(Store store) {
            var data = store.Data;
            var operatingLocations = data.OperatingLocationIds.ConvertAll(olid =>
                OperatingLocationDbSetInterfacer.ToDbOperatingLocation(
                    StoreManagerApplication.Get<OperatingLocation>(olid)
                )
            );
            var inventory = data.Inventory.Select(
                kv => ToDbStoreInventory(kv)
            ).ToList();
            return new DbStore
            {
                StoreId = store.Id,
                Name = data.Name,
                OperatingLocations = operatingLocations,
                StoreInventories = inventory
            };
        }

        internal static Store ToStore(DbStore dbStore) {
            var data = new StoreData(
                dbStore.Name, 
                dbStore.OperatingLocations.Select(
                    ol => ol.OperatingLocationId
                ).ToList(),
                dbStore.StoreInventories.ToDictionary(
                    si => si.ProductId,
                    si => si.Count
                )
            );
            return new Store(dbStore.StoreId, data);
        }

        internal static DbStoreInventory ToDbStoreInventory(KeyValuePair<int, int> pair) {
            var product = ProductDbSetInterfacer.ToDbProduct(
                StoreManagerApplication.Get<Product>(pair.Key)
            );
            return new DbStoreInventory
            {
                Product = product,
                Count = pair.Value
            };
        }

        private readonly DbContextOptions<StoreManagerContext> _contextOptions;

        internal StoreDbSetInterfacer(DbContextOptions<StoreManagerContext> contextOptions) {
            _contextOptions = contextOptions;
        }

        public async Task Create(Store store) {
            using var context = new StoreManagerContext(_contextOptions);
            // Make sure to include the other items
            _ = context.Stores
                .Include(s => s.OperatingLocations)
                .Include(s => s.StoreInventories);
            // Convert the data for the DbContext to use
            var item = ToDbStore(store);
            context.Stores.Add(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }

        public async Task<Store> Get(int id) {
            using var context = new StoreManagerContext(_contextOptions);
            // Make sure to include the other items
            _ = context.Stores
                .Include(s => s.OperatingLocations)
                .Include(s => s.StoreInventories);
            // Convert the data for the Library to use
            var item = context.Stores.Find(id);
            return await Task.Run(() => ToStore(item));
        }

        public async Task Update(Store store) {
            using var context = new StoreManagerContext(_contextOptions);
            // Make sure to include the other items
            _ = context.Stores
                .Include(s => s.OperatingLocations)
                .Include(s => s.StoreInventories);
            // Convert the data for the DbContext to use
            var item = ToDbStore(store);
            context.Stores.Update(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }

        public async Task Delete(Store store) {
            using var context = new StoreManagerContext(_contextOptions);
            // Make sure to include the other items
            _ = context.Stores
                .Include(s => s.OperatingLocations)
                .Include(s => s.StoreInventories);
            // Convert the data for the DbContext to use
            var item = ToDbStore(store);
            context.Stores.Remove(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }
    }
}

﻿using System;
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

namespace StoreManager.Library.Database.DbSetInterfacer
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
                    si => new Tuple<int, int?>(si.Count, si.Threshold)
                )
            );
            return new Store(dbStore.StoreId, data);
        }

        internal static DbStoreInventory ToDbStoreInventory(KeyValuePair<int, Tuple<int, int?>> pair) {
            var product = ProductDbSetInterfacer.ToDbProduct(
                StoreManagerApplication.Get<Product>(pair.Key)
            );
            return new DbStoreInventory
            {
                Product = product,
                Count = pair.Value.Item1,
                Threshold = pair.Value.Item2
            };
        }

        private readonly DbContextOptions<StoreManagerContext> _contextOptions;

        internal StoreDbSetInterfacer(DbContextOptions<StoreManagerContext> contextOptions) {
            _contextOptions = contextOptions;
        }

        public async Task<bool> Any() {
            using var context = new StoreManagerContext(_contextOptions);
            return await context.Stores.AnyAsync();
        }

        public async Task<bool> IdExistsAsync(int id) {
            using var context = new StoreManagerContext(_contextOptions);
            if (!context.Stores.Any())
                return false;
            return await Task.Run(() => context.Stores.Find(id) is not null);
        }

        public async Task<int> MaxIdAsync() {
            using var context = new StoreManagerContext(_contextOptions);
            if (!context.Stores.Any())
                return 0;
            return await Task.Run(() => context.Stores.Max(s => s.StoreId));
        }

        public async Task CreateManyAsync(List<Store> items) {
            await Task.Run(() => items.ForEach(s => CreateOneAsync(s).Wait()));
        }

        public async Task CreateOneAsync(Store store) {
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

        public async Task<List<Store>> GetAllAsync() {
            using var context = new StoreManagerContext(_contextOptions);
            if (!context.Stores.Any())
                // If there are none
                return await Task.Run(() => new List<Store>());

            // Make sure to include the other items
            var stores = context.Stores
                .Include(s => s.OperatingLocations)
                .Include(s => s.StoreInventories);
            // Convert the data for the Library to use
            return await Task.Run(() => stores.Select(s => ToStore(s)).ToList());
        }

        public async Task<List<Store>> GetManyAsync(List<int> ids) {
            return await Task.Run(() => ids.ConvertAll(id => GetOneAsync(id).Result));
        }

        public async Task<Store> GetOneAsync(int id) {
            using var context = new StoreManagerContext(_contextOptions);
            if (!context.Stores.Any())
                // If there are none
                return null;

            // Make sure to include the other items
            var stores = context.Stores
                .Include(s => s.OperatingLocations)
                .Include(s => s.StoreInventories);
            // Convert the data for the Library to use
            var item = context.Stores.Find(id);
            return await Task.Run(() => ToStore(item));
        }

        public async Task UpdateManyAsync(List<Store> items) {
            await Task.Run(() => items.ForEach(id => UpdateOneAsync(id).Wait()));
        }

        public async Task UpdateOneAsync(Store store) {
            using var context = new StoreManagerContext(_contextOptions);
            if (!context.Stores.Any())
                // If there are none
                return;

            // Make sure to include the other items
            var stores = context.Stores
                .Include(s => s.OperatingLocations)
                .Include(s => s.StoreInventories);
            // Convert the data for the DbContext to use
            var item = ToDbStore(store);
            context.Stores.Update(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }

        public async Task DeleteManyAsync(List<Store> items) {
            await Task.Run(() => items.ForEach(id => DeleteOneAsync(id).Wait()));
        }

        public async Task DeleteOneAsync(Store store) {
            using var context = new StoreManagerContext(_contextOptions);
            if (!context.Stores.Any())
                // If there are none
                return;

            // Make sure to include the other items
            var stores = context.Stores
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

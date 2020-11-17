using System.Threading.Tasks;

using DbOperatingLocation = StoreManager.DataModel.OperatingLocation;
using StoreManagerContext = StoreManager.DataModel.StoreManagerContext;

using StoreManager.Library.Entity;
using StoreManager.Library.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace StoreManager.Library.Database.DbSetInterfacer
{
    internal class OperatingLocationDbSetInterfacer : IDbSetInterfacer<OperatingLocation>
    {
        internal static DbOperatingLocation ToDbOperatingLocation(OperatingLocation operatingLocation) {
            var data = operatingLocation.Data;
            return new DbOperatingLocation
            {
                OperatingLocationId = operatingLocation.Id,
                AddressId = data.AddressId
            };
        }

        internal static OperatingLocation ToOperatingLocation(DbOperatingLocation dbOperatingLocation) {
            var data = new OperatingLocationData(dbOperatingLocation.StoreId, dbOperatingLocation.AddressId);
            return new OperatingLocation(dbOperatingLocation.OperatingLocationId, data);
        }

        private readonly DbContextOptions<StoreManagerContext> _contextOptions;

        public OperatingLocationDbSetInterfacer(DbContextOptions<StoreManagerContext> contextOptions) {
            _contextOptions = contextOptions;
        }

        public async Task CreateSomeAsync(List<OperatingLocation> items) {
            await Task.Run(() => items.ForEach(ol => CreateOneAsync(ol).Wait()));
        }

        public async Task CreateOneAsync(OperatingLocation operatingLocation) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the DbContext to use
            var item = ToDbOperatingLocation(operatingLocation);
            context.OperatingLocations.Add(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }

        public async Task<List<OperatingLocation>> GetAllAsync() {
            using var context = new StoreManagerContext(_contextOptions);
            // Make sure to include the other items
            var operatingLocations = context.OperatingLocations
                .Include(ol => ol.StoreId)
                .Include(ol => ol.AddressId);
            // Convert the data for the Library to use
            return await Task.Run(() => operatingLocations.Select(ol => ToOperatingLocation(ol)).ToList());
        }

        public async Task<List<OperatingLocation>> GetSomeAsync(List<int> ids) {
            return await Task.Run(() => ids.ConvertAll(id => GetOneAsync(id).Result));
        }

        public async Task<OperatingLocation> GetOneAsync(int id) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the Library to use
            var item = context.OperatingLocations.Find(id);
            return await Task.Run(() => ToOperatingLocation(item));
        }

        public async Task UpdateAllAsync(List<OperatingLocation> items) {
            using var context = new StoreManagerContext(_contextOptions);
            // Make sure to include the other items
            var operatingLocations = context.OperatingLocations
                .Include(ol => ol.StoreId)
                .Include(ol => ol.AddressId);
            // Convert the data for the Library to use
            await Task.Run(() => operatingLocations.Select(ol => UpdateOneAsync(ToOperatingLocation(ol))));
        }

        public async Task UpdateSomeAsync(List<OperatingLocation> items) {
            await Task.Run(() => items.ForEach(id => UpdateOneAsync(id).Wait()));
        }

        public async Task UpdateOneAsync(OperatingLocation operatingLocation) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the DbContext to use
            var item = ToDbOperatingLocation(operatingLocation);
            context.OperatingLocations.Update(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }

        public async Task DeleteAllAsync() {
            using var context = new StoreManagerContext(_contextOptions);
            var items = context.OperatingLocations;
            await Task.Run(() => items.ForEachAsync(id => DeleteOneAsync(ToOperatingLocation(id)).Wait()));
        }

        public async Task DeleteSomeAsync(List<OperatingLocation> items) {
            await Task.Run(() => items.ForEach(id => DeleteOneAsync(id).Wait()));
        }

        public async Task DeleteOneAsync(OperatingLocation operatingLocation) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the DbContext to use
            var item = ToDbOperatingLocation(operatingLocation);
            context.OperatingLocations.Remove(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }
    }
}

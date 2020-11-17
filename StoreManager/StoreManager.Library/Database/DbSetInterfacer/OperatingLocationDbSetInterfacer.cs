using System.Threading.Tasks;

using DbOperatingLocation = StoreManager.DataModel.OperatingLocation;
using StoreManagerContext = StoreManager.DataModel.StoreManagerContext;

using StoreManager.Library.Entity;
using StoreManager.Library.Data;
using Microsoft.EntityFrameworkCore;

namespace StoreManager.Library.Database.DbSetTranslator
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

        public async Task Create(OperatingLocation operatingLocation) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the DbContext to use
            var item = ToDbOperatingLocation(operatingLocation);
            context.OperatingLocations.Add(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }

        public async Task<OperatingLocation> Get(int id) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the Library to use
            var item = context.OperatingLocations.Find(id);
            return await Task.Run(() => ToOperatingLocation(item));
        }

        public async Task Update(OperatingLocation operatingLocation) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the DbContext to use
            var item = ToDbOperatingLocation(operatingLocation);
            context.OperatingLocations.Update(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }

        public async Task Delete(OperatingLocation operatingLocation) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the DbContext to use
            var item = ToDbOperatingLocation(operatingLocation);
            context.OperatingLocations.Remove(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }
    }
}

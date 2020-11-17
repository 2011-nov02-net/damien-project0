using System.Threading.Tasks;

using DbAddress = StoreManager.DataModel.Address;
using StoreManagerContext = StoreManager.DataModel.StoreManagerContext;

using StoreManager.Library.Entity;
using StoreManager.Library.Data;
using Microsoft.EntityFrameworkCore;

namespace StoreManager.Library.Database.DbSetTranslator
{
    internal class AddressDbSetInterfacer : IDbSetInterfacer<Address>
    {
        private static DbAddress ToDbAddress(Address address) {
            var data = address.Data;
            return new DbAddress
            {
                AddressId = address.Id,
                AddressLine1 = data.AddressLine1,
                AddressLine2 = data.AddressLine2,
                City = data.City,
                State = data.State,
                Country = data.Country,
                ZipCode = data.ZipCode
            };
        }

        private static Address ToAddress(DbAddress dbAddress) {
            var data = new AddressData(dbAddress.AddressLine1, dbAddress.AddressLine2,
                dbAddress.City, dbAddress.State, dbAddress.Country, dbAddress.ZipCode);
            return new Address(dbAddress.AddressId, data);
        }

        private readonly DbContextOptions<StoreManagerContext> _contextOptions;

        public AddressDbSetInterfacer(DbContextOptions<StoreManagerContext> contextOptions) {
            _contextOptions = contextOptions;
        }

        public async Task Create(Address address) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the DbContext to use
            var item = ToDbAddress(address);
            context.Addresses.Add(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }

        public async Task<Address> Get(int id) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the Library to use
            var item = context.Addresses.Find(id);
            return await Task.Run(() => ToAddress(item));
        }

        public async Task Update(Address address) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the DbContext to use
            var item = ToDbAddress(address);
            context.Addresses.Update(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }

        public async Task Delete(Address address) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the DbContext to use
            var item = ToDbAddress(address);
            context.Addresses.Remove(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }
    }
}

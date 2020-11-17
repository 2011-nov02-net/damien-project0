using System.Threading.Tasks;

using DbAddress = StoreManager.DataModel.Address;
using StoreManagerContext = StoreManager.DataModel.StoreManagerContext;

using StoreManager.Library.Entity;
using StoreManager.Library.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace StoreManager.Library.Database.DbSetInterfacer
{
    internal class AddressDbSetInterfacer : IDbSetInterfacer<Address>
    {
        internal static DbAddress ToDbAddress(Address address) {
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

        internal static Address ToAddress(DbAddress dbAddress) {
            var data = new AddressData(dbAddress.AddressLine1, dbAddress.AddressLine2,
                dbAddress.City, dbAddress.State, dbAddress.Country, dbAddress.ZipCode);
            return new Address(dbAddress.AddressId, data);
        }

        private readonly DbContextOptions<StoreManagerContext> _contextOptions;

        public AddressDbSetInterfacer(DbContextOptions<StoreManagerContext> contextOptions) {
            _contextOptions = contextOptions;
        }

        public async Task CreateSomeAsync(List<Address> items) {
            await Task.Run(() => items.ForEach(a => CreateOneAsync(a).Wait()));
        }

        public async Task CreateOneAsync(Address address) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the DbContext to use
            var item = ToDbAddress(address);
            context.Addresses.Add(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }

        public async Task<List<Address>> GetAllAsync() {
            using var context = new StoreManagerContext(_contextOptions);
            // Make sure to include the other items
            var addresses = context.Addresses;
            // Convert the data for the Library to use
            return await Task.Run(() => addresses.Select(a => ToAddress(a)).ToList());
        }

        public async Task<List<Address>> GetSomeAsync(List<int> ids) {
            return await Task.Run(() => ids.ConvertAll(id => GetOneAsync(id).Result));
        }

        public async Task<Address> GetOneAsync(int id) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the Library to use
            var item = context.Addresses.Find(id);
            return await Task.Run(() => ToAddress(item));
        }

        public async Task UpdateAllAsync(List<Address> items) {
            using var context = new StoreManagerContext(_contextOptions);
            // Make sure to include the other items
            var addresses = context.Addresses;
            // Convert the data for the Library to use
            await Task.Run(() => addresses.Select(a => UpdateOneAsync(ToAddress(a))));
        }

        public async Task UpdateSomeAsync(List<Address> items) {
            await Task.Run(() => items.ForEach(id => UpdateOneAsync(id).Wait()));
        }

        public async Task UpdateOneAsync(Address address) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the DbContext to use
            var item = ToDbAddress(address);
            context.Addresses.Update(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }

        public async Task DeleteSomeAsync(List<Address> items) {
            await Task.Run(() => items.ForEach(id => DeleteOneAsync(id).Wait()));
        }

        public async Task DeleteOneAsync(Address address) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the DbContext to use
            var item = ToDbAddress(address);
            context.Addresses.Remove(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }
    }
}

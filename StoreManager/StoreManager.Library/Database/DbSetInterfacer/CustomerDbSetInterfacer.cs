using System.Threading.Tasks;

using DbCustomer = StoreManager.DataModel.Customer;
using StoreManagerContext = StoreManager.DataModel.StoreManagerContext;

using StoreManager.Library.Entity;
using StoreManager.Library.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace StoreManager.Library.Database.DbSetInterfacer
{
    internal class CustomerDbSetInterfacer : IDbSetInterfacer<Customer>
    {
        internal static DbCustomer ToDbCustomer(Customer customer) {
            var data = customer.Data;
            return new DbCustomer
            {
                CustomerId = customer.Id,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Email = data.Email,
                PhoneNumber = data.PhoneNumber,
                BirthDate = data.BirthDate,
                AddressId = data.AddressId,
                OperatingLocationId = data.DefaultStoreLocationId
            };
        }

        internal static Customer ToCustomer(DbCustomer dbCustomer) {
            var data = new CustomerData(dbCustomer.FirstName, dbCustomer.LastName, dbCustomer.Email, dbCustomer.PhoneNumber, dbCustomer.AddressId, dbCustomer.BirthDate, dbCustomer.OperatingLocationId);
            return new Customer(dbCustomer.CustomerId, data);
        }

        private readonly DbContextOptions<StoreManagerContext> _contextOptions;

        public CustomerDbSetInterfacer(DbContextOptions<StoreManagerContext> contextOptions) {
            _contextOptions = contextOptions;
        }

        public async Task CreateSomeAsync(List<Customer> items) {
            await Task.Run(() => items.ForEach(c => CreateOneAsync(c).Wait()));
        }

        public async Task CreateOneAsync(Customer customer) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the DbContext to use
            var item = ToDbCustomer(customer);
            context.Customers.Add(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }

        public async Task<List<Customer>> GetAllAsync() {
            using var context = new StoreManagerContext(_contextOptions);
            // Make sure to include the other items
            var customers = context.Customers
                .Include(c => c.AddressId)
                .Include(c => c.OperatingLocationId);
            // Convert the data for the Library to use
            return await Task.Run(() => customers.Select(c => ToCustomer(c)).ToList());
        }

        public async Task<List<Customer>> GetSomeAsync(List<int> ids) {
            return await Task.Run(() => ids.ConvertAll(id => GetOneAsync(id).Result));
        }

        public async Task<Customer> GetOneAsync(int id) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the Library to use
            var item = context.Customers.Find(id);
            return await Task.Run(() => ToCustomer(item));
        }

        public async Task UpdateAllAsync(List<Customer> items) {
            using var context = new StoreManagerContext(_contextOptions);
            // Make sure to include the other items
            var customers = context.Customers
                .Include(c => c.AddressId)
                .Include(c => c.OperatingLocationId);
            // Convert the data for the Library to use
            await Task.Run(() => customers.Select(c => UpdateOneAsync(ToCustomer(c))));
        }

        public async Task UpdateSomeAsync(List<Customer> items) {
            await Task.Run(() => items.ForEach(id => UpdateOneAsync(id).Wait()));
        }

        public async Task UpdateOneAsync(Customer customer) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the DbContext to use
            var item = ToDbCustomer(customer);
            context.Customers.Update(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }

        public async Task DeleteSomeAsync(List<Customer> items) {
            await Task.Run(() => items.ForEach(id => DeleteOneAsync(id).Wait()));
        }

        public async Task DeleteOneAsync(Customer customer) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the DbContext to use
            var item = ToDbCustomer(customer);
            context.Customers.Remove(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }
    }
}

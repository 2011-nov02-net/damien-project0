using System.Threading.Tasks;

using DbCustomer = StoreManager.DataModel.Customer;
using StoreManagerContext = StoreManager.DataModel.StoreManagerContext;

using StoreManager.Library.Entity;
using StoreManager.Library.Data;
using Microsoft.EntityFrameworkCore;

namespace StoreManager.Library.Database.DbSetTranslator
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

        public async Task Create(Customer customer) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the DbContext to use
            var item = ToDbCustomer(customer);
            context.Customers.Add(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }

        public async Task Delete(Customer customer) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the DbContext to use
            var item = ToDbCustomer(customer);
            context.Customers.Remove(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }

        public async Task<Customer> Get(int id) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the Library to use
            var item = context.Customers.Find(id);
            return await Task.Run(() => ToCustomer(item));
        }

        public async Task Update(Customer customer) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the DbContext to use
            var item = ToDbCustomer(customer);
            context.Customers.Update(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }
    }
}

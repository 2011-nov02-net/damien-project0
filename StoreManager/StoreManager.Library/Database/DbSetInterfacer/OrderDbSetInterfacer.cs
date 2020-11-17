using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DbOrder = StoreManager.DataModel.Order;
using DbOrderProduct = StoreManager.DataModel.OrderProduct;
using StoreManagerContext = StoreManager.DataModel.StoreManagerContext;

using StoreManager.Library.Entity;
using StoreManager.Library.Data;
using Microsoft.EntityFrameworkCore;

namespace StoreManager.Library.Database.DbSetInterfacer
{
    internal class OrderDbSetInterfacer : IDbSetInterfacer<Order>
    {
        internal static DbOrder ToDbOrder(Order order) {
            var data = order.Data;
            // Get the customer
            var customer = CustomerDbSetInterfacer.ToDbCustomer(
                StoreManagerApplication.Get<Customer>(data.CustomerId)
            );
            // Get the Operating Location
            var operatingLocations = OperatingLocationDbSetInterfacer.ToDbOperatingLocation(
                    StoreManagerApplication.Get<OperatingLocation>(data.OperatingLocationId)
            );
            // Get the Products Requested
            var productsRequested = data.ProductsRequested.Select(
                pr => ToDbOrderProduct(pr)
            ).ToList();
            return new DbOrder
            {
                OrderId = order.Id,
                Customer = customer,
                OperatingLocation = null,
                OrderProducts = productsRequested
            };
        }

        internal static Order ToOrder(DbOrder dbOrder) {
            var data = new OrderData(dbOrder.CustomerId, dbOrder.OperatingLocationId,
                dbOrder.OrderProducts.ToDictionary(
                    op => op.ProductId,
                    op => op.Count
                )
            );
            return new Order(dbOrder.OrderId, data);
        }

        internal static DbOrderProduct ToDbOrderProduct(KeyValuePair<int, int> pair) {
            var product = ProductDbSetInterfacer.ToDbProduct(
                StoreManagerApplication.Get<Product>(pair.Key)
            );
            return new DbOrderProduct
            {
                Product = product,
                Count = pair.Value
            };
        }

        private readonly DbContextOptions<StoreManagerContext> _contextOptions;

        public OrderDbSetInterfacer(DbContextOptions<StoreManagerContext> contextOptions) {
            _contextOptions = contextOptions;
        }

        public async Task CreateSomeAsync(List<Order> items) {
            await Task.Run(() => items.ForEach(o => CreateOneAsync(o).Wait()));
        }

        public async Task CreateOneAsync(Order order) {
            using var context = new StoreManagerContext(_contextOptions);
            // Make sure to include the other items
            _ = context.Orders
                .Include(o => o.OrderProducts);
            // Convert the data for the DbContext to use
            var item = ToDbOrder(order);
            context.Orders.Add(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }

        public async Task<List<Order>> GetAllAsync() {
            using var context = new StoreManagerContext(_contextOptions);
            // Make sure to include the other items
            var orders = context.Orders
                .Include(o => o.OperatingLocationId)
                .Include(o => o.CustomerId);
            // Convert the data for the Library to use
            return await Task.Run(() => orders.Select(o => ToOrder(o)).ToList());
        }

        public async Task<List<Order>> GetSomeAsync(List<int> ids) {
            return await Task.Run(() => ids.ConvertAll(id => GetOneAsync(id).Result));
        }

        public async Task<Order> GetOneAsync(int id) {
            using var context = new StoreManagerContext(_contextOptions);
            // Make sure to include the other items
            _ = context.Orders
                .Include(o => o.OrderProducts);
            // Convert the data for the Library to use
            var item = context.Orders.Find(id);
            return await Task.Run(() => ToOrder(item));
        }

        public async Task UpdateAllAsync(List<Order> items) {
            using var context = new StoreManagerContext(_contextOptions);
            // Make sure to include the other items
            var orders = context.Orders
                .Include(o => o.OperatingLocationId)
                .Include(o => o.CustomerId);
            // Convert the data for the Library to use
            await Task.Run(() => orders.Select(o => UpdateOneAsync(ToOrder(o))));
        }

        public async Task UpdateSomeAsync(List<Order> items) {
            await Task.Run(() => items.ForEach(id => UpdateOneAsync(id).Wait()));
        }

        public async Task UpdateOneAsync(Order order) {
            using var context = new StoreManagerContext(_contextOptions);
            // Make sure to include the other items
            _ = context.Orders
                .Include(o => o.OrderProducts);
            // Convert the data for the DbContext to use
            var item = ToDbOrder(order);
            context.Orders.Update(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }

        public async Task DeleteSomeAsync(List<Order> items) {
            await Task.Run(() => items.ForEach(id => DeleteOneAsync(id).Wait()));
        }

        public async Task DeleteOneAsync(Order order) {
            using var context = new StoreManagerContext(_contextOptions);
            // Make sure to include the other items
            _ = context.Orders
                .Include(o => o.OrderProducts);
            // Convert the data for the DbContext to use
            var item = ToDbOrder(order);
            context.Orders.Remove(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }
    }
}

using System.Threading.Tasks;

using DbProduct = StoreManager.DataModel.Product;
using StoreManagerContext = StoreManager.DataModel.StoreManagerContext;

using StoreManager.Library.Entity;
using StoreManager.Library.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace StoreManager.Library.Database.DbSetInterfacer
{
    internal class ProductDbSetInterfacer : IDbSetInterfacer<Product>
    {
        internal static DbProduct ToDbProduct(Product product) {
            var data = product.Data;
            return new DbProduct
            {
                ProductId = product.Id,
                Name = product.GetName(),
                Price = data.Price,
                Discount = data.Discount
            };
        }

        internal static Product ToProduct(DbProduct dbProduct) {
            var data = new ProductData(dbProduct.Name, dbProduct.Price, dbProduct.Discount);
            return new Product(dbProduct.ProductId, data);
        }

        private readonly DbContextOptions<StoreManagerContext> _contextOptions;

        internal ProductDbSetInterfacer(DbContextOptions<StoreManagerContext> contextOptions) {
            _contextOptions = contextOptions;
        }

        public async Task<bool> Any() {
            using var context = new StoreManagerContext(_contextOptions);
            return await context.Products.AnyAsync();
        }

        public async Task CreateManyAsync(List<Product> items) {
            await Task.Run(() => items.ForEach(p => CreateOneAsync(p).Wait()));
        }

        public async Task CreateOneAsync(Product product) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the DbContext to use
            var item = ToDbProduct(product);
            context.Products.Add(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }

        public async Task<List<Product>> GetAllAsync() {
            using var context = new StoreManagerContext(_contextOptions);
            var products = context.Products;
            // Make sure to include the other items
            // Convert the data for the Library to use
            return await Task.Run(() => products.Select(p => ToProduct(p)).ToList());
        }

        public async Task<List<Product>> GetManyAsync(List<int> ids) {
            return await Task.Run(() => ids.ConvertAll(id => GetOneAsync(id).Result));
        }

        public async Task<Product> GetOneAsync(int id) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the Library to use
            var item = context.Products.Find(id);
            return await Task.Run(() => ToProduct(item));
        }

        public async Task UpdateManyAsync(List<Product> items) {
            await Task.Run(() => items.ForEach(id => UpdateOneAsync(id).Wait()));
        }

        public async Task UpdateOneAsync(Product product) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the DbContext to use
            var item = ToDbProduct(product);
            context.Products.Update(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }

        public async Task DeleteManyAsync(List<Product> items) {
            await Task.Run(() => items.ForEach(id => DeleteOneAsync(id).Wait()));
        }

        public async Task DeleteOneAsync(Product product) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the DbContext to use
            var item = ToDbProduct(product);
            context.Products.Remove(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }
    }
}

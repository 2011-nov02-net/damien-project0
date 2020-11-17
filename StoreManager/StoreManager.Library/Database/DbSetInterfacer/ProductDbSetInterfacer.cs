using System.Threading.Tasks;

using DbProduct = StoreManager.DataModel.Product;
using StoreManagerContext = StoreManager.DataModel.StoreManagerContext;

using StoreManager.Library.Entity;
using StoreManager.Library.Data;
using Microsoft.EntityFrameworkCore;

namespace StoreManager.Library.Database.DbSetTranslator
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

        public async Task Create(Product product) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the DbContext to use
            var item = ToDbProduct(product);
            context.Products.Add(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }

        public async Task<Product> Get(int id) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the Library to use
            var item = context.Products.Find(id);
            return await Task.Run(() => ToProduct(item));
        }

        public async Task Update(Product product) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the DbContext to use
            var item = ToDbProduct(product);
            context.Products.Update(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }

        public async Task Delete(Product product) {
            using var context = new StoreManagerContext(_contextOptions);
            // Convert the data for the DbContext to use
            var item = ToDbProduct(product);
            context.Products.Remove(item);
            // Save the changes
            await Task.Run(() => context.SaveChanges());
        }
    }
}

using System.Collections.Generic;
using System.Linq;

using StoreManager.Library.Data;
using StoreManager.Library.Entity;

namespace StoreManager.Library.Factory
{
    internal class ProductFactory : ISEntityFactory<Product>
    {
        private readonly IdGenerator _idGenerator;
        
        public ProductFactory() {
            Items = new List<Product>();
            _idGenerator = new IdGenerator(0);
        }

        public ProductFactory(List<Product> products) {
            Items = products;
            _idGenerator = new IdGenerator(Items.Max(p => p.Id));
        }
        
        public List<Product> Items { get; set; }

        public int Create(IData data) {
            var product = new Product(_idGenerator, data as ProductData);
            Items.Add(product);
            return product.Id;
        }

        public Product Get(int id) {
            return Items.FirstOrDefault(p => p.Id == id);
        }

        public void Update(int id, IData data) {
            var product = Get(id);

            if (product is null)
                return;

            product.Data = data as ProductData;
        }

        public void Delete(int id) {
            var product = Get(id);

            if (product is null)
                return;

            Items.Remove(product);
        }
    }
}

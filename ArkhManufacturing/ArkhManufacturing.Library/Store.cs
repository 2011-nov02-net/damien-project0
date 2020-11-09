using ArkhManufacturing.Library.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace ArkhManufacturing.Library
{
    // TODO: Add comment here
    public class Store : IdentifiableBase
    {
        private static readonly IdGenerator _idGenerator = new IdGenerator();

        // TODO: Add comment here
        public string Name { get; internal set; }
        // TODO: Add comment here
        public Location Location { get; internal set; }
        // TODO: Add comment here
        public List<Order> Orders { get; private set; }
        // TODO: Add comment here
        public Dictionary<Product, int> Inventory { get; internal set; }

        private int _productCountThreshold;

        // TODO: Add comment here
        public int ProductCountThreshold
        {
            get => _productCountThreshold;
            set => _productCountThreshold = value > 0 ? value : _productCountThreshold;
        }

        // TODO: Add comment here
        public Store(string name, int productCountThreshold, Location location) :
            base(_idGenerator) {
            Name = name;
            ProductCountThreshold = productCountThreshold;
            Location = location;
            Orders = new List<Order>();
            Inventory = new Dictionary<Product, int>();
        }

        // TODO: Add comment here
        public Store(string name, int productCountThreshold, Location location, List<Order> orders, Dictionary<Product, int> inventory) :
            base(_idGenerator) {
            Name = name;
            ProductCountThreshold = productCountThreshold;
            Location = location;
            Orders = orders;
            Inventory = inventory;
        }

        // TODO: Add comment here
        public bool HasStock() => Inventory.All(kv => kv.Value > 0);

        // TODO: Add comment here
        public List<Product> ProductsInInventory() => Inventory.Where(kv => kv.Value > 0).Select(kv => kv.Key).ToList();

        // TODO: Add comment here
        public Product GetProductById(long productId) => Inventory.FirstOrDefault(kv => kv.Value > 0 && kv.Key.Id == productId).Key;

        // TODO: Add comment here
        public void AddProduct(Product product, int count) {
            Inventory[product] = count;
        }

        // TODO: Add comment here
        public void SubmitOrder(Order order) {
            foreach (var kv in order.ProductsRequested) {
                // Check if we have the product
                if (Inventory.ContainsKey(kv.Key)) {
                    // Check if the number of items requested isn't too much
                    if (kv.Value > Inventory[kv.Key])
                        throw new ProductRequestExcessiveException(kv.Key, kv.Value, Inventory[kv.Key]);
                }
                // We don't have the product
                else throw new ProductNotOfferedException(kv.Key);
            }

            // Update the inventory, since the order is acceptable
            foreach (var kv in order.ProductsRequested)
                Inventory[kv.Key] -= kv.Value;

            // Add the order
            Orders.Add(order);
        }

        // TODO: Add comment here
        public override string ToString() => $"Store#{Id} at {Location}";
    }
}

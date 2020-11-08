using ArkhManufacturing.Library.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace ArkhManufacturing.Library
{
    public class Store : IdentifiableBase
    {
        private static readonly IdGenerator _idGenerator = new IdGenerator();

        public static List<Product> Catalog;

        public Location Location { get; }
        public List<Order> Orders { get; private set; }
        public Dictionary<Product, int> Inventory { get; private set; }

        private int _productCountThreshold;

        public int ProductCountThreshold
        {
            get => _productCountThreshold;
            private set => _productCountThreshold = value > 0 ? value : _productCountThreshold;
        }

        public Store(int productCountThreshold, Location location, List<Order> orders, Dictionary<Product, int> inventory) :
            base(_idGenerator)
        {
            ProductCountThreshold = productCountThreshold;
            Location = location;
            Orders = orders;
            Inventory = inventory;
        }

        public bool HasStock() => Inventory.All(kv => kv.Value > 0);

        public List<Product> ProductsInInventory() => Inventory.Where(kv => kv.Value > 0).Select(kv => kv.Key).ToList();

        public Product GetProductById(long productId) => Inventory.FirstOrDefault(kv => kv.Value > 0 && kv.Key.Id == productId).Key;

        public void SubmitOrder(Order order)
        {
            foreach (var kv in order.ProductsRequested)
            {
                // Check if we have the product
                if (Inventory.ContainsKey(kv.Key))
                {
                    // Check if the number of items requested isn't too much
                    if (kv.Value > Inventory[kv.Key])
                        throw new ProductRequestExcessiveException(kv.Key, kv.Value, Inventory[kv.Key]);
                }
                // We don't have the product
                else throw new ProductNotOfferedException(kv.Key);
            }

            // Update the inventory, since the order is acceptable
            foreach(var kv in order.ProductsRequested)
                Inventory[kv.Key] -= kv.Value;

            // Add the order
            Orders.Add(order);
        }

        public override string ToString() => $"Store#{Id} at {Location}";
    }
}

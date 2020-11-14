using ArkhManufacturing.Library.Exception;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ArkhManufacturing.Library
{
    // TODO: Add comment here
    public class Store : Identifiable
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
        internal Store(string name, int productCountThreshold, Location location) :
            base(_idGenerator) {
            Name = name;
            ProductCountThreshold = productCountThreshold;
            Location = location;
            Orders = new List<Order>();
            Inventory = new Dictionary<Product, int>();
        }

        // TODO: Add comment here
        internal Store(string name, int productCountThreshold, Location location, List<Order> orders, Dictionary<Product, int> inventory) :
            base(_idGenerator) {
            Name = name;
            ProductCountThreshold = productCountThreshold;
            Location = location;
            Orders = orders;
            Inventory = inventory;
        }

        // TODO: Add comment here
        public bool HasStock() => Inventory.Any(kv => kv.Value > 0);

        // TODO: Add comment here
        public void AddProduct(Product product, int count) {
            Inventory[product] = count;
        }

        // TODO: Add comment here
        public void SubmitOrder(Order order) {
            foreach (var kv in order.ProductsRequested) {
                // Check if we have the product
                var product = kv.Key;
                var count = kv.Value;
                if (Inventory.ContainsKey(product)) {
                    // Check if the number of items requested isn't too much
                    if (count > Inventory[product]) {
                        throw new ProductRequestExcessiveException(product, count, Inventory[product]);
                    }
                }
                // We don't have the product
                else {
                    throw new ProductNotOfferedException(product);
                }
            }

            // Update the inventory, since the order is acceptable
            foreach (var kv in order.ProductsRequested)
                // Decrement the count in inventory
                Inventory[kv.Key] -= kv.Value;

            // Add the order
            Orders.Add(order);
        }

        // TODO: Add comment here
        public override string ToString() => $"Store#{Id} at {Location}";

        // TODO: Add comment here
        public override bool Equals(object obj) {
            if (obj is Store) {
                return (obj as Store).Id == Id;
            } else return false;
        }

        // TODO: Add comment here
        public override int GetHashCode() => HashCode.Combine(Id, Name, Location, Orders, Inventory, _productCountThreshold, ProductCountThreshold);
    }
}

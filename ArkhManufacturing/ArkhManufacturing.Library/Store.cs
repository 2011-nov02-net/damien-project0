using ArkhManufacturing.Library.Exceptions;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ArkhManufacturing.Library
{
    // TODO: Add comment here
    public class Store : IdentifiableBase
    {
        private static readonly IdGenerator _idGenerator = new IdGenerator();

        private readonly Franchise _parentFranchise;

        // TODO: Add comment here
        public string Name { get; internal set; }
        // TODO: Add comment here
        public Location Location { get; internal set; }
        // TODO: Add comment here
        public List<Order> Orders { get; private set; }
        // TODO: Add comment here
        public Dictionary<long, int> Inventory { get; internal set; }

        private int _productCountThreshold;

        // TODO: Add comment here
        public int ProductCountThreshold
        {
            get => _productCountThreshold;
            set => _productCountThreshold = value > 0 ? value : _productCountThreshold;
        }

        // TODO: Add comment here
        public Store(Franchise parent, string name, int productCountThreshold, Location location) :
            base(_idGenerator) {
            _parentFranchise = parent;
            Name = name;
            ProductCountThreshold = productCountThreshold;
            Location = location;
            Orders = new List<Order>();
            Inventory = new Dictionary<long, int>();
        }

        // TODO: Add comment here
        public Store(Franchise parent, string name, int productCountThreshold, Location location, List<Order> orders, Dictionary<long, int> inventory) :
            base(_idGenerator) {
            _parentFranchise = parent;
            Name = name;
            ProductCountThreshold = productCountThreshold;
            Location = location;
            Orders = orders;
            Inventory = inventory;
        }

        // TODO: Add comment here
        public bool HasStock() => Inventory.Any(kv => kv.Value > 0);

        // TODO: Add comment here
        public List<Product> ProductsInInventory() => Inventory.Where(kv => kv.Value > 0).Select(kv => _parentFranchise.GetProductById(kv.Key)).ToList();

        // TODO: Add comment here
        public Product GetProductById(long productId) {
            return _parentFranchise.GetProductById(productId);
        }

        // TODO: Add comment here
        public void AddProduct(Product product, int count) {
            Inventory[product.Id] = count;
        }

        // TODO: Add comment here
        public void SubmitOrder(Order order) {
            foreach (var kv in order.ProductsRequested) {
                // Check if we have the product
                if (Inventory.ContainsKey(kv.Key)) {
                    // Check if the number of items requested isn't too much
                    if (kv.Value > Inventory[kv.Key]) {
                        var product = _parentFranchise.GetProductById(kv.Key);
                        throw new ProductRequestExcessiveException(product, kv.Value, Inventory[kv.Key]);
                    }
                }
                // We don't have the product
                else {
                    var product = _parentFranchise.GetProductById(kv.Key);
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
    }
}

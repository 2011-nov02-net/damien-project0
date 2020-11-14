using System.Collections.Generic;

namespace ArkhManufacturing.Library.CreationData
{
    // TODO: Add comment here
    public class StoreCreationData : ICreationData
    {
        // TODO: Add comment here
        public StoreCreationData(string name, Location location, List<Order> orders, Dictionary<Product, int> inventory) {
            Name = name;
            Location = location;
            Orders = orders;
            Inventory = inventory;
        }

        // TODO: Add comment here
        public string Name { get; }
        // TODO: Add comment here
        public Location Location { get; }
        // TODO: Add comment here
        public List<Order> Orders { get; }
        // TODO: Add comment here
        public Dictionary<Product, int> Inventory { get; }
        public int ProductCountThreshold { get; }
    }
}

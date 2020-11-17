using System.Collections.Generic;

namespace StoreManager.Library.Data
{
    public class StoreData : NamedData
    {
        public override string Name { get; set; }
        public List<int> OperatingLocationIds { get; set; }
        public List<int> CustomerIds { get; set; }
        public List<int> OrderIds { get; set; }
        public Dictionary<int, int> Inventory { get; set; }

        public StoreData() { }

        public StoreData(string name, List<int> operatingLocations, List<int> customers, Dictionary<int, int> inventory) {
            Name = name;
            OperatingLocationIds = operatingLocations;
            CustomerIds = customers;
            Inventory = inventory;
        }

        public StoreData(StoreData data) :
            this(data.Name, data.OperatingLocationIds, data.CustomerIds, data.Inventory) {
        }
    }
}

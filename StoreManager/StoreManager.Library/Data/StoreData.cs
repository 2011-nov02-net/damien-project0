using System.Collections.Generic;

namespace StoreManager.Library.Data
{
    public class StoreData : NamedData
    {
        public override string Name { get; set; }
        public List<long> OperatingLocationIds { get; set; }
        public List<long> CustomerIds { get; set; }
        public List<long> OrderIds { get; set; }
        public Dictionary<long, int> Inventory { get; set; }

        public StoreData() { }

        public StoreData(string name, List<long> operatingLocations, List<long> customers, Dictionary<long, int> inventory) {
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

using System;
using System.Collections.Generic;

#nullable disable

namespace StoreManager.DataModel
{
    public partial class Store
    {
        public Store()
        {
            OperatingLocations = new HashSet<OperatingLocation>();
            StoreInventories = new HashSet<StoreInventory>();
        }

        public int StoreId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<OperatingLocation> OperatingLocations { get; set; }
        public virtual ICollection<StoreInventory> StoreInventories { get; set; }
    }
}

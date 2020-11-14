using System;
using System.Collections.Generic;

#nullable disable

namespace StoreManager.DataModel
{
    public partial class OperatingLocation
    {
        public OperatingLocation()
        {
            Customers = new HashSet<Customer>();
            Orders = new HashSet<Order>();
            StoreOperatingLocations = new HashSet<StoreOperatingLocation>();
        }

        public int OperatingLocationId { get; set; }
        public int AddressId { get; set; }

        public virtual Address Address { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<StoreOperatingLocation> StoreOperatingLocations { get; set; }
    }
}

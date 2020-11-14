using System;
using System.Collections.Generic;

#nullable disable

namespace StoreManager.DataModel
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int AddressId { get; set; }
        public int? OperatingLocationId { get; set; }

        public virtual Address Address { get; set; }
        public virtual OperatingLocation OperatingLocation { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace StoreManager.DataModel
{
    public partial class Order
    {
        public Order()
        {
            CustomerOrders = new HashSet<CustomerOrder>();
            OrderProducts = new HashSet<OrderProduct>();
        }

        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int OperatingLocationId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual OperatingLocation OperatingLocation { get; set; }
        public virtual ICollection<CustomerOrder> CustomerOrders { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}

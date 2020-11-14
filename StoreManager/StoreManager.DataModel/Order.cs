using System;
using System.Collections.Generic;

#nullable disable

namespace StoreManager.DataModel
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int OperatingLocationId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual OperatingLocation OperatingLocation { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace StoreManager.DataModel
{
    public partial class StoreOperatingLocation
    {
        public int StoreId { get; set; }
        public int OperatingLocationId { get; set; }

        public virtual OperatingLocation OperatingLocation { get; set; }
        public virtual Store Store { get; set; }
    }
}

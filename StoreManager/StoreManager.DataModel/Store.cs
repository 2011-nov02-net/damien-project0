using System;
using System.Collections.Generic;

#nullable disable

namespace StoreManager.DataModel
{
    public partial class Store
    {
        public int StoreId { get; set; }
        public string Name { get; set; }

        public virtual StoreOperatingLocation StoreOperatingLocation { get; set; }
    }
}

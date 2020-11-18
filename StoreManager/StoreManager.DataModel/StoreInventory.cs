using System;
using System.Collections.Generic;

#nullable disable

namespace StoreManager.DataModel
{
    public partial class StoreInventory
    {
        public int StoreId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public int? Threshold { get; set; }

        public virtual Product Product { get; set; }
        public virtual Store Store { get; set; }
    }
}

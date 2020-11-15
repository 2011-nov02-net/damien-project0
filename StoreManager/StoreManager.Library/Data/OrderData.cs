using System.Collections.Generic;

using StoreManager.Library.Entity;

namespace StoreManager.Library.Data
{
    public class OrderData : IData
    {
        public long CustomerId { get; set; }
        public long OperatingLocationId { get; set; }
        public Dictionary<long, int> ProductsRequested { get; set; }

        public OrderData() { }

        public OrderData(long customer, long operatingLocationId, Dictionary<long, int> productsRequested) {
            CustomerId = customer;
            OperatingLocationId = operatingLocationId;
            ProductsRequested = productsRequested;
        }

        public OrderData(OrderData data) :
            this(data.CustomerId, data.OperatingLocationId, data.ProductsRequested) {
        }
    }
}

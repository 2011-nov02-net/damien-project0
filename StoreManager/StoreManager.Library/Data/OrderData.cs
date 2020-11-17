using System.Collections.Generic;

using StoreManager.Library.Entity;

namespace StoreManager.Library.Data
{
    public class OrderData : IData
    {
        public int CustomerId { get; set; }
        public int OperatingLocationId { get; set; }
        public Dictionary<int, int> ProductsRequested { get; set; }

        public OrderData() { }

        public OrderData(int customer, int operatingLocationId, Dictionary<int, int> productsRequested) {
            CustomerId = customer;
            OperatingLocationId = operatingLocationId;
            ProductsRequested = productsRequested;
        }

        public OrderData(OrderData data) :
            this(data.CustomerId, data.OperatingLocationId, data.ProductsRequested) {
        }
    }
}

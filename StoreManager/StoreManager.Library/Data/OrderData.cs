using System.Collections.Generic;

using StoreManager.Library.Entity;

namespace StoreManager.Library.Data
{
    public class OrderData : IData
    {
        public Customer Customer { get; set; }
        public long OperatingLocationId { get; set; }
        public Dictionary<long, int> ProductsRequested { get; set; }

        public OrderData(Customer customer, long operatingLocationId, Dictionary<long, int> productsRequested) {
            Customer = customer;
            OperatingLocationId = operatingLocationId;
            ProductsRequested = productsRequested;
        }

        public OrderData(OrderData data) :
            this(data.Customer, data.OperatingLocationId, data.ProductsRequested) {
        }
    }
}

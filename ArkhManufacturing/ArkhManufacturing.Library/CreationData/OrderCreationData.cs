using System;
using System.Collections.Generic;

namespace ArkhManufacturing.Library.CreationData
{
    // TODO: Add comment here
    public class OrderCreationData : ICreationData
    {
        // TODO: Add comment here
        public OrderCreationData(Customer customer, Store store, DateTime orderDate, Dictionary<Product, int> productsRequested) {
            Customer = customer;
            Store = store;
            OrderDate = orderDate;
            ProductsRequested = productsRequested;
        }

        // TODO: Add comment here
        public Customer Customer { get; }
        // TODO: Add comment here
        public Store Store { get; }
        // TODO: Add comment here
        public DateTime OrderDate { get; }
        // TODO: Add comment here
        public Dictionary<Product, int> ProductsRequested { get; }
    }
}

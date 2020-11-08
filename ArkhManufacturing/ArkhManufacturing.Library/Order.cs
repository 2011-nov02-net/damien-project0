using System;
using System.Collections.Generic;

namespace ArkhManufacturing.Library
{
    public class Order : IdentifiableBase
    {
        private static readonly IdGenerator _idGenerator = new IdGenerator();

        // TODO: Add comment here
        public Customer Customer { get; set; }
        // TODO: Add comment here
        public Store StoreLocation { get; set; }
        // TODO: Add comment here
        public DateTime OrderDate { get; set; }
        // TODO: Add comment here
        public Dictionary<Product, int> ProductsRequested { get; set; }

        // TODO: Add comment here
        public Order(Customer customer, Store storeLocation, DateTime orderDate, Dictionary<Product, int> productsRequested) :
            base(_idGenerator) {
            Customer = customer;
            StoreLocation = storeLocation;
            OrderDate = orderDate;
            ProductsRequested = productsRequested;
        }

        // TODO: Add comment here
        public override string ToString() => $"Order#{Id} for {Customer}";
    }
}

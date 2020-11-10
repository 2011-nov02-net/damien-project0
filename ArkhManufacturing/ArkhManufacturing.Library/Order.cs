using System;
using System.Collections.Generic;

namespace ArkhManufacturing.Library
{
    // TODO: Add comment here
    public class Order : IdentifiableBase
    {
        private static readonly IdGenerator _idGenerator = new IdGenerator();

        // TODO: Add comment here
        public Customer Customer { get; set; }
        // TODO: Add comment here
        public Store Store { get; set; }
        // TODO: Add comment here
        public DateTime OrderDate { get; set; }
        // TODO: Add comment here
        public Dictionary<long, int> ProductsRequested { get; set; }

        // TODO: Add comment here
        public Order(Customer customer, Store store, DateTime orderDate, Dictionary<long, int> productsRequested) :
            base(_idGenerator) {
            Customer = customer;
            Store = store;
            OrderDate = orderDate;
            ProductsRequested = productsRequested;
        }

        // TODO: Add comment here
        public override string ToString() => $"Order#{Id} for Customer ID#{Customer}";
    }
}

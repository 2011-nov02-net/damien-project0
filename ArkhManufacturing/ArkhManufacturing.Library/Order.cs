using System;
using System.Collections.Generic;

namespace ArkhManufacturing.Library
{
    public class Order : IdentifiableBase
    {
        private static readonly IdGenerator _idGenerator = new IdGenerator();

        public Customer Customer { get; set; }
        public Store StoreLocation { get; set; }
        public DateTime OrderDate { get; set; }
        public Dictionary<int, int> ProductsRequested { get; set; }

        public Order(Customer customer, Store storeLocation, DateTime orderDate, Dictionary<int, int> productsRequested) :
            base(_idGenerator)
        {
            Customer = customer;
            StoreLocation = storeLocation;
            OrderDate = orderDate;
            ProductsRequested = productsRequested;
        }

        public override string ToString() => $"Order#{Id}:\n  {{\n    StoreLocation: {StoreLocation}\n  }}";
    }
}

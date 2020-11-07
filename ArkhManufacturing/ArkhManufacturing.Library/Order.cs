using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ArkhManufacturing.Library
{
    public class Order : IdentifiableBase
    {
        private static readonly IdGenerator _idGenerator = new IdGenerator();

        public Customer Customer { get; private set; }
        public Store StoreLocation { get; private set; }
        public DateTime OrderDate { get; private set; }
        public Dictionary<int, int> ProductsRequested { get; private set; }

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

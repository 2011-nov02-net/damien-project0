using System;
using System.Collections.Generic;

namespace ArkhManufacturing.Library
{
    // TODO: Add comment here
    public class Order : Identifiable
    {
        private static readonly IdGenerator _idGenerator = new IdGenerator();

        // TODO: Add comment here
        internal Order(Customer targetCustomer, Store targetStore, DateTime orderDate, Dictionary<Product, int> productsRequested) :
            base(_idGenerator) {
            TargetCustomer = targetCustomer;
            TargetStore = targetStore;
            OrderDate = orderDate;
            ProductsRequested = productsRequested;
        }

        // TODO: Add comment here
        public Customer TargetCustomer { get; set; }
        // TODO: Add comment here
        public Store TargetStore { get; set; }
        // TODO: Add comment here
        public DateTime OrderDate { get; set; }
        // TODO: Add comment here
        public Dictionary<Product, int> ProductsRequested { get; set; }

        // TODO: Add comment here
        public override string ToString() => $"Order#{Id} for Customer ID#{TargetCustomer}";

        // TODO: Add comment here
        public override bool Equals(object obj) {
            if(obj is Order) {
                return (obj as Order).Id == Id;
            } else {
                return false;
            }
        }

        // TODO: Add comment here
        public override int GetHashCode() => HashCode.Combine(Id, TargetCustomer, TargetStore, OrderDate, ProductsRequested);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace ArkhManufacturing.Library
{
    public class Franchise
    {
        public List<Store> Stores { get; }
        public List<Order> Orders
        {
            get
            {
                List<Order> orders = new List<Order>();
                Stores.ForEach(store => orders.AddRange(store.Orders));
                return orders;
            }
        }
        public List<Customer> Customers { get; }

        public Franchise() {
            Stores = new List<Store>();
            Customers = new List<Customer>();
        }

        public Franchise(List<Product> productCatalog, List<Customer> customers) {
            Stores = new List<Store>();
            Customers = customers;
        }

        // Customer customer, Store storeLocation, DateTime orderDate, Dictionary<Product, int> productsRequested
        public void AddOrder(long customerId, long? storeId, Dictionary<long, int> productsRequested) {
            Customer targetCustomer = Customers.First(c => c.Id == customerId);
            // TODO: Make sure the target store is NOT null
            Store targetStore = Stores.First(s =>
            {
                if (storeId == null)
                    return s.Location == targetCustomer.DefaultStoreLocation;
                else return s.Id == storeId;
            });

            // TODO: Come back later and simplify this in LINQ, perhaps
            Dictionary<Product, int> orderProductsRequested = new Dictionary<Product, int>();

            foreach (var kv in productsRequested) {
                Product targetProduct = targetStore.Inventory.Select(productCountPair => productCountPair.Key).First(p => p.Id == kv.Key);
                orderProductsRequested.Add(targetProduct, kv.Value);
            }

            Order order = new Order(targetCustomer, targetStore, DateTime.Now, orderProductsRequested);
            targetStore.SubmitOrder(order);
        }

        public void AddCustomer(Customer customer) {
            if (Customers.Select(c => c.Name).Count(n => n == customer.Name) == 0)
                Customers.Add(customer);
        }

        public void AddStore(Store store) {
            if (Stores.First(s => s.Id == store.Id) == null)
                Stores.Add(store);
        }

        public Customer GetCustomerById(long customerId) => Customers.FirstOrDefault(c => c.Id == customerId);

        public List<Customer> GetCustomersByName(string name) => Customers.Where(c => c.Name.ToString() == name).ToList();

        public Store GetStoreById(long storeId) => Stores.First(s => s.Id == storeId);

        public List<Order> GetCustomerOrderHistory(long customerId) => Orders.Where(order => order.Customer.Id == customerId).ToList();

        public List<Order> GetStoreOrderHistory(long storeId) => Stores.First(store => store.Id == storeId).Orders;
    }
}

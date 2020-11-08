using System;
using System.Collections.Generic;
using System.Linq;

namespace ArkhManufacturing.Library
{
    // TODO: Add comment here
    public class Franchise
    {
        // TODO: Add comment here
        public List<Store> Stores { get; }
        // TODO: Add comment here
        public List<Order> Orders
        {
            get
            {
                List<Order> orders = new List<Order>();
                Stores.ForEach(store => orders.AddRange(store.Orders));
                return orders;
            }
        }
        // TODO: Add comment here
        public List<Customer> Customers { get; }

        // TODO: Add comment here
        public Franchise() {
            Stores = new List<Store>();
            Customers = new List<Customer>();
        }

        // TODO: Add comment here
        public Franchise(List<Product> productCatalog, List<Customer> customers) {
            Stores = new List<Store>();
            Customers = customers;
        }

        // TODO: Add comment here
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
            // Dictionary<Product, int> orderProductsRequested = new Dictionary<Product, int>();
            // 
            // foreach (var kv in productsRequested) {
            //     Product targetProduct = targetStore.Inventory.Select(productCountPair => productCountPair.Key).First(p => p.Id == kv.Key);
            //     orderProductsRequested.Add(targetProduct, kv.Value);
            // }

            Dictionary<Product, int> orderProductsRequested = productsRequested.ToDictionary(kv0 => targetStore.Inventory.First(kv1 => kv0.Key == kv1.Key.Id).Key, kv => kv.Value);

            Order order = new Order(targetCustomer, targetStore, DateTime.Now, orderProductsRequested);
            targetStore.SubmitOrder(order);
        }

        // TODO: Add comment here
        public void AddCustomer(Customer customer) {
            if (Customers.Select(c => c.Name).Count(n => n == customer.Name) == 0)
                Customers.Add(customer);
        }

        // TODO: Add comment here
        public void AddStore(Store store) {
            if (Stores.First(s => s.Id == store.Id) == null)
                Stores.Add(store);
        }
        
        // TODO: Add comment here
        public Customer GetCustomerById(long customerId) => Customers.FirstOrDefault(c => c.Id == customerId);

        // TODO: Add comment here
        public List<Customer> GetCustomersByName(string name) => Customers.Where(c => c.Name.ToString() == name).ToList();

        // TODO: Add comment here
        public Store GetStoreById(long storeId) => Stores.First(s => s.Id == storeId);

        // TODO: Add comment here
        public List<Order> GetCustomerOrderHistory(long customerId) => Orders.Where(order => order.Customer.Id == customerId).ToList();

        // TODO: Add comment here
        public List<Order> GetStoreOrderHistory(long storeId) => Stores.First(store => store.Id == storeId).Orders;
    }
}

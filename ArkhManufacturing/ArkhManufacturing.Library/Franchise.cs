﻿using System;
using System.Collections.Generic;
using System.Linq;
using ArkhManufacturing.Library.Exceptions;

namespace ArkhManufacturing.Library
{
    // TODO: Add comment here
    public class Franchise
    {
        // TODO: Add comment here
        public List<Store> Stores { get; internal set; }
        // TODO: Add comment here
        public List<Customer> Customers { get; internal set; }
        // TODO: Add comment here
        public List<Location> Locations { get; internal set; }
        // TODO: Add comment here
        public List<Product> Products { get; internal set; }

        // TODO: Add comment here
        public Franchise() {
            Stores = new List<Store>();
            Customers = new List<Customer>();
            Locations = new List<Location>();
            Products = new List<Product>();
        }

        // TODO: Add comment here
        public Franchise(List<Customer> customers, List<Location> locations, List<Product> products) {
            Stores = new List<Store>();
            Customers = customers;
            Locations = locations;
            Products = products;
        }

        // TODO: Add comment here
        public Franchise(List<Store> stores, List<Customer> customers, List<Location> locations, List<Product> products) {
            Stores = stores;
            Customers = customers;
            Locations = locations;
            Products = products;
        }

        #region Customer CRUD Operations

        // TODO: Add comment here
        public long CreateCustomer(string firstName, string lastName) {
            var customerName = new CustomerName(firstName, lastName);
            Customer customer = new Customer(customerName, -1);
            Customers.Add(customer);
            return customer.Id;
        }

        // TODO: Add comment here
        public long CreateCustomer(string firstName, string lastName, string planet, string province, string city) {
            var customerName = new CustomerName(firstName, lastName);
            var location = new Location(planet, province, city);
            Customer customer = new Customer(customerName, location.Id);
            Customers.Add(customer);
            Locations.Add(location);
            return customer.Id;
        }

        // TODO: Add comment here
        public long CreateCustomer(CustomerName customerName, Location defaultStoreLocation) {
            long storeId = -1;

            if (defaultStoreLocation != null)
                storeId = defaultStoreLocation.Id;

            var customer = new Customer(customerName, storeId);
            Customers.Add(customer);
            if(Locations.Count > 0) {
                if(Locations.FirstOrDefault(l => l == defaultStoreLocation) == null) {
                    Locations.Add(defaultStoreLocation);
                }
            }
            return customer.Id;
        }

        // TODO: Add comment here
        public List<Customer> GetCustomers() => Customers;

        // TODO: Add comment here
        public List<Customer> GetCustomersByName(string customerName) => Customers.Where(c => c.Name.ToString() == customerName).ToList();

        // TODO: Add comment here
        public Customer GetCustomerById(long customerId) {
            var customer = Customers.FirstOrDefault(c => c.Id == customerId);
            return customer ?? throw new NonExistentIndentifiableException(customerId);
        }

        // TODO: Add comment here
        public void UpdateCustomer(long customerId, string firstName, string lastName) {
            var customer = GetCustomerById(customerId);
            if (firstName != null)
                customer.Name.FirstName = firstName;
            if (lastName != null)
                customer.Name.LastName = lastName;
        }

        // TODO: Add comment here
        public void UpdateCustomer(long customerId, long locationId) {
            var customer = GetCustomerById(customerId);
            var location = GetLocationById(locationId);
            customer.DefaultStoreLocation = location.Id;
        }

        // TODO: Add comment here
        public void DeleteCustomer(long customerId) {
            var customer = GetCustomerById(customerId);
            Customers.Remove(customer);
        }

        #endregion

        #region Store CRUD Operations

        // Create
        // TODO: Add comment here
        public long CreateStore(string name, int productCountThreshold, Location location) {
            var store = new Store(this, name, productCountThreshold, location);
            Stores.Add(store);
            return store.Id;
        }

        // TODO: Add comment here
        public long CreateStore(string name, int productCountThreshold, Location location, Dictionary<long, int> inventory) {
            var store = new Store(this, name, productCountThreshold, location, null, inventory);
            Stores.Add(store);
            return store.Id;
        }

        // Read
        // TODO: Add comment here
        public Store GetStoreById(long storeId) {
            var store = Stores.FirstOrDefault(s => s.Id == storeId);
            return store ?? throw new NonExistentIndentifiableException(storeId);
        }

        // TODO: Add comment here
        public List<Store> GetStores() => Stores;

        // TODO: Add comment here
        public List<Store> GetStoresByName(string storeName) => Stores.Where(s => s.Name == storeName).ToList();

        // TODO: Add comment here
        public Store GetStoreByLocationId(long locationId) {
            return Stores.First(s => s.Location.Id == locationId);
        }

        // Update
        // TODO: Add comment here
        public void UpdateStore(long storeId, string name = null, int? productCountThreshold = null, Location location = null) {
            var store = GetStoreById(storeId);
            if (name != null)
                store.Name = name;
            if (productCountThreshold != null)
                store.ProductCountThreshold = productCountThreshold.Value;
            if (location != null)
                store.Location = location;
        }

        // Delete
        // TODO: Add comment here
        public void DeleteStore(long storeId) {
            var store = GetStoreById(storeId);
            Stores.Remove(store);
        }

        // TODO: Add comment here
        public void DeleteStoreByLocation(long locationId) {
            var store = GetStoreByLocationId(locationId);
            Stores.Remove(store);
        }

        #endregion

        #region Order CRUD Operations

        // Create
        // TODO: Add comment here
        public long CreateOrder(long customerId, long storeId, Dictionary<long, int> productsRequested) {
            Customer targetCustomer = GetCustomerById(customerId);
            Store targetStore = GetStoreById(storeId);

            // TODO: Ensure the LINQ query works as intended
            #region Old Products Requested Code
            // Dictionary<Product, int> orderProductsRequested = new Dictionary<Product, int>();
            // 
            // foreach (var kv in productsRequested) {
            //     Product targetProduct = targetStore.Inventory.Select(productCountPair => productCountPair.Key).First(p => p.Id == kv.Key);
            //     orderProductsRequested.Add(targetProduct, kv.Value);
            // }
            #endregion

            Order order = new Order(targetCustomer, targetStore, DateTime.Now, productsRequested);
            targetStore.SubmitOrder(order);
            return order.Id;
        }

        // Read
        // TODO: Add comment here
        public Order GetOrderById(long orderId) {
            var orders = GetOrders();
            var order = orders.FirstOrDefault(o => o.Id == orderId);
            return order ?? throw new NonExistentIndentifiableException(orderId);
        }

        // TODO: Add comment here
        public List<Order> GetOrders() {
            List<Order> orders = new List<Order>();
            Stores.ForEach(store => orders.AddRange(store.Orders));
            return orders;
        }

        // TODO: Add comment here
        public List<Order> GetOrdersByCustomerId(long customerId) {
            var customer = GetCustomerById(customerId);
            var orders = GetOrders();
            return orders.Where(o => o.Customer == customer).ToList();
        }

        // TODO: Add comment here
        public List<Order> GetOrdersFromStoreId(long storeId) {
            var store = GetStoreById(storeId);
            return store.Orders;
        }

        // Update
        // TODO: Add comment here
        public void UpdateOrder(long orderId, Dictionary<long, int> productsRequested) {
            var order = GetOrderById(orderId);
            var store = GetStoreByLocationId(order.Store.Id);
            if (productsRequested != null) {
                order.ProductsRequested = productsRequested;

                foreach(var kv in productsRequested) {
                    store.Inventory[kv.Key] += kv.Value;
                }
            }
        }

        // Delete
        // TODO: Add comment here
        public void DeleteOrderById(long orderId) {
            var order = GetOrderById(orderId);
            var store = Stores.First(s => s.Orders.FirstOrDefault(o => o.Id == orderId) != null);
            store.Orders.Remove(order);
        }

        #endregion

        #region Location CRUD Operations
        // Create
        // TODO: Add comment here
        public long CreateLocation(string planet, string province, string city) {
            var location = new Location(planet, province, city);
            Locations.Add(location);
            return location.Id;
        }

        // Read
        // TODO: Add comment here
        public Location GetLocationById(long locationId) {
            var location = Locations.First(l => l.Id == locationId);
            return location;
        }

        // TODO: Add comment here
        public List<Location> GetLocations() => Locations;

        // TODO: Add comment here
        public List<Location> GetLocationsByPlanet(string planetName) => Locations.Where(l => l.Planet == planetName).ToList();

        // TODO: Add comment here
        public List<Location> GetLocationsByProvince(string provinceName) => Locations.Where(l => l.Province == provinceName).ToList();

        // TODO: Add comment here
        public List<Location> GetLocationsByCity(string cityName) => Locations.Where(l => l.City == cityName).ToList();

        // Update
        // TODO: Add comment here
        public void UpdateLocation(long locationId, string planet, string province, string city) {
            var location = GetLocationById(locationId);
            if (planet != null)
                location.Planet = planet;
            if (province != null)
                location.Province = province;
            if (city != null)
                location.City = city;
        }

        // Delete
        // TODO: Add comment here
        public void DeleteLocation(long locationId) {
            var location = GetLocationById(locationId);
            Locations.Remove(location);
        }

        #endregion

        #region Product CRUD Operations

        // Create
        // TODO: Add comment here
        public long CreateProduct(string productName, double price, double? discount = null) {
            var product = new Product(productName, price, discount);
            Products.Add(product);
            return product.Id;
        }

        // TODO: Add comment here
        public long CreateProduct(long storeId, int count, string productName, double price, double? discount = null) {
            var store = GetStoreById(storeId);
            var product = new Product(productName, price, discount);
            store.AddProduct(product, count);
            return product.Id;
        }

        // Read
        // TODO: Add comment here
        public Product GetProductById(long productId) {
            var product = Products.FirstOrDefault(p => p.Id == productId);
            return product ?? throw new NonExistentIndentifiableException(productId);
        }

        // TODO: Add comment here
        public List<Product> GetProducts() => Products;

        // Update
        // TODO: Add comment here
        public void UpdateProduct(long productId, string productName, double? price, double? discount) {
            var product = GetProductById(productId);
            if (!string.IsNullOrWhiteSpace(productName))
                product.Name = productName;
            if (price != null)
                product.Price = price.Value;
            if (discount != null)
                product.Discount = discount.Value;
        }

        // Delete
        // TODO: Add comment here
        public void DeleteProductById(long productId) {
            var product = GetProductById(productId);
            Products.Remove(product);
            foreach (var store in Stores.Where(s => s.Inventory.Keys.Contains(productId))) {
                store.Inventory.Remove(productId);
            }
        }

        #endregion
    }
}

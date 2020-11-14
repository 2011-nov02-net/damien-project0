using System;
using System.Collections.Generic;
using System.Linq;

using ArkhManufacturing.Library.CreationData;
using ArkhManufacturing.Library.Creator;
using ArkhManufacturing.Library.Creators;
using ArkhManufacturing.Library.Exception;

namespace ArkhManufacturing.Library
{
    // TODO: Add comment here
    public class Franchise
    {
        private Dictionary<Type, long> _maxIds;
        private Dictionary<Type, ICreator<Identifiable>> _creators;

        // TODO: Add comment here
        public List<Store> Stores { get; internal set; }
        // TODO: Add comment here
        public List<Customer> Customers { get; internal set; }
        // TODO: Add comment here
        public List<Order> Orders { get; internal set; }
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

            Init();
        }

        // TODO: Add comment here
        public Franchise(List<Customer> customers, List<Location> locations, List<Product> products) {
            Stores = new List<Store>();
            Customers = customers;
            Locations = locations;
            Products = products;

            Init();
        }

        // TODO: Add comment here
        public Franchise(List<Store> stores, List<Customer> customers, List<Order> orders, List<Location> locations, List<Product> products) {
            Stores = stores;
            Customers = customers;
            Locations = locations;
            Orders = orders;
            Products = products;

            Init();
        }

        // TODO: Add comment here
        private void Init() {
            _maxIds = new Dictionary<Type, long>
            {
                { typeof(Customer), -1 },
                { typeof(Store), -1 },
                { typeof(Product), -1 },
                { typeof(Order), -1 },
                { typeof(Location), -1 }
            };

            _creators = new Dictionary<Type, ICreator<Identifiable>>
            {
                { typeof(Customer), new CustomerCreator() },
                { typeof(Store), new StoreCreator() },
                { typeof(Order), null },
                { typeof(Product), null },
                { typeof(Location), null }
            };
        }

        private void NewIdCheck<T>(long id)
            where T : Identifiable {
            bool newIdGreater = _maxIds[typeof(T)] < id;
            if (newIdGreater)
                _maxIds[typeof(T)] = id;
        }

        private void UpdateMaxId<T>(long id)
            where T : Identifiable {
            long maxId = _maxIds[typeof(T)];
            if (maxId == id)
                _maxIds[typeof(T)]--;
        }

        // TODO: Add comment here
        public long GetMaxId<T>()
            where T : Identifiable {
            return _maxIds[typeof(T)];
        }

        #region Generic CRUD Methods

        /*  How would the paramters be entered? CreationData 
         *      and something that consumes the CreationData and produces the item?
         *      
         *  ICreatable:
         *      Identifiable Create(ICreationData creationData)
         *  
         */

        public void Create<T>(ICreationData creationData)
            where T : Identifiable, new() {
            // Alright, how will we call create on the generic type?
            // Use the ICreator interface to create 
            var newItem = _creators[typeof(T)].Create(creationData) as T;
            NewIdCheck<T>(newItem.Id);
            // Add the item
        }

        public T GetById<T>(long id) {

        }

        //  public long Create<T>()

        #endregion

        #region Customer CRUD Operations

        // TODO: Add comment here
        public long CreateCustomer(string firstName, string lastName) {
            Customer customer = new Customer(firstName, lastName, null);
            Customers.Add(customer);

            NewIdCheck<Customer>(customer.Id);

            return customer.Id;
        }

        // TODO: Add comment here
        public long CreateCustomer(string firstName, string lastName, string planet, string province, string city) {
            long locationId = CreateLocation(planet, province, city);
            Customer customer = new Customer(firstName, lastName, GetLocationById(locationId));
            Customers.Add(customer);

            NewIdCheck<Customer>(customer.Id);

            return customer.Id;
        }

        // TODO: Add comment here
        public long CreateCustomer(string firstName, string lastName, Location defaultStoreLocation) {
            long locationId = -1;

            if (defaultStoreLocation != null)
                locationId = defaultStoreLocation.Id;

            var customer = new Customer(firstName, lastName, locationId);
            Customers.Add(customer);

            NewIdCheck<Customer>(customer.Id);

            return customer.Id;
        }

        // TODO: Add comment here
        public List<Customer> GetCustomers() => Customers;

        // TODO: Add comment here
        public List<Customer> GetCustomersByName(string customerName) => Customers.Where(c => c.FullName == customerName).ToList();

        // TODO: Add comment here
        public Customer GetCustomerById(long customerId) {
            var customer = Customers.FirstOrDefault(c => c.Id == customerId);
            return customer ?? throw new NonExistentIndentifiableException(customerId);
        }

        // TODO: Add comment here
        public void UpdateCustomer(long customerId, string firstName, string lastName) {
            var customer = GetCustomerById(customerId);
            if (firstName != null)
                customer.FirstName = firstName;
            if (lastName != null)
                customer.LastName = lastName;
        }

        // TODO: Add comment here
        public void UpdateCustomer(long customerId, long locationId) {
            var customer = GetCustomerById(customerId);
            var location = GetLocationById(locationId);
            customer.DefaultStoreLocation = location.Id;
        }

        // TODO: Add comment here
        public void DeleteCustomer(long customerId) {
            // First, check if the customer even exists
            var customer = GetCustomerById(customerId);
            Customers.Remove(customer);

            // Update the max id if they are equal
            UpdateMaxId<Customer>(customerId);
        }

        #endregion

        #region Store CRUD Operations

        // Create
        // TODO: Add comment here
        public long CreateStore(string name, int productCountThreshold, Location location) {
            var store = new Store(this, name, productCountThreshold, location.Id);
            Stores.Add(store);

            NewIdCheck<Store>(store.Id);

            return store.Id;
        }

        // TODO: Add comment here
        public long CreateStore(string name, int productCountThreshold, Location location, Dictionary<long, int> inventory) {
            var store = new Store(this, name, productCountThreshold, location.Id, null, inventory);
            Stores.Add(store);

            NewIdCheck<Store>(store.Id);

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
            // Check if the location exists
            var location = GetLocationById(locationId);
            // Check to see the first store that has the location
            return Stores.First(s => s.Location == location);
        }

        // Update
        // TODO: Add comment here
        public void UpdateStore(long storeId, string name = null, int? productCountThreshold = null, Location location = null) {
            var store = GetStoreById(storeId);
            if (name != null)
                store.Name = name;
            if (productCountThreshold != null)
                store.ProductCountThreshold = productCountThreshold.Value;
            store.Location = location;
        }

        // Delete
        // TODO: Add comment here
        public void DeleteStore(long storeId) {
            var store = GetStoreById(storeId);
            Stores.Remove(store);

            UpdateMaxId<Store>(storeId);
        }

        // TODO: Add comment here
        public void DeleteStoreByLocation(long locationId) {
            var store = GetStoreByLocationId(locationId);
            UpdateMaxId<Store>(store.Id);
            Stores.Remove(store);
        }

        #endregion

        #region Order CRUD Operations

        // Create
        // TODO: Add comment here
        public long CreateOrder(long customerId, long storeId, Dictionary<long, int> productsRequested) {
            Customer targetCustomer = GetCustomerById(customerId);
            Store targetStore = GetStoreById(storeId);
            Order order = new Order(targetCustomer.Id, targetStore.Id, DateTime.Now, productsRequested);

            try {
                targetStore.SubmitOrder(order);
            } catch (System.Exception ex) {
                if (ex is ProductNotOfferedException || ex is ProductRequestExcessiveException) {
                    // Order Failed
                    return -1;
                }
            }

            UpdateMaxId<Order>(order.Id);

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
            List<long> orderIds = new List<long>();
            Stores.ForEach(store => orderIds.AddRange(store.Orders));
            return orderIds.Select(orderId => Orders.First(o => o.Id == orderId)).ToList();
        }

        // TODO: Add comment here
        public List<Order> GetOrdersByCustomerId(long customerId) {
            var customer = GetCustomerById(customerId);
            var orders = GetOrders();
            return orders.Where(o => o.CustomerId == customer.Id).ToList();
        }

        // TODO: Add comment here
        public List<Order> GetOrdersFromStoreId(long storeId) {
            var store = GetStoreById(storeId);
            return store.Orders.Select(orderId => Orders.First(o => o.Id == orderId)).ToList();
        }

        // Update
        // TODO: Add comment here
        public void UpdateOrder(long orderId, Dictionary<long, int> productsRequested) {
            var order = GetOrderById(orderId);
            var store = GetStoreByLocationId(order.StoreId);
            if (productsRequested != null) {
                order.ProductsRequested = productsRequested;

                foreach (var kv in productsRequested) {
                    store.Inventory[kv.Key] += kv.Value;
                }
            }
        }

        // Delete
        // TODO: Add comment here
        public void DeleteOrderById(long orderId) {
            var order = GetOrderById(orderId);
            var store = Stores.First(s => s.Orders.FirstOrDefault(o => o == orderId) != -1);
            store.Orders.Remove(order.Id);
            Orders.Remove(order);
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

            bool newProductIdGreater = _maxIds[typeof(Product)] < product.Id;
            if (newProductIdGreater)
                _maxIds[typeof(Product)] = product.Id;

            return product.Id;
        }

        // TODO: Add comment here
        public long CreateProduct(long storeId, int count, string productName, double price, double? discount = null) {
            var store = GetStoreById(storeId);
            var product = new Product(productName, price, discount);
            var productId = CreateProduct(productName, price, discount);
            store.AddProduct(GetProductById(productId), count);

            bool newIdGreater = _maxIds[typeof(Store)] < store.Id;
            if (newIdGreater)
                _maxIds[typeof(Store)] = store.Id;

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

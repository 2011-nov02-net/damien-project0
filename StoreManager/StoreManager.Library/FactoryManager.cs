using System;
using System.Collections.Generic;
using System.Linq;

using StoreManager.Library.Data;
using StoreManager.Library.Entity;
using StoreManager.Library.Factory;

namespace StoreManager.Library
{
    internal class FactoryManager
    {
        private readonly CustomerFactory _customerFactory;
        private readonly OrderFactory _orderFactory;
        private readonly ProductFactory _productFactory;
        private readonly AddressFactory _addressFactory;
        private readonly StoreFactory _storeFactory;
        private readonly OperatingLocationFactory _operatingLocationFactory;

        public List<Store> Stores { get => _storeFactory.Items; }
        public List<Order> Orders { get => _orderFactory.Items; }
        public List<Customer> Customers { get => _customerFactory.Items; }
        public List<Product> Products { get => _productFactory.Items; }
        public List<Address> Addresses { get => _addressFactory.Items; }
        public List<OperatingLocation> OperatingLocations { get => _operatingLocationFactory.Items; }

        public FactoryManager() {
            _customerFactory = new CustomerFactory();
            _orderFactory = new OrderFactory();
            _productFactory = new ProductFactory();
            _addressFactory = new AddressFactory();
            _storeFactory = new StoreFactory();
            _operatingLocationFactory = new OperatingLocationFactory();
        }

        public FactoryManager(DataBundle dataBundle) :
            this() {
            // TODO: Finish this part when a list of existing stores is passed in
            // with the data bundle passed in, now the data can be assorted into each of the objects
            // Id lifespan is only of the lifetime of the application; they are reassigned on loading. 
            // The ids are only used in the location, and not by any of the storage items
            dataBundle.StoreData.ForEach(sd => Create(typeof(Store), sd));
            dataBundle.OrderData.ForEach(od => Create(typeof(Order), od));
            dataBundle.CustomerData.ForEach(cd => Create(typeof(Customer), cd));
            dataBundle.OperatingLocationData.ForEach(old => Create(typeof(OperatingLocation), old));
            dataBundle.ProductData.ForEach(pd => Create(typeof(Product), pd));
            dataBundle.AddressData.ForEach(ad => Create(typeof(Address), ad));
        }

        public DataBundle BundleData
        {
            get
            {
                var storesData = Stores.Select(s => s.Data).ToList();
                var ordersData = Orders.Select(o => o.Data).ToList();
                var customersData = Customers.Select(c => c.Data).ToList();
                var addressesData = Addresses.Select(a => a.Data).ToList();
                var operatingLocationsData = OperatingLocations.Select(ol => ol.Data).ToList();
                var productsData = Products.Select(p => p.Data).ToList();
                var dataBundle = new DataBundle(storesData, ordersData, customersData, addressesData, operatingLocationsData, productsData);
                return dataBundle;
            }
        }

        public bool Any(Type type) {
            if (type == typeof(Customer)) {
                return _customerFactory.Items.Count == 0;
            } else if (type == typeof(Order)) {
                return _orderFactory.Items.Count == 0;
            } else if (type == typeof(Address)) {
                return _addressFactory.Items.Count == 0;
            } else if (type == typeof(Store)) {
                return _storeFactory.Items.Count == 0;
            } else if (type == typeof(Product)) {
                return _productFactory.Items.Count == 0;
            } else if (type == typeof(OperatingLocation)) {
                return _operatingLocationFactory.Items.Count == 0;
            } else throw new ArgumentException($"Invalid type passed in; got {type}");
        }

        public long MaxId(Type type) {
            if (type == typeof(Customer)) {
                return _customerFactory.Items.Max(c => c.Id);
            } else if (type == typeof(Order)) {
                return _orderFactory.Items.Max(c => c.Id);
            } else if (type == typeof(Address)) {
                return _addressFactory.Items.Max(c => c.Id);
            } else if (type == typeof(Store)) {
                return _storeFactory.Items.Max(c => c.Id);
            } else if (type == typeof(Product)) {
                return _productFactory.Items.Max(c => c.Id);
            } else if (type == typeof(OperatingLocation)) {
                return _operatingLocationFactory.Items.Max(c => c.Id);
            } else throw new ArgumentException($"Invalid type passed in; got {type}");
        }

        public long Create(Type type, IData data) {
            if (type == typeof(Customer)) {
                return _customerFactory.Create(data);
            } else if (type == typeof(Order)) {
                return _orderFactory.Create(data);
            } else if (type == typeof(Address)) {
                return _addressFactory.Create(data);
            } else if (type == typeof(Store)) {
                return _storeFactory.Create(data);
            } else if (type == typeof(Product)) {
                return _productFactory.Create(data);
            } else if (type == typeof(OperatingLocation)) {
                return _operatingLocationFactory.Create(data);
            } else throw new ArgumentException($"Invalid type passed in; got {type}");
        }

        public SEntity Get(Type type, long id) {
            if (type == typeof(Customer)) {
                return _customerFactory.Get(id);
            } else if (type == typeof(Order)) {
                return _orderFactory.Get(id);
            } else if (type == typeof(Address)) {
                return _addressFactory.Get(id);
            } else if (type == typeof(Store)) {
                return _storeFactory.Get(id);
            } else if (type == typeof(Product)) {
                return _productFactory.Get(id);
            } else if (type == typeof(OperatingLocation)) {
                return _operatingLocationFactory.Get(id);
            } else throw new ArgumentException($"Invalid type passed in; got {type}");
        }

        public void Update(Type type, long id, IData data) {
            if (type == typeof(Customer)) {
                _customerFactory.Update(id, data);
            } else if (type == typeof(Order)) {
                _orderFactory.Update(id, data);
            } else if (type == typeof(Address)) {
                _addressFactory.Update(id, data);
            } else if (type == typeof(Store)) {
                _storeFactory.Update(id, data);
            } else if (type == typeof(Product)) {
                _productFactory.Update(id, data);
            } else if (type == typeof(OperatingLocation)) {
                _operatingLocationFactory.Update(id, data);
            } else throw new ArgumentException($"Invalid type passed in; got {type}");
        }

        public void Delete(Type type, long id) {
            if (type == typeof(Customer)) {
                _customerFactory.Delete(id);
            } else if (type == typeof(Order)) {
                _orderFactory.Delete(id);
            } else if (type == typeof(Address)) {
                _addressFactory.Delete(id);
            } else if (type == typeof(Store)) {
                _storeFactory.Delete(id);
            } else if (type == typeof(Product)) {
                _productFactory.Delete(id);
            } else if (type == typeof(OperatingLocation)) {
                _operatingLocationFactory.Delete(id);
            } else throw new ArgumentException($"Invalid type passed in; got {type}");
        }
    }
}

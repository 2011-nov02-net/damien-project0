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
        private readonly Dictionary<Type, IFactory<SEntity>> _typeFactories;

        public FactoryManager() {
            _typeFactories = new Dictionary<Type, IFactory<SEntity>>
            {
                { typeof(Store), new StoreFactory() as IFactory<SEntity> },
                { typeof(Order), new OrderFactory() as IFactory<SEntity> },
                { typeof(Customer), new CustomerFactory() as IFactory<SEntity> },
                { typeof(Product), new ProductFactory() as IFactory<SEntity> },
                { typeof(Address), new AddressFactory() as IFactory<SEntity> },
                { typeof(OperatingLocation), new OperatingLocationFactory() as IFactory<SEntity> },
            };
        }

        public FactoryManager(DataBundle dataBundle) :
            this() {
            if (dataBundle is null)
                return;
            // with the data bundle passed in, now the data can be assorted into each of the objects
            // Id lifespan is only of the lifetime of the application; they are reassigned on loading. 
            // The ids are only used in the location, and not by any of the storage items
            dataBundle.StoreData.ForEach(sd => Create<Store>(sd));
            dataBundle.OrderData.ForEach(od => Create<Order>(od));
            dataBundle.CustomerData.ForEach(cd => Create<Customer>(cd));
            dataBundle.OperatingLocationData.ForEach(old => Create<OperatingLocation>(old));
            dataBundle.ProductData.ForEach(pd => Create<Product>(pd));
            dataBundle.AddressData.ForEach(ad => Create<Address>(ad));
        }

        public DataBundle BundleData
        {
            get
            {
                var storesData = _typeFactories[typeof(Store)].Items.Select(item => item.GetData() as StoreData).ToList();
                var ordersData = _typeFactories[typeof(Order)].Items.Select(item => item.GetData() as OrderData).ToList();
                var customersData = _typeFactories[typeof(Customer)].Items.Select(item => item.GetData() as CustomerData).ToList();
                var addressesData = _typeFactories[typeof(Address)].Items.Select(item => item.GetData() as AddressData).ToList();
                var operatingLocationsData = _typeFactories[typeof(OperatingLocation)].Items.Select(item => item.GetData() as OperatingLocationData).ToList();
                var productsData = _typeFactories[typeof(Product)].Items.Select(item => item.GetData() as ProductData).ToList();
                var dataBundle = new DataBundle(storesData, ordersData, customersData, addressesData, operatingLocationsData, productsData);
                return dataBundle;
            }
        }

        public bool Any<T>()
            where T : SEntity {
            return _typeFactories[typeof(T)].Items.Count > 0;
        }

        public int MaxId<T>()
            where T : SEntity {
            return _typeFactories[typeof(T)].Items.Max(i => i.Id);
        }
        
        public bool IdExists<T>(int id)
            where T : SEntity {
            return _typeFactories[typeof(T)].Items.FirstOrDefault(i => i.Id == id) is not null;
        }

        public int Create<T>(IData data)
            where T : SEntity {
            return _typeFactories[typeof(T)].Create(data);
        }

        public List<T> GetAll<T>()
            where T : SEntity {
            return _typeFactories[typeof(T)].Items.ConvertAll(item => item as T);
        }

        public List<T> GetSome<T>(List<int> ids)
            where T : SEntity {
            return _typeFactories[typeof(T)].Items
                .Where(item => ids.Contains(item.Id))
                .Select(item => item as T)
                .ToList();
        }

        public T Get<T>(int id)
            where T : SEntity {
            return _typeFactories[typeof(T)].Get(id) as T;
        }

        public List<T> GetByName<T>(string name)
            where T : NamedSEntity {
            return _typeFactories[typeof(T)].Items.ConvertAll(item => item as T);
        }

        public void Update<T>(int id, IData data)
            where T : SEntity {
            _typeFactories[typeof(T)].Update(id, data);
        }

        public void Delete<T>(int id)
            where T : SEntity {
            _typeFactories[typeof(T)].Delete(id);
        }
    }
}

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
        private readonly Dictionary<Type, IFactory> _typeFactories;

        public FactoryManager() {
            _typeFactories = new Dictionary<Type, IFactory>
            {
                { typeof(Store), new StoreFactory() },
                { typeof(Order), new OrderFactory() },
                { typeof(Customer), new CustomerFactory() },
                { typeof(Product), new ProductFactory() },
                { typeof(Address), new AddressFactory() },
                { typeof(OperatingLocation), new OperatingLocationFactory() },
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
            // TODO: This might have to be changed at a later date
            get
            {
                var storesData = Factories<Store>().Items.Select(item => item.GetData() as StoreData).ToList();
                var ordersData = Factories<Order>().Items.Select(item => item.GetData() as OrderData).ToList();
                var customersData = Factories<Customer>().Items.Select(item => item.GetData() as CustomerData).ToList();
                var addressesData = Factories<Address>().Items.Select(item => item.GetData() as AddressData).ToList();
                var operatingLocationsData = Factories<OperatingLocation>().Items.Select(item => item.GetData() as OperatingLocationData).ToList();
                var productsData = Factories<Product>().Items.Select(item => item.GetData() as ProductData).ToList();
                var dataBundle = new DataBundle(storesData, ordersData, customersData, addressesData, operatingLocationsData, productsData);
                return dataBundle;
            }
        }

        internal ISEntityFactory<T> Factories<T>()
            where T : SEntity {
            return _typeFactories[typeof(T)] as ISEntityFactory<T>;
        }

        public bool Any<T>()
            where T : SEntity {
            return Factories<T>().Items.Count > 0;
        }

        public int MaxId<T>()
            where T : SEntity {
            
            if (!Any<T>())
                return -1;

            return Factories<T>().Items.Max(i => i.Id);
        }

        public bool IdExists<T>(int id)
            where T : SEntity {

            if (!Any<T>())
                return false;

            return Factories<T>().Items.FirstOrDefault(i => i.Id == id) is not null;
        }

        public int Create<T>(IData data)
            where T : SEntity {
            return Factories<T>().Create(data);
        }

        public List<T> GetAll<T>()
            where T : SEntity {
            return Factories<T>().Items.ConvertAll(item => item as T);
        }

        public List<T> GetSome<T>(List<int> ids)
            where T : SEntity {
            return Factories<T>().Items
                .Where(item => ids.Contains(item.Id))
                .Select(item => item as T)
                .ToList();
        }

        public T Get<T>(int id)
            where T : SEntity {
            var entity = Factories<T>().Get(id);
            return entity as T ?? null;
        }

        public List<T> GetByName<T>(string name)
            where T : NamedSEntity {
            return Factories<T>().Items
                .ConvertAll(item => item as T)
                .Where(i => i.GetName().Contains(name))
                .ToList();
        }

        public void Update<T>(int id, IData data)
            where T : SEntity {
            Factories<T>().Update(id, data);
        }

        public void DeleteAll<T>()
            where T : SEntity {
            Factories<T>().Items.Clear();
        }

        public void Delete<T>(int id)
            where T : SEntity {
            Factories<T>().Delete(id);
        }
    }
}

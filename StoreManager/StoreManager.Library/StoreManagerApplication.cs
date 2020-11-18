using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using StoreManager.Library.Data;
using StoreManager.Library.Database;
using StoreManager.Library.Entity;

namespace StoreManager.Library
{
    /// <summary>
    /// Class exposed to the using application that can manage a set of stores
    /// </summary>
    public class StoreManagerApplication
    {
        private static StoreManagerApplication s_storeManager;

        private readonly IStorageRepository _storage;
        private readonly FactoryManager _factoryManager;

        #region Singleton Methods

        internal static List<T> GetAll<T>()
            where T : SEntity {
            return s_storeManager.GetAllEntities<T>();
        }

        internal static List<T> GetMany<T>(List<int> ids)
            where T : SEntity {
            return s_storeManager.GetManyEntities<T>(ids);
        }

        internal static T Get<T>(int id)
            where T : SEntity {
            return s_storeManager.GetEntity<T>(id);
        }

        public static void Initialize(IStorageRepository storage = null, IConfigurationOptions configurationOptions = null) {
            s_storeManager ??= new StoreManagerApplication(storage, configurationOptions);
        }

        public static bool Any<T>()
            where T : SEntity {
            return s_storeManager.AnyEntity<T>();
        }

        public static int MaxId<T>()
            where T : SEntity {
            return s_storeManager.MaxEntityId<T>() + 1;
        }

        public static bool IdExists<T>(int id)
            where T : SEntity {
            return s_storeManager.EntityIdExists<T>(id);
        }

        public static int Create<T>(IData data)
            where T : SEntity {
            return s_storeManager.CreateEntity<T>(data);
        }

        public static List<IData> GetAllData<T>()
            where T : SEntity {
            return s_storeManager.GetAllEntityData<T>();
        }

        public static List<IData> GetSomeData<T>(List<int> ids)
            where T : SEntity {
            return s_storeManager.GetManyEntityData<T>(ids);
        }

        public static IData GetData<T>(int id)
            where T : SEntity {
            return s_storeManager.GetEntityData<T>(id);
        }

        public static string GetName<T>(int id)
            where T : NamedSEntity {
            return (s_storeManager.GetEntityData<T>(id) as NamedData).Name;
        }

        public static List<int> GetCustomerIdsByName(string name) {
            return s_storeManager.GetByName<Customer>(name).ConvertAll(c => c.Id);
        }

        public static List<int> GetStoreIdsByName(string name) {
            return s_storeManager.GetByName<Store>(name).ConvertAll(s => s.Id);
        }

        public static List<CustomerData> GetCustomerDataByName(string name) {
            return s_storeManager.GetByName<Customer>(name).ConvertAll(c => c.Data);
        }

        public static List<StoreData> GetStoreDataByName(string name) {
            return s_storeManager.GetByName<Store>(name).ConvertAll(s => s.Data);
        }

        public static List<int> GetOrderIdsByCustomerId(int id) {
            var storeData = s_storeManager.GetAllEntityData<Store>().ConvertAll(ed => ed as StoreData);
            List<int> orderIds = new List<int>();
            storeData.ForEach(sd => orderIds.AddRange(sd.OrderIds));
            return orderIds;
        }

        public static void Update<T>(int id, IData data)
            where T : SEntity {
            s_storeManager.UpdateEntity<T>(id, data);
        }

        public static void Delete<T>(int id)
            where T : SEntity {
            s_storeManager.DeleteEntity<T>(id);
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Creates a StoreManager instance
        /// </summary>
        /// <param name="storage">The storage medium in which the application stores its data</param>
        private StoreManagerApplication(IStorageRepository storage, IConfigurationOptions configurationOptions) {
            _storage = storage ?? new DatabaseStorageRepository();
            _storage.Configure(configurationOptions);

            try {
                var dataBundle = _storage?.ReadAsync()?.Result;
                _factoryManager = new FactoryManager(dataBundle);
            } catch (Exception) {
                // TODO: Log the error to disk
            } finally {
                _factoryManager ??= new FactoryManager();
            }
        }

        ~StoreManagerApplication() {
            Task.Run(() => Save());
        }

        private void UpdateEntities<T>()
            where T : SEntity {
            List<T> items = _storage.GetAllAsync<T>().Result;
            if (items.Any()) {
                items.ForEach(item => _factoryManager.Update<T>(item.Id, item.GetData()));
            }                
        }

        private bool AnyEntity<T>()
            where T : SEntity {
            UpdateEntities<T>();
            return _factoryManager.Any<T>();
        }

        private int MaxEntityId<T>()
            where T : SEntity {
            UpdateEntities<T>();
            return _factoryManager.MaxId<T>();
        }

        private bool EntityIdExists<T>(int id)
            where T : SEntity {
            UpdateEntities<T>();
            return _factoryManager.IdExists<T>(id);
        }

        private int CreateEntity<T>(IData data)
            where T : SEntity {
            int itemId = _factoryManager.Create<T>(data);
            var item = _factoryManager.Get<T>(itemId);
            _storage.CreateOneAsync<T>(item);
            return itemId;
        }

        private List<IData> GetAllEntityData<T>()
            where T : SEntity {
            UpdateEntities<T>();
            var entities = GetAllEntities<T>();

            if (!entities.Any())
                return new List<IData>();

            return entities.ConvertAll(e => e.GetData());
        }

        private List<IData> GetManyEntityData<T>(List<int> ids)
            where T : SEntity {
            UpdateEntities<T>();
            var entities = GetManyEntities<T>(ids);

            if (!entities.Any())
                return new List<IData>();

            return entities.ConvertAll(e => e.GetData());
        }

        private IData GetEntityData<T>(int id)
            where T : SEntity {
            UpdateEntities<T>();
            var item = GetEntity<T>(id);
            return item?.GetData();
        }

        private List<T> GetAllEntities<T>()
            where T : SEntity {
            var entities = _storage.GetAllAsync<T>().Result;

            if (!entities.Any())
                return entities;

            entities.ForEach(e => _factoryManager.Update<T>(e.Id, e.GetData()));
            return _factoryManager.GetAll<T>();
        }

        private List<T> GetManyEntities<T>(List<int> ids)
            where T : SEntity {
            UpdateEntities<T>();
            var entities = _storage.GetManyAsync<T>(ids).Result;

            if (!entities.Any())
                return entities;

            entities.ForEach(e => _factoryManager.Update<T>(e.Id, e.GetData()));
            return _factoryManager.GetSome<T>(ids);
        }

        private T GetEntity<T>(int id)
            where T : SEntity {
            UpdateEntities<T>();
            var entity = _storage.GetOneAsync<T>(id)?.Result;
            
            if (entity is null)
                return entity;

            _factoryManager.Update<T>(id, entity.GetData());
            return _factoryManager.Get<T>(id);
        }

        private List<T> GetByName<T>(string name)
            where T : NamedSEntity {
            UpdateEntities<T>();
            var entities = _storage.GetAllAsync<T>().Result;

            if (!entities.Any())
                return entities;

            entities.ForEach(e => _factoryManager.Update<T>(e.Id, e.GetData()));
            return _factoryManager.GetByName<T>(name);
        }

        private void UpdateEntity<T>(int id, IData data)
            where T : SEntity {
            UpdateEntities<T>();
            var item = _factoryManager.Get<T>(id);
            _factoryManager.Update<T>(id, data);
            _storage.UpdateOneAsync<T>(item);
        }

        private void DeleteEntity<T>(int id)
            where T : SEntity {
            UpdateEntities<T>();
            var item = _factoryManager.Get<T>(id);

            _storage.DeleteOneAsync<T>(item);
            _factoryManager.Delete<T>(id);
        }

        private async Task Save() {
            await _storage.WriteAsync(_factoryManager.BundleData);
        }

        #endregion

    }
}

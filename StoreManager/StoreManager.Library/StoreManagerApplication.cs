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

        internal static List<T> GetSome<T>(List<int> ids)
            where  T : SEntity {
            return s_storeManager.GetSomeEntities<T>(ids);
        }

        internal static T Get<T>(int id)
            where T : SEntity {
            return s_storeManager.GetEntity<T>(id);
        }

        public static void Initialize(IStorageRepository storage = null, IConfigurationOptions configurationOptions = null, SaveFrequency saveFrequency = SaveFrequency.Always) {
            s_storeManager ??= new StoreManagerApplication(storage, configurationOptions, saveFrequency);
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
            return s_storeManager.GetSomeEntityData<T>(ids);
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

        public static void Update<T>(int id, IData data)
            where T : SEntity {
            s_storeManager.UpdateEntity<T>(id, data);
        }

        public static void Delete<T>(int id)
            where T : SEntity {
            s_storeManager.DeleteEntity<T>(id);
        }

        #endregion

        private readonly SaveFrequency _saveFrequency;

        #region Instance Methods

        /// <summary>
        /// Creates a StoreManager instance
        /// </summary>
        /// <param name="storage">The storage medium in which the application stores its data</param>
        private StoreManagerApplication(IStorageRepository storage, IConfigurationOptions configurationOptions, SaveFrequency saveFrequency) {
            _storage = storage ?? new DatabaseStorageRepository();
            // TODO: Decide how the connection string is passed in/set
            _storage.Configure(configurationOptions ?? new DatabaseConfigurationOptions(""));
            _saveFrequency = saveFrequency;

            try {
                var dataBundle = _storage?.ReadAsync()?.Result;
                // TODO: Check if this is allowed, or if it will give a NullReferenceException
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
            items.ForEach(item => _factoryManager.Update<T>(item.Id, item.GetData()));
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

            if (_saveFrequency == SaveFrequency.Always) {
                Task.Run(() => Save());
            }

            return itemId;
        }

        private List<IData> GetAllEntityData<T>()
            where T : SEntity {
            return _factoryManager.GetAll<T>().Select(t => t.GetData()).ToList();
        }

        private List<IData> GetSomeEntityData<T>(List<int> ids)
            where T : SEntity {
            return _factoryManager.GetSome<T>(ids).Select(t => t.GetData()).ToList();
        }

        private IData GetEntityData<T>(int id)
            where T : SEntity {
            var item = _factoryManager.Get<T>(id);
            return item.GetData();
        }

        private List<T> GetAllEntities<T>()
            where T : SEntity {
            return _factoryManager.GetAll<T>();
        }

        private List<T> GetSomeEntities<T>(List<int> ids) 
            where T : SEntity {
            return _factoryManager.GetSome<T>(ids);
        }

        private T GetEntity<T>(int id)
            where T : SEntity {
            return _factoryManager.Get<T>(id);
        }

        private List<T> GetByName<T>(string name)
            where T : NamedSEntity {
            return _factoryManager.GetByName<T>(name);
        }

        private void UpdateEntity<T>(int id, IData data)
            where T : SEntity {
            _factoryManager.Update<T>(id, data);

            if (_saveFrequency == SaveFrequency.Always) {
                Task.Run(() => Save());
            }
        }

        private void DeleteEntity<T>(int id)
            where T : SEntity {
            _factoryManager.Delete<T>(id);

            if (_saveFrequency == SaveFrequency.Always) {
                Task.Run(() => Save());
            }
        }

        private async Task Save() {
            // await _storage.Write(_factoryManager.BundleData);
            await Task.Run(() => { /* TODO: Decide how to handle storing the data (other than internally, in the lifetime of the program) */ });
        }

        #endregion

    }
}

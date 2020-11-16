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

        private readonly IStorageRepository<DataBundle> _storage;
        private readonly FactoryManager _factoryManager;

        public static void Initialize(IStorageRepository<DataBundle> storage = null, IConfigurationOptions configurationOptions = null, SaveFrequency saveFrequency = SaveFrequency.Always) {
            s_storeManager ??= new StoreManagerApplication(storage, configurationOptions, saveFrequency);
        }

        public static bool Any<T>()
            where T : SEntity {
            return s_storeManager.AnyEntity<T>();
        }

        public static long MaxId<T>()
            where T : SEntity {
            return s_storeManager.MaxEntityId<T>() + 1;
        }

        public static bool IdExists<T>(long id)
            where T : SEntity {
            return s_storeManager.EntityIdExists<T>(id);
        }

        public static long Create<T>(IData data)
            where T : SEntity {
            return s_storeManager.CreateEntity<T>(data);
        }

        public static List<IData> GetAll<T>()
            where T : SEntity {
            return s_storeManager.GetAllEntities<T>();
        }

        public static List<IData> GetSome<T>(List<long> ids)
            where T : SEntity {
            return s_storeManager.GetSomeEntities<T>(ids);
        }

        public static IData Get<T>(long id)
            where T : SEntity {
            return s_storeManager.GetEntity<T>(id);
        }

        public static string GetName<T>(long id)
            where T : NamedSEntity {
            return (s_storeManager.GetEntity<T>(id) as NamedData).Name;
        }

        public static List<CustomerData> GetCustomersByName(string name) {
            return s_storeManager.GetByName<Customer>(name).ConvertAll(nd => nd as CustomerData);
        }

        public static List<StoreData> GetStoresByName(string name) {
            return s_storeManager.GetByName<Store>(name).ConvertAll(nd => nd as StoreData);
        }

        public static void Update<T>(long id, IData data)
            where T : SEntity {
            s_storeManager.UpdateEntity<T>(id, data);
        }

        public static void Delete<T>(long id)
            where T : SEntity {
            s_storeManager.DeleteEntity<T>(id);
        }

        private readonly SaveFrequency _saveFrequency;

        /// <summary>
        /// Creates a StoreManager instance
        /// </summary>
        /// <param name="storage">The storage medium in which the application stores its data</param>
        private StoreManagerApplication(IStorageRepository<DataBundle> storage, IConfigurationOptions configurationOptions, SaveFrequency saveFrequency) {
            _storage = storage ?? new DatabaseStorageRepository();
            // TODO: Decide how the connection string is passed in/set
            _storage.Configure(configurationOptions ?? new DatabaseConfigurationOptions(""));
            _saveFrequency = saveFrequency;

            try {
                var dataBundle = _storage?.Read().Result;
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

        private bool AnyEntity<T>()
            where T : SEntity {
            return _factoryManager.Any<T>();
        }

        private long MaxEntityId<T>()
            where T : SEntity {
            return _factoryManager.MaxId<T>();
        }

        private bool EntityIdExists<T>(long id)
            where T : SEntity {
            return _factoryManager.IdExists<T>(id);
        }

        private long CreateEntity<T>(IData data)
            where T : SEntity {
            long itemId = _factoryManager.Create<T>(data);

            if (_saveFrequency == SaveFrequency.Always) {
                Task.Run(() => Save());
            }

            return itemId;
        }

        private List<IData> GetAllEntities<T>()
            where T : SEntity {
            return _factoryManager.GetAll<T>().Select(t => t.GetData()).ToList();
        }

        private List<IData> GetSomeEntities<T>(List<long> ids)
            where T : SEntity {
            return _factoryManager.GetSome<T>(ids).Select(t => t.GetData()).ToList();
        }

        private IData GetEntity<T>(long id)
            where T : SEntity {
            var item = _factoryManager.Get<T>(id);
            return item.GetData();
        }

        private List<NamedData> GetByName<T>(string name)
            where T : NamedSEntity {
            return _factoryManager.GetByName<T>(name).ConvertAll(item => item as NamedData);
        }

        private void UpdateEntity<T>(long id, IData data)
            where T : SEntity {
            _factoryManager.Update<T>(id, data);

            if (_saveFrequency == SaveFrequency.Always) {
                Task.Run(() => Save());
            }
        }

        private void DeleteEntity<T>(long id)
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
    }
}

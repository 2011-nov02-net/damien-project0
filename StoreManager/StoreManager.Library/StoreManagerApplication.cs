using System;
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

        public static void Create<T>(IData data)
            where T : SEntity {
            s_storeManager.CreateEntity<T>(data);
        }

        public static IData Get<T>(long id)
            where T : SEntity {
            return s_storeManager.GetEntity<T>(id);
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
        // TODO: Use this when an exception is thrown or something

        /// <summary>
        /// Creates a StoreManager instance
        /// </summary>
        /// <remarks>
        /// </remarks>
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

        private void CreateEntity<T>(IData data)
            where T : SEntity {
            _factoryManager.Create(typeof(T), data);

            if (_saveFrequency == SaveFrequency.Always) {
                Task.Run(() => Save());
            }
        }

        private IData GetEntity<T>(long id)
            where T : SEntity {
            var item = _factoryManager.Get(typeof(T), id) as T;
            return item.GetData();
        }

        private void UpdateEntity<T>(long id, IData data)
            where T : SEntity {
            _factoryManager.Update(typeof(T), id, data);

            if (_saveFrequency == SaveFrequency.Always) {
                Task.Run(() => Save());
            }
        }

        private void DeleteEntity<T>(long id)
            where T : SEntity {
            _factoryManager.Delete(typeof(T), id);

            if(_saveFrequency == SaveFrequency.Always) {
                Task.Run(() => Save());
            }
        }

        private async Task Save() {
            await _storage.Write(_factoryManager.BundleData);
        }
    }
}

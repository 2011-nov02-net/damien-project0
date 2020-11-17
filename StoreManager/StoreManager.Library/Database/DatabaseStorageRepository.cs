using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using StoreManager.DataModel;
using StoreManager.Library.Data;
using StoreManager.Library.Entity;

namespace StoreManager.Library.Database
{
    public class DatabaseStorageRepository : IStorageRepository
    {
        private DbContextOptions<StoreManagerContext> s_contextOptions;

        public void Configure(IConfigurationOptions configurationOptions) {
            if (configurationOptions is not DatabaseConfigurationOptions)
                throw new ArgumentException($"Invalid IConfigurationsOptions argument; got '{configurationOptions.GetType()}' instead.");

            var configOptions = configurationOptions as DatabaseConfigurationOptions;
            var optionsBuilder = new DbContextOptionsBuilder<StoreManagerContext>();
            optionsBuilder.UseSqlServer(configOptions.ConnectionString);
            // TODO: Set up the logging structure here

            s_contextOptions = optionsBuilder.Options;
        }

        public async Task<DataBundle> Read() {
            using var context = new StoreManagerContext(s_contextOptions);
            // Interact with the DataModel Library here
            return await Task.Run(() => new DataBundle());
        }

        public Task<List<T>> ReadAll<T>() where T : SEntity {
            throw new NotImplementedException();
        }

        public Task<T> ReadOne<T>(int id) where T : SEntity {
            throw new NotImplementedException();
        }

        public Task<List<T>> ReadSome<T>(int[] ids) where T : SEntity {
            throw new NotImplementedException();
        }

        public async Task Write(DataBundle dataBundle) {
            using var context = new StoreManagerContext(s_contextOptions);
            // Interact with the DataModel Library here
            await Task.Run(() => { });
        }

        public Task WriteAll<T>(List<T> dataItems) where T : SEntity {
            throw new NotImplementedException();
        }

        public Task WriteOne<T>(int id, T item) where T : SEntity {
            throw new NotImplementedException();
        }

        public Task WriteSome<T>(int[] ids, List<T> dataItems) where T : SEntity {
            throw new NotImplementedException();
        }
    }
}

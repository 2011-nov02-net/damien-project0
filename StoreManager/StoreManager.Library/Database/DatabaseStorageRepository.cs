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
    public class DatabaseStorageRepository : IStorageRepository<DataBundle>
    {
        private readonly DbContextOptions<StoreManagerContext> s_contextOptions;

        public void Configure(IConfigurationOptions configurationOptions) {
            if (configurationOptions is not DatabaseConfigurationOptions)
                throw new ArgumentException($"Invalid IConfigurationsOptions argument; got '{configurationOptions.GetType()}' instead.");

            var configOptions = configurationOptions as DatabaseConfigurationOptions;
            var optionsBuilder = new DbContextOptionsBuilder<StoreManagerContext>();
            optionsBuilder.UseSqlServer(configOptions.ConnectionString);
            // TODO: Set up the logging structure here
        }

        public async Task<DataBundle> Read() {
            using var context = new StoreManagerContext(s_contextOptions);
            // Interact with the DataModel Library here
            return await Task.Run(() => new DataBundle());
        }

        public async Task Write(DataBundle dataBundle) {
            using var context = new StoreManagerContext(s_contextOptions);
            // Interact with the DataModel Library here
            await Task.Run(() => { });
        }
    }
}

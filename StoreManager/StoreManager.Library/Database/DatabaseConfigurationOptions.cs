using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StoreManager.Library.Logger;

namespace StoreManager.Library.Database
{
    public class DatabaseConfigurationOptions : IConfigurationOptions
    {
        public string ConnectionString { get; set; }
        public ILogger Logger { get; set; }

        public DatabaseConfigurationOptions(ILogger logger, string connectionString) {
            Logger = logger;
            ConnectionString = connectionString;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManager.Library.Database
{
    public class DatabaseConfigurationOptions : IConfigurationOptions
    {
        public string ConnectionString { get; set; }

        public DatabaseConfigurationOptions(string connectionString) {
            ConnectionString = connectionString;
        }
    }
}

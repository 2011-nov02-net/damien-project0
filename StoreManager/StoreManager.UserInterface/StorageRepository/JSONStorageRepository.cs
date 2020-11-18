using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

using StoreManager.Library;
using StoreManager.Library.Data;
using StoreManager.Library.Entity;

namespace StoreManager.UserInterface.StorageRepository
{
    class JSONStorageRepository : ISerializer
    {
        private string _filepath;
        public string Filepath
        {
            get => _filepath;
            protected set => _filepath = value;
        }

        public JSONStorageRepository(string filepath) {
            Filepath = filepath;
        }

        public void Configure(IConfigurationOptions configurationOptions) {
            if (configurationOptions is not JSONConfigurationOptions)
                return;

            /*var configOptions */ _ = configurationOptions as JSONConfigurationOptions;
            // TODO: Set up how the configuration options will work here
        }

        public async Task<DataBundle> ReadAsync() {
            string json = File.ReadAllText(_filepath);
            var dataBundle = JsonSerializer.Deserialize<DataBundle>(json);
            return await Task.Run(() => dataBundle);
        }

        public Task WriteAsync(DataBundle dataBundle) {
            throw new System.NotImplementedException();
        }
    }
}

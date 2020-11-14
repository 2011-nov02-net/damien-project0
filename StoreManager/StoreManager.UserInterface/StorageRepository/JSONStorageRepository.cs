using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

using StoreManager.Library;
using StoreManager.Library.Data;

namespace StoreManager.UserInterface.StorageRepository
{
    class JSONStorageRepository : IStorageRepository<DataBundle>
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

        public async Task<DataBundle> Read() {
            string json = File.ReadAllText(_filepath);
            var stores = JsonSerializer.Deserialize<DataBundle>(json);
            return await Task.Run(() => stores);
        }

        public async Task Write(DataBundle dataBundle) {
            await Task.Run(() => { });
        }
    }
}

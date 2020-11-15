using System.Threading.Tasks;

namespace StoreManager.Library
{
    public interface IStorageRepository<T>
    {
        void Configure(IConfigurationOptions configurationOptions);
        Task<T> Read();
        Task Write(T items);
    }
}

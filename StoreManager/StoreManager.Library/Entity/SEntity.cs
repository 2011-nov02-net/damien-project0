using StoreManager.Library.Data;

namespace StoreManager.Library.Entity
{
    public abstract class SEntity
    {
        internal int Id { get; set; }

        internal abstract IData GetData();

        internal SEntity(int id) {
            Id = id;
        }

        internal SEntity(IdGenerator idGenerator) {
            Id = idGenerator.NextId();
        }
    }
}

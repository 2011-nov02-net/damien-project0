using StoreManager.Library.Data;

namespace StoreManager.Library.Entity
{
    public abstract class SEntity
    {
        internal long Id { get; private set; }

        internal abstract IData GetData();

        internal SEntity(IdGenerator idGenerator) {
            Id = idGenerator.NextId();
        }
    }
}

using StoreManager.Library.Data;

namespace StoreManager.Library.Entity
{
    public sealed class Store : NamedSEntity
    {
        private StoreData _data;
        internal StoreData Data
        {
            get => _data;
            set => _data = value;
        }

        internal Store(IdGenerator idGenerator, StoreData data) :
            base(idGenerator) {
            _data = data;
        }

        internal Store(int id, StoreData data) :
            base(id) {
            _data = data;
        }

        internal override IData GetData() => new StoreData(_data);

        internal override string GetName() => Data.Name;
    }
}

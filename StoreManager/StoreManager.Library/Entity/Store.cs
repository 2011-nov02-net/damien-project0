using StoreManager.Library.Data;

namespace StoreManager.Library.Entity
{
    public sealed class Store : SEntity
    {
        private StoreData _data;

        public StoreData Data
        {
            get => _data;
            internal set => _data = value;
        }

        internal Store(IdGenerator idGenerator, StoreData data) :
            base(idGenerator) {
            _data = data;
        }

        internal override IData GetData() => new StoreData(Data);
    }
}

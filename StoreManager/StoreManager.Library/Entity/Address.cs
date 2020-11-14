using StoreManager.Library.Data;

namespace StoreManager.Library.Entity
{
    public sealed class Address : SEntity
    {
        private AddressData _data;

        public AddressData Data
        {
            get => new AddressData(_data);
            internal set => _data = value;
        }

        internal Address(IdGenerator idGenerator, AddressData data) :
            base(idGenerator) {
            _data = data;
        }

        internal override IData GetData() => Data;
    }
}

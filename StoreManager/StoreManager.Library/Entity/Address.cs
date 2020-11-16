using StoreManager.Library.Data;

namespace StoreManager.Library.Entity
{
    public sealed class Address : SEntity
    {
        private AddressData _data;
        internal AddressData Data
        {
            get => _data;
            set => _data = value;
        }

        internal Address(IdGenerator idGenerator, AddressData data) :
            base(idGenerator) {
            _data = data;
        }

        internal override IData GetData() => new AddressData(_data);
    }
}

using StoreManager.Library.Data;

namespace StoreManager.Library.Entity
{
    public sealed class Customer : SEntity
    {
        private CustomerData _data;

        public CustomerData Data
        {
            get => new CustomerData(_data);
            internal set => _data = value;
        }

        internal Customer(IdGenerator idGenerator, CustomerData data) :
            base(idGenerator) {
            _data = data;
        }

        internal override IData GetData() => Data;
    }
}

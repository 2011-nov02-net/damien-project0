using StoreManager.Library.Data;

namespace StoreManager.Library.Entity
{
    public sealed class Customer : NamedSEntity
    {
        private CustomerData _data;
        internal CustomerData Data
        {
            get => _data;
            set => _data = value;
        }

        internal Customer(IdGenerator idGenerator, CustomerData data) :
            base(idGenerator) {
            _data = data;
        }

        internal override IData GetData() => new CustomerData(_data);

        internal override string GetName() => Data.Name;
    }
}

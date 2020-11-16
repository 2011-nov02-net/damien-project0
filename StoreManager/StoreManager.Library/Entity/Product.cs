using StoreManager.Library.Data;

namespace StoreManager.Library.Entity
{
    public sealed class Product : NamedSEntity
    {
        private ProductData _data;
        internal ProductData Data
        {
            get => _data;
            set => _data = value;
        }

        internal Product(IdGenerator idGenerator, ProductData data) :
            base(idGenerator) {
            _data = data;
        }

        internal override IData GetData() => new ProductData(_data);

        internal override string GetName() => Data.Name;
    }
}

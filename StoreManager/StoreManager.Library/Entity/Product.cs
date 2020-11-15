using StoreManager.Library.Data;

namespace StoreManager.Library.Entity
{
    public sealed class Product : SEntity
    {
        private ProductData _data;

        public ProductData Data
        {
            get => _data;
            internal set => _data = value;
        }

        internal Product(IdGenerator idGenerator, ProductData data) :
            base(idGenerator) {
            _data = data;
        }

        internal override IData GetData() => new ProductData(Data);
    }
}

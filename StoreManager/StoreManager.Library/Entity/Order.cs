using StoreManager.Library.Data;

namespace StoreManager.Library.Entity
{
    public sealed class Order : SEntity
    {
        private OrderData _data;
        internal OrderData Data
        {
            get => _data;
            set => _data = value;
        }

        internal Order(IdGenerator idGenerator, OrderData data) :
            base(idGenerator) {
            _data = data;
        }

        internal Order(int id, OrderData data) :
            base(id) {
            _data = data;
        }

        internal override IData GetData() => new OrderData(_data);
    }
}

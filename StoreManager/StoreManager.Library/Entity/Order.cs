using StoreManager.Library.Data;

namespace StoreManager.Library.Entity
{
    public sealed class Order : SEntity
    {
        private OrderData _data;

        public OrderData Data
        {
            get => _data;
            internal set => _data = value;
        }

        internal Order(IdGenerator idGenerator, OrderData data) :
            base(idGenerator) {
            _data = data;
        }

        internal override IData GetData() => new OrderData(Data);
    }
}

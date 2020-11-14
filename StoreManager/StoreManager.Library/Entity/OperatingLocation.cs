using StoreManager.Library.Data;

namespace StoreManager.Library.Entity
{
    public sealed class OperatingLocation : SEntity
    {
        private OperatingLocationData _data;
        public OperatingLocationData Data
        {
            get => _data;
            internal set => _data = value;
        }

        public OperatingLocation(IdGenerator idGenerator, OperatingLocationData data) :
            base(idGenerator) {
            _data = data;
        }

        internal override IData GetData() => new OperatingLocationData(Data);
    }
}

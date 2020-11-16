using StoreManager.Library.Data;

namespace StoreManager.Library.Entity
{
    public sealed class OperatingLocation : SEntity
    {
        private OperatingLocationData _data;
        internal OperatingLocationData Data
        {
            get => _data;
            set => _data = value;
        }

        public OperatingLocation(IdGenerator idGenerator, OperatingLocationData data) :
            base(idGenerator) {
            _data = data;
        }

        internal override IData GetData() => new OperatingLocationData(_data);
    }
}

namespace StoreManager.Library.Data
{
    public class OperatingLocationData : IData
    {
        public long StoreId { get; set; }
        public long AddressId { get; set; }

        public OperatingLocationData() { }

        public OperatingLocationData(long storeId, long addressId) {
            StoreId = storeId;
            AddressId = addressId;
        }

        public OperatingLocationData(OperatingLocationData data) :
            this(data.StoreId, data.AddressId) {
        }
    }
}

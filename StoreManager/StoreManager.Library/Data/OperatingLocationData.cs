namespace StoreManager.Library.Data
{
    public class OperatingLocationData : IData
    {
        public long AddressId { get; set; }

        public OperatingLocationData() { }

        public OperatingLocationData(long addressId) {
            AddressId = addressId;
        }

        public OperatingLocationData(OperatingLocationData data) :
            this(data.AddressId) {
        }
    }
}

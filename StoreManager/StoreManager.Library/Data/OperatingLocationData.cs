namespace StoreManager.Library.Data
{
    public class OperatingLocationData : IData
    {
        public int StoreId { get; set; }
        public int AddressId { get; set; }

        public OperatingLocationData() { }

        public OperatingLocationData(int storeId, int addressId) {
            StoreId = storeId;
            AddressId = addressId;
        }

        public OperatingLocationData(OperatingLocationData data) :
            this(data.StoreId, data.AddressId) {
        }
    }
}

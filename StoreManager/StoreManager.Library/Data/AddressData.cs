namespace StoreManager.Library.Data
{
    public class AddressData : IData
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }

        public AddressData(string addressLine1, string addressLine2, string city, string state, string country, string zipCode) {
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipCode;
        }

        public AddressData(AddressData data) :
            this(data.AddressLine1, data.AddressLine2, data.City, data.State, data.Country, data.ZipCode) {
        }

    }
}

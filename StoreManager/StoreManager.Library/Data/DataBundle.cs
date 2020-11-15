using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManager.Library.Data
{
    public class DataBundle
    {
        public List<StoreData> StoreData { get; set; }
        public List<OrderData> OrderData { get; set; }
        public List<CustomerData> CustomerData { get; set; }
        public List<AddressData> AddressData { get; set; }
        public List<OperatingLocationData> OperatingLocationData { get; set; }
        public List<ProductData> ProductData { get; set; }

        public DataBundle() {
            StoreData = new List<StoreData>();
            OrderData = new List<OrderData>();
            CustomerData = new List<CustomerData>();
            AddressData = new List<AddressData>();
            OperatingLocationData = new List<OperatingLocationData>();
            ProductData = new List<ProductData>();
        }

        public DataBundle(List<StoreData> storeData, List<OrderData> orderData, List<CustomerData> customerData, List<AddressData> addressData, List<OperatingLocationData> operatingLocationData, List<ProductData> productData) {
            StoreData = storeData;
            OrderData = orderData;
            CustomerData = customerData;
            AddressData = addressData;
            OperatingLocationData = operatingLocationData;
            ProductData = productData;
        }
    }
}

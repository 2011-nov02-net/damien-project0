using System;

using Microsoft.EntityFrameworkCore;

using StoreManagerContext = StoreManager.DataModel.StoreManagerContext;

using Xunit;
using StoreManager.Library.Database;
using StoreManager.Library.Logger;
using System.IO;
using System.Text.Json;
using StoreManager.Library.Entity;
using System.Linq;
using StoreManager.Library.Data;

namespace StoreManager.Library.Tests
{
    public class StoreManagerTests
    {
        private static readonly Random s_random = new Random();

        private static string RandomString(int length) {
            string result = "";
            for (int i = 0; i < length; i++) {
                result += (char)(s_random.Next(26) + 97);
            }
            return result;
        }

        // [Fact] public void Test() { }
        // [Theory] public void Test(int param) { }
        public StoreManagerTests() {
            // Get the connection string
            string filepath = @"C:/Users/Khypr/Desktop/store_manager_configuration.json";
            string json = File.ReadAllText(filepath);
            string connString = JsonSerializer.Deserialize<string>(json);

            // Set up storage
            IStorageRepository storage = new DatabaseStorageRepository();
            // Set up the configuration options
            IConfigurationOptions configurationOptions = new DatabaseConfigurationOptions(new FileLogger("C:/Users/Khypr/Desktop/StoreManager/store_manager.tests.log"), connString);

            // Initialize the App
            StoreManagerApplication.Initialize(storage, null, configurationOptions);
        }

        [Fact]
        public void AnyCustomerExists() {
            Assert.True(StoreManagerApplication.Any<Customer>(), "No customers were found in the database.");
        }

        [Fact]
        public void AnyVeroExists() {
            try {
                var customerIds = StoreManagerApplication.GetCustomerIdsByName("Vero");
                Assert.True(customerIds.Any(), "Nobody with the name 'Vero' was found in the database");
            } catch (Exception) {
                Assert.True(false);
            }
        }

        [Theory]
        [InlineData("Vero", "Richter", "vero.richter@arkhen.net", "123-456-7890", 0, 1984, 10, 11, null)]
        [InlineData("Izyk", "Herzel", "herzelpretzel@apollo.net", "890-567-1234", 0, 1976, 6, 7, null)]
        [InlineData("Invictus", "Valkyrius", "ivalkyrius@hyperion.co", "147-036-9258", 0, 1996, 11, 3, null)]
        public void CreateTestCustomer(string firstName, string lastName, string email, string phoneNumber, int addressId, int year, int month, int day, int? defaultStoreLocation) {
            try {
                CustomerData data = new CustomerData(firstName, lastName, email, phoneNumber, addressId, new DateTime(year, month, day), defaultStoreLocation);
                StoreManagerApplication.Create<Customer>(data);
            } catch (Exception) {
                Assert.True(false);
            }
        }

        [Theory]
        [InlineData("", "", "", "", "", "")]
        public void CreateTestAddress(string addressLine1, string addressLine2, string city, string state, string country, string zipCode) {
            try {

            } catch (Exception) {
                Assert.True(false);
            }
        }

        // [Theory]
        // public void CreateTestOrder() {
        // 
        // }
        // 
        // [Theory]
        // public void CreateTestStore() {
        // 
        // }
        // 
        // [Theory]
        // public void CreateTestOperatingLocation() {
        // 
        // }
        // 
        // [Theory]
        // public void CreateTestProduct() {
        // 
        // }
    }
}

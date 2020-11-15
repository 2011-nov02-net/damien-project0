using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StoreManager.Library;
using StoreManager.Library.Data;
using StoreManager.Library.Entity;

using CUI = ConsoleUI.ConsoleUI;

namespace StoreManager.UserInterface.ApplicationInterface
{
    public abstract class ApplicationInterfaceBase
    {
        protected readonly bool _allowTangentialPrompts;
        protected readonly Dictionary<Type, string> _typeNames;

        public abstract void Run();

        public ApplicationInterfaceBase() {
            // Prompt to see if the user wishes to have rabbit-hole prompts
            _allowTangentialPrompts = CUI.PromptForBool("Allow the prompts to detour from the original prompt?", "yes", "no");
            _typeNames = new Dictionary<Type, string>
            {
                { typeof(Customer), "customer" },
                { typeof(Store), "store" },
                { typeof(Order), "order" },
                { typeof(Address), "address" },
                { typeof(OperatingLocation), "operating location" },
                { typeof(Product), "product" }
            };
        }

        public long PromptForId<T>()
            where T : SEntity {
            return CUI.PromptRange($"Please enter the ID for the {_typeNames[typeof(T)]}", 0, StoreManagerApplication.MaxId<T>());
        }

        public IData GetData<T>()
            where T : SEntity {
            long id = PromptForId<T>();
            return StoreManagerApplication.Get<T>(id);
        }

        public IData PromptForCreateOrExist<T>(Func<IData> func) 
            where T : SEntity {
            IData result = null;
            bool noItemsCreated = StoreManagerApplication.Any<T>();
            bool createItem = true;

            if (!noItemsCreated)
                createItem = CUI.PromptForBool($"Create a new {_typeNames[typeof(T)]} or use one that is existing?", "create", "existing");

            if(noItemsCreated || createItem) {
                // create the item
                result = func();
            } else {
                // just get the id
                result = GetData<T>();
            }

            return result;
        }

        public CustomerData CreateCustomerData() {
            /* Data required:
             *  First name
             *  Last name
             *  Email
             *  Phone number
             *  Address -> CreateAddressData() | GetAddressId()
             *  Birth Date
             *  default store location?
             */
            string firstName = CUI.PromptForInput("Enter the first name", false);
            string lastName = CUI.PromptForInput("Enter the last name", false);
            string email = CUI.PromptForInput("Enter the email", false);
            string phoneNumber = CUI.PromptForInput("Enter the phone number", false);
            if (_allowTangentialPrompts) {

                long addressId = PromptForId<Address>();
            } else {
                long addressId = PromptForId<Address>();
            }
            // how to prompt for a date?
            // TODO: ConsoleUI.PromptForDate()
            // TODO: ConsoleUI.PromptForEmail()
            // TODO: ConsoleUI.PromptForPhoneNumber()
            long? defaultStoreLocationId = CUI.PromptForBool("Set a default store location?", "yes", "no")
                ? PromptForId<OperatingLocation>() : null;

            return null;
        }

        public StoreData CreateStoreData() {
            /* Data required:
             *  Name
             *  Operating Locations -> CreateOperatingLocationData() | GetOperatingLocationId()
             *  Inventory -> CreateProductData() | GetProductId()
             */
            string storeName = CUI.PromptForInput("Enter the store name", false);
            List<long> operatingLocationIds = new List<long>();
            List<long> customerIds = new List<long>();
            Dictionary<long, int> inventory = new Dictionary<long, int>();

            if(_allowTangentialPrompts) {

            } else {

            }

            return new StoreData(storeName, operatingLocationIds, customerIds, inventory);
        }

        public OrderData CreateOrderData() {
            /* Data required:
             *  Customer -> CreateCustomerData() | GetCustomerId()
             *  Operating Location -> CreateOperatingLocationData() | GetOperatingLocationId()
             *  Products Requested -> CreateProductData() | GetProductId()
             */
            return null;
        }

        public AddressData CreateAddressData() {
            /* Data required:
             *  Address Line 1
             *  Address Line 2?
             *  City
             *  State?
             *  Country
             *  Zip Code
             */
            string addressLine1 = CUI.PromptForInput("Enter address line 1", false);
            string addressLine2 = CUI.PromptForInput("Enter address line 2", false);
            string city = CUI.PromptForInput("Enter the city", false);
            string state = CUI.PromptForInput("Enter the state", true);
            state = string.IsNullOrWhiteSpace(state) ? null : state;
            string country = CUI.PromptForInput("Enter the country", false);
            string zipCode = CUI.PromptForInput("Enter the Zip Code", false);
            return new AddressData(addressLine1, addressLine2, city, state, country, zipCode);
        }

        public OperatingLocationData CreateOperatingLocationData() {
            /* Data required:
             *  Address -> CreateAddressData() | GetAddressId()
             */
            OperatingLocationData result = null;
            long addressId;

            if (_allowTangentialPrompts) {
                var tempData = PromptForCreateOrExist<OperatingLocation>(() =>
                {
                    // This is run for a new creating a new item
                    IData temp = null;



                    return temp;
                });
                result = tempData as OperatingLocationData;

                // TODO: Extrapolate this into a separate method that's generic
                bool noOperatingLocationsCreated = StoreManagerApplication.Any<OperatingLocation>();
                bool createNew = true;

                if (!noOperatingLocationsCreated)
                    createNew = CUI.PromptForBool("Create a new or use an existing Operating Location?", "yes", "no");

                if(noOperatingLocationsCreated || createNew) {
                    // Create a new one
                    

                    var data = CreateAddressData();
                    addressId = StoreManagerApplication.Create<Address>(data);
                    result = new OperatingLocationData(addressId);
                } else {
                    // Get the id
                    result = GetData<OperatingLocation>() as OperatingLocationData;
                }
            } else {
                // Get the id
                result = GetData<OperatingLocation>() as OperatingLocationData;
            }

            return result;
        }

        public ProductData CreateProductData() {
            /* Data required:
             *  Name
             *  Price
             *  Discount?
             */
            string name = CUI.PromptForInput("Enter the product name", false);
            decimal price = CUI.PromptRange("Enter the product price", 0.01M, decimal.MaxValue);
            decimal? discount = CUI.PromptForBool("Place a discount on this product?", "yes", "no") 
                ? CUI.PromptRange("Enter the discount percentage", 0, 100) : null;
            return new ProductData(name, price, discount);
        }
    }
}

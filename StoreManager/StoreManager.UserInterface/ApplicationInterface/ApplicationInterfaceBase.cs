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

        protected long PromptForId<T>()
            where T : SEntity {
            bool idExists = false;
            long id;

            do {
                id = CUI.PromptRange($"Please enter the ID for the {_typeNames[typeof(T)]}", 0, StoreManagerApplication.MaxId<T>());

                if (StoreManagerApplication.IdExists<T>(id))
                    idExists = true;

            } while (!idExists);

            return id;
        }

        protected IData GetData<T>()
            where T : SEntity {
            // Get the data associated with the id
            bool success = false;
            IData result;

            do {
                // Get the id
                long id = PromptForId<T>();
                result = StoreManagerApplication.Get<T>(id);

                if (result is not null) {
                    success = true;
                }

            } while (!success);

            return result;
        }

        protected IData PromptForCreateOrExist<T>(Func<IData> idataCreationFunction)
            where T : SEntity {
            IData result;
            bool createItem = true;

            if (!StoreManagerApplication.Any<T>())
                createItem = CUI.PromptForBool($"Create a new {_typeNames[typeof(T)]} or use one that is existing?", "create", "existing");

            if (createItem) {
                // create the item
                result = idataCreationFunction();
            } else {
                // just get the id
                result = GetData<T>();
            }

            return result;
        }

        protected void PromptForCreateOrExist<T>(Action creationAction)
            where T : SEntity {
            bool createItem = true;

            if (!StoreManagerApplication.Any<T>())
                createItem = CUI.PromptForBool($"Create a new {_typeNames[typeof(T)]} or use one that is existing?", "create", "existing");

            if (createItem) {
                // create the item
                creationAction();
            }
        }

        protected Dictionary<long, int> GetProductsWithCounts() {
            Dictionary<long, int> result = new Dictionary<long, int>();
            bool done = false;

            do {
                // Add products and the inventory they have
                // Prompt for the product
                long productId = PromptForId<Product>();
                // Get the count
                int count = CUI.PromptRange("Enter the count of said product", 0, int.MaxValue);
                result[productId] = count;

            } while (!done);

            return result;
        }

        protected CustomerData CreateCustomerData() {
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
            string email = CUI.PromptForEmail("Enter the email");
            string phoneNumber = CUI.PromptForPhoneNumber("Enter the phone number");
            long addressId = -1;
            long? defaultStoreLocationId = null;

            if (_allowTangentialPrompts) {
                PromptForCreateOrExist<Address>(() =>
                {
                    var temp = CreateAddressData();
                    addressId = StoreManagerApplication.Create<Address>(temp);
                });
            } else {
                addressId = PromptForId<Address>();
            }

            DateTime birthDate = CUI.PromptForDateTime("Enter a birth date", CUI.TimeFrame.Past, true);
            defaultStoreLocationId = CUI.PromptForBool("Set a default store location?", "yes", "no")
                ? PromptForId<OperatingLocation>() : null;

            if (addressId == -1)
                throw new Exception("addressId was -1, which should probably not have happened...");

            return new CustomerData(firstName, lastName, email, phoneNumber, addressId, birthDate, defaultStoreLocationId, new List<long>());
        }

        protected StoreData CreateStoreData() {
            /* Data required:
             *  Name
             *  Operating Locations -> CreateOperatingLocationData() | GetOperatingLocationId()
             *  Inventory -> CreateProductData() | GetProductId()
             */
            string storeName = CUI.PromptForInput("Enter the store name", false);
            List<long> operatingLocationIds = new List<long>();
            List<long> customerIds = new List<long>();
            Dictionary<long, int> inventory = new Dictionary<long, int>();

            StoreData result = null;

            if (_allowTangentialPrompts) {

                // Operating locations

                // customers
                
                // products

            } else {
                bool done = false;

                do {
                    // Add operating locations
                    // Get the operating location by id
                    long locationId = PromptForId<OperatingLocation>();
                    if (locationId == -1)
                        done = true;
                    else operatingLocationIds.Add(locationId);

                } while (!done);

                done = false;

                do {
                    // Get the customer Id
                    long customerId = PromptForId<Customer>();
                    if (customerId == -1)
                        done = true;
                    else customerIds.Add(customerId);

                } while (!done);
                inventory = GetProductsWithCounts();

                result = new StoreData(storeName, operatingLocationIds, customerIds, inventory);
            }

            return result;
        }

        protected OrderData CreateOrderData() {
            /* Data required:
             *  Customer -> CreateCustomerData() | GetCustomerId()
             *  Operating Location -> CreateOperatingLocationData() | GetOperatingLocationId()
             *  Products Requested -> CreateProductData() | GetProductId()
             */

            long customerId = -1;
            long operatingLocationId = -1;
            Dictionary<long, int> productsRequested = null;

            if (_allowTangentialPrompts) {

                // Get the customer id
                PromptForCreateOrExist<Customer>(() =>
                {
                    // Create a new Customer
                    customerId = -1;
                });

                // Get the operating location of the store
                // Start with the store?
                PromptForCreateOrExist<OperatingLocation>(() =>
                {
                    // Create an OparatingLocation
                    operatingLocationId = -1;
                });

                // Get the products the user wants
            } else {

                // Get the customer id
                customerId = PromptForId<Customer>();
                // Get the operating location of the store
                // Start with the store?
                operatingLocationId = PromptForId<OperatingLocation>();

                // Get the products the user wants
                productsRequested = GetProductsWithCounts();
            }

            return new OrderData(customerId, operatingLocationId, productsRequested);
        }

        protected AddressData CreateAddressData() {
            string addressLine1 = CUI.PromptForInput("Enter address line 1", false);
            string addressLine2 = CUI.PromptForInput("Enter address line 2", false);
            string city = CUI.PromptForInput("Enter the city", false);
            string state = CUI.PromptForInput("Enter the state", true);
            state = string.IsNullOrWhiteSpace(state) ? null : state;
            string country = CUI.PromptForInput("Enter the country", false);
            string zipCode = CUI.PromptForInput("Enter the Zip Code", false);
            return new AddressData(addressLine1, addressLine2, city, state, country, zipCode);
        }

        protected OperatingLocationData CreateOperatingLocationData() {
            OperatingLocationData result = null;
            long addressId;

            if (_allowTangentialPrompts) {
                var tempData = PromptForCreateOrExist<OperatingLocation>(() =>
                {
                    // This is run for a new creating a new item
                    IData temp = CreateAddressData();
                    addressId = StoreManagerApplication.Create<Address>(temp);
                    return new OperatingLocationData(addressId);
                });
                result = tempData as OperatingLocationData;
            } else {
                // Get the id
                result = GetData<OperatingLocation>() as OperatingLocationData;
            }

            return result;
        }

        protected ProductData CreateProductData() {
            string name = CUI.PromptForInput("Enter the product name", false);
            decimal price = CUI.PromptRange("Enter the product price", 0.01M, decimal.MaxValue);
            decimal? discount = CUI.PromptForBool("Place a discount on this product?", "yes", "no")
                ? CUI.PromptRange("Enter the discount percentage", 0, 100) : null;
            return new ProductData(name, price, discount);
        }
    }
}

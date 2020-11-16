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
            long id = -1;

            UntilItIsDone(() => {
                id = CUI.PromptRange($"Please enter the ID for the {_typeNames[typeof(T)]}", 0, StoreManagerApplication.MaxId<T>());
                return StoreManagerApplication.IdExists<T>(id);
            });

            return id;
        }

        protected IData GetData<T>()
            where T : SEntity {
            // Get the data associated with the id
            IData result = null;

            UntilItIsDone(() => {
                // Get the id
                long id = PromptForId<T>();
                result = StoreManagerApplication.Get<T>(id);

                return result is not null;
            });

            return result;
        }

        protected long PromptForCreateOrExist<T>(Func<long> creationFunction, Func<long> existingFunction)
            where T : SEntity {
            long result;
            bool createItem = true;

            if (_allowTangentialPrompts) {
                if (!StoreManagerApplication.Any<T>())
                    createItem = CUI.PromptForBool($"Create a new {_typeNames[typeof(T)]} or use one that is existing?", "create", "existing");

                if (createItem) {
                    // create the item
                    result = creationFunction();
                } else {
                    // just get the id
                    result = existingFunction();
                }
            } else {
                result = existingFunction();
            }

            return result;
        }

        protected Dictionary<long, int> PromptForProductsWithCounts() {
            Dictionary<long, int> result = new Dictionary<long, int>();

            UntilItIsDone(() => {
                // Add products and the inventory they have
                long productId = PromptForCreateOrExist<Product>(
                    () => {
                        var data = CreateProductData();
                        return StoreManagerApplication.Create<Product>(data);
                    },
                    () => PromptForId<Product>()
                );

                // Get the count
                int count = CUI.PromptRange("Enter the count of said product", 0, int.MaxValue);
                result[productId] = count;

                return !CUI.PromptForBool("Add another product?", "yes", "no");
            });

            return result;
        }

        protected CustomerData CreateCustomerData() {
            string firstName = CUI.PromptForInput("Enter the first name", false);
            string lastName = CUI.PromptForInput("Enter the last name", false);
            string email = CUI.PromptForEmail("Enter the email");
            string phoneNumber = CUI.PromptForPhoneNumber("Enter the phone number");

            long addressId = PromptForCreateOrExist<Address>(
                            () => {
                                var temp = CreateAddressData();
                                return StoreManagerApplication.Create<Address>(temp);
                            },
                            () => PromptForId<Address>()
                        );

            DateTime birthDate = CUI.PromptForDateTime("Enter a birth date", CUI.TimeFrame.Past, true);
            long? defaultStoreLocationId = CUI.PromptForBool("Set a default store location?", "yes", "no")
                ? PromptForId<OperatingLocation>() : null;

            return new CustomerData(firstName, lastName, email, phoneNumber, addressId, birthDate, defaultStoreLocationId, new List<long>());
        }

        protected static void UntilItIsDone(Func<bool> task) {
            bool done;

            do {
                done = task();
            } while (!done);
        }

        protected StoreData CreateStoreData() {
            string storeName = CUI.PromptForInput("Enter the store name", false);
            List<long> operatingLocationIds = new List<long>();
            List<long> customerIds = new List<long>();
            Dictionary<long, int> inventory;

            long locationId;
            long customerId;

            UntilItIsDone(() => {
                // Add operating locations
                locationId = PromptForCreateOrExist<OperatingLocation>(
                    () => {
                        // Create a new operating location
                        var data = CreateOperatingLocationData();
                        return StoreManagerApplication.Create<OperatingLocation>(data);
                    },
                    // Prompt for an operating location
                    () => PromptForId<OperatingLocation>()
                );

                operatingLocationIds.Add(locationId);

                return !CUI.PromptForBool("Add another operating location?", "yes", "no");
            });

            UntilItIsDone(() => {
                customerId = PromptForCreateOrExist<Customer>(
                    () => {
                        var data = CreateCustomerData();
                        return StoreManagerApplication.Create<Customer>(data);
                    },
                    () => PromptForId<Customer>()
                );

                customerIds.Add(customerId);

                return !CUI.PromptForBool("Add another customer?", "yes", "no");
            });

            inventory = PromptForProductsWithCounts();

            return new StoreData(storeName, operatingLocationIds, customerIds, inventory);
        }

        protected OrderData CreateOrderData() {
            long customerId = -1;
            long operatingLocationId = -1;
            Dictionary<long, int> productsRequested = null;

            // Get the customer id
            customerId = PromptForCreateOrExist<Customer>(
                () => {
                    // Create a new customer
                    var data = CreateCustomerData();
                    return StoreManagerApplication.Create<Customer>(data);
                },
                // Get the customer id
                () => PromptForId<Customer>()
            );

            // Get the store that owns the location
            long storeId = PromptForCreateOrExist<Store>(
                () => {
                    var data = CreateStoreData();
                    return StoreManagerApplication.Create<Store>(data);
                },
                () => PromptForId<Store>()
            );

            var store = StoreManagerApplication.Get<Store>(storeId) as StoreData;
            // Prompt for a store, then look through the locations it has
            var options = store.OperatingLocationIds.Select(id => {
                var data = StoreManagerApplication.Get<OperatingLocation>(id) as OperatingLocationData;
                var address = StoreManagerApplication.Get<Address>(data.AddressId) as AddressData;
                return $"{store.Name} - {address}";
            }).ToArray();
            // Show the available locations that can be chosen from
            int selectedOption = CUI.PromptForMenuSelection(options, false);
            operatingLocationId = store.OperatingLocationIds[selectedOption];

            // Get the products the user wants
            productsRequested = PromptForProductsWithCounts();

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
            // Get the store that owns this location
            long storeId = PromptForCreateOrExist<Store>(
                () => {
                    var data = CreateStoreData();
                    return StoreManagerApplication.Create<Store>(data);
                },
                () => PromptForId<Store>()
            );

            // Get the address of this location
            long addressId = PromptForCreateOrExist<Address>(
                () => {
                    var data = CreateAddressData();
                    return StoreManagerApplication.Create<Address>(data);
                },
                () => PromptForId<Address>()
            );

            return new OperatingLocationData(storeId, addressId);
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

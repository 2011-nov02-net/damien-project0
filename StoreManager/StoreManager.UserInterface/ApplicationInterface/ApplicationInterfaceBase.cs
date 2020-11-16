using System;
using System.Collections.Generic;
using System.Linq;

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

        protected static void UntilItIsDone(Func<bool> task) {
            bool done;

            do {
                done = task();
            } while (!done);
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

        #region Creation Methods

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

        #endregion

        #region Displaying Methods

        public void DisplayCustomer(long id, bool newLine = false, int tabIndents = 0) {
            string indentation = new string(' ', tabIndents * 2);
            var data = StoreManagerApplication.Get<Customer>(id) as CustomerData;
            var address = (StoreManagerApplication.Get<Address>(data.AddressId) as AddressData).ToString();
            var prefix = $"{(newLine ? "\n" : "")}{indentation}";
            var items = new List<string>
            {
                //   Name
                data.Name,
                //   Email
                data.Email,
                //   PhoneNumber
                data.PhoneNumber,
                //   AddressId -> Address
                address,
                //   BirthDate
                data.BirthDate.ToString()
            };
            Console.Write($"{prefix}{string.Join(prefix, items)}");
            //   DefaultStoreLocationId -> OperatingLocation&store (if exists)
            if (data.DefaultStoreLocationId.HasValue)
                DisplayOperatingLocation(data.DefaultStoreLocationId.Value);
            // ? OrderIds -> Get distinct stores the user has purchased from?
            // TODO: Decide if orders are shown in with customer data
            // Probably not, because it isn't directly *about* the customer
        }

        public void DisplayStore(long id, bool newLine = false, int tabIndents = 0) {
            string indentation = new string(' ', tabIndents * 2);
            string prefix = $"{(newLine ? "\n" : "")}{indentation}";
            var data = StoreManagerApplication.Get<Store>(id) as StoreData;
            // Name
            string storeName = data.Name;
            // Operating Locations
            // var operatingLocationAddressIds = data.OperatingLocationIds.ConvertAll(ol => (StoreManagerApplication.Get<OperatingLocation>(ol) as  OperatingLocationData).AddressId);
            // var addresses = operatingLocationAddressIds.ConvertAll(ola => StoreManagerApplication.Get<Address>(ola) as AddressData);
            // addresses.ForEach(ol => );
            Console.Write($"{prefix}{storeName}");
            data.OperatingLocationIds.ForEach(olid => DisplayOperatingLocation(olid, true, tabIndents + 1));
        }

        public void DisplayOrder(long orderId, bool newLine = false, int tabIndents = 0) {
            var data = StoreManagerApplication.Get<Order>(orderId) as OrderData;
            string customerName = StoreManagerApplication.GetName<Customer>(data.CustomerId);
            // Get the addressId and storeId from the operating location
            long storeId = (StoreManagerApplication.Get<OperatingLocation>(data.OperatingLocationId) as OperatingLocationData).StoreId;
            // get the store that owns the operating location
            string storeName = StoreManagerApplication.GetName<Store>(storeId);

            // Display the order id and display the Customer name
            Console.Write($"Order Details for Order#{orderId} from {storeName}:\n{new string(' ', (tabIndents + 1) * 2)}Customer: {customerName}");
            // Display the Operating Location
            DisplayOperatingLocation(data.OperatingLocationId, true, tabIndents + 1);
            // Display the products requested
            Console.Write($"{new string(' ', (tabIndents + 1) * 2)}Products Requested:");
            foreach (var kv in data.ProductsRequested) {
                DisplayProduct(kv.Key, kv.Value, true, tabIndents + 2);
            }
        }

        public void DisplayAddress(long id, bool newLine = false, int tabIndents = 0) {
            string indentation = new string(' ', tabIndents * 2);
            string address = (StoreManagerApplication.Get<Address>(id) as AddressData).ToString();
            Console.Write($"{(newLine ? "\n" : "")}{indentation}{address}");
        }

        public void DisplayOperatingLocation(long id, bool newLine = false, int tabIndents = 0) {
            string indentation = new string(' ', tabIndents * 2);
            // Get the store and address id from the operating location's data
            (long storeId, long addressId) = new Func<Tuple<long, long>>(() => {
                var tempData = StoreManagerApplication.Get<OperatingLocation>(id) as OperatingLocationData;
                return new Tuple<long, long>(tempData.StoreId, tempData.AddressId);
            }).Invoke();
            // Get the name of the store
            string storeName = StoreManagerApplication.GetName<Store>(storeId);
            // Get the address as a string
            Console.Write($"{(newLine ? "\n" : "")}{indentation}{storeName} - ");
            DisplayAddress(addressId);
        }

        public void DisplayProduct(long id, int count = 1, bool newLine = false, int tabIndents = 0) {
            var data = StoreManagerApplication.Get<Product>(id) as ProductData;
            string indentation = new string(' ', tabIndents * 2);
            string discount = data.Discount.HasValue ? $" ({data.Discount.Value}% off, making it {data.Price * (100 - data.Discount) / 100})" : "";
            Console.Write($"{(newLine ? "\n" : "")}{indentation}{data.Name} - ${data.Price * count} (x{count}) {discount}");
        }

        #endregion
    }
}

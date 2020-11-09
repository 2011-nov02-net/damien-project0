using ArkhManufacturing.Library;
using ArkhManufacturing.Library.Exceptions;
using ArkhManufacturing.UserInterface.Exceptions;
using ArkhManufacturing.UserInterface.Serializers;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ArkhManufacturing.UserInterface
{
    // TODO: Add comment here
    public class FranchiseManager
    {
        private readonly IDataSerializer<Franchise> _dataSerializer;
        private readonly Franchise _franchise;

        // TODO: Add comment here
        public FranchiseManager(IDataSerializer<Franchise> dataSerializer) {
            _dataSerializer = dataSerializer;

            try {
                _franchise = _dataSerializer?.Read();
            } catch (IOException) {
            }

            if (_dataSerializer == null) {
                _franchise = new Franchise();
            }
        }

        // TODO: Add comment here
        private void DisplaySuccessMessage() => Console.WriteLine("Operation completed successfully.\n");

        // TODO: Add comment here
        private Customer GetCustomerById(string message) {
            if (_franchise.Customers.Count == 0)
                throw new IndexOutOfRangeException("There are no existing customers in this franchise.");

            long customerId = ConsoleUI.PromptRange(message, -1, _franchise.Customers.Count);
            if (customerId == -1)
                throw new UserExitException();

            return _franchise.GetCustomerById(customerId);
        }

        // TODO: Add comment here
        private Store GetStoreById(string message) {
            if (_franchise.Stores.Count == 0)
                throw new IndexOutOfRangeException("There are no existing stores in this franchise.");

            long storeId = ConsoleUI.PromptRange(message, -1, _franchise.Stores.Count);
            if (storeId == -1)
                throw new UserExitException();

            return _franchise.GetStoreById(storeId);
        }

        private Product CreateProduct() {

            string productName = ConsoleUI.PromptForInput("Please enter a product name: ", false);
            double price = ConsoleUI.PromptRange("Please enter a price", 0, double.MaxValue);
            double? discount = null;

            bool addDiscount = ConsoleUI.PromptForBool("Do you wish to add a discount on the product?", "yes", "no");
            if (addDiscount)
                discount = ConsoleUI.PromptRange("Please enter the discount percentage you wish to give", 0, 100);

            var product = new Product(productName, price, discount);

            bool done = false;

            while (!done) {
                bool noStoresCreated = _franchise.Stores.Count == 0;
                if (noStoresCreated) {
                    bool createNewStore = ConsoleUI.PromptForBool("Do you wish to create a new store?", "yes", "no");
                    if (createNewStore) {
                        // Call the create new Store
                        var store = CreateStore();
                        int count = ConsoleUI.PromptRange("Please enter the number you wish to place in stock: ", 0, int.MaxValue);
                        store.AddProduct(product, count);
                    }
                } else {
                    bool addToStore = ConsoleUI.PromptForBool("Do you wish to add this item to a store or stores?", "yes", "no");
                    if (addToStore) {
                        long storeId = ConsoleUI.PromptRange("Please enter the store ID: ", 0, _franchise.Stores.Count);
                        int count = ConsoleUI.PromptRange("Please enter the number you wish to place in stock: ", 0, int.MaxValue);
                        var store = _franchise.GetStoreById(storeId);
                        store.AddProduct(product, count);
                    }
                }
            }

            return product;
        }

        // TODO: Add comment here
        private Store CreateStore() {
            int productCountThreshold = 0;
            Location location = null;

            string storeName = ConsoleUI.PromptForInput("Please enter a store name: ", false);

            bool setProductCountThreshold = ConsoleUI.PromptForBool("Do you wish to set a product count threshold?", "yes", "no");
            if (setProductCountThreshold) {
                productCountThreshold = ConsoleUI.PromptRange("Please enter a product count threshold: ", 0, int.MaxValue);
            }
            bool createNewLocation = ConsoleUI.PromptForBool("Do you wish to create a new location, or use an existing one?", "create", "existing");
            if (createNewLocation) {
                location = PromptStoreLocation();
            }

            bool addInventory = ConsoleUI.PromptForBool("Do you wish to add items to the store?", "yes", "no");
            Dictionary<long, int> inventory = new Dictionary<long, int>();
            if (addInventory) {
                bool createNewProduct = ConsoleUI.PromptForBool("Do you wish to create a new product?", "yes", "no");
                if (createNewProduct) {
                    var product = CreateProduct();
                    int count = ConsoleUI.PromptRange("Please enter the number you wish to place in stock: ", 0, int.MaxValue);
                    inventory[product.Id] = count;
                }
            }

            long storeId = _franchise.CreateStore(storeName, productCountThreshold, location, inventory);
            var store = _franchise.GetStoreById(storeId);

            return store;
        }

        // TODO: Add comment here
        private Customer CreateCustomer() {
            CustomerName customerName = PromptCustomerName();
            Location storeLocation = null;

            bool doPrompt = ConsoleUI.PromptForBool("Do you wish to add a default store location for this customer?", "yes", "no");
            if (doPrompt)
                storeLocation = PromptStoreLocation();

            return new Customer(customerName, storeLocation);
        }

        // TODO: Add comment here
        private Customer PromptForCustomer() {
            Customer customer = null;
            bool noCustomersPresent = _franchise.Customers.Count == 0;
            bool createNewCustomer = false;

            if (!noCustomersPresent)
                createNewCustomer = ConsoleUI.PromptForBool("Do you wish to create a new customer, or use an existing customer? ", "create", "existing");

            if (noCustomersPresent || createNewCustomer) {
                Console.WriteLine("No customers exist; please create one.");
                customer = CreateCustomer();
            } else {
                bool success = false;

                do {
                    try {
                        customer = GetCustomerById("Please enter the customer ID that is ordering, or -1 to quit: ");
                        success = true;
                    } catch (NonExistentIndentifiableException) {
                        Console.WriteLine($"Customer ID is invalid; please try again.");
                    }
                } while (!success);
            }

            return customer;
        }

        // TODO: Add comment here
        private Store PromptForStore(long customerId) {
            Store store = null;
            var customer = _franchise.GetCustomerById(customerId);

            if (customer.DefaultStoreLocation == null) {
                if (_franchise.Stores.Count == 0) {
                    // only allow creating along, since there are no stores
                    Console.WriteLine("No stores exist; please create one.");
                    store = CreateStore();
                } else {
                    bool createNewStore = ConsoleUI.PromptForBool("Do you wish to create a new store, or use an existing store?", "create", "existing");
                    if (createNewStore) {
                        store = CreateStore();
                    } else {
                        bool success = false;

                        do {
                            try {
                                store = GetStoreById("Please enter the store you wish to place the order to: ");
                                success = true;
                            } catch (NonExistentIndentifiableException ex) {
                                Console.WriteLine(ex.Message);
                            }
                        } while (!success);
                    }
                }
            } else {
                long storeId;
                if (ConsoleUI.PromptForBool($"Do you wish use the default store ({customer.DefaultStoreLocation})? ", "yes", "no"))
                    storeId = customer.DefaultStoreLocation.Id;
                else {
                    storeId = ConsoleUI.PromptRange($"Please enter the store you wish to place the order to, or -1 to exit: ", -1, _franchise.Customers.Count);

                    if (storeId == -1)
                        throw new UserExitException();
                }
                store = _franchise.GetStoreById(storeId);
            }

            // Check if the store has anything in stock
            while (!store.HasStock()) {
                // if not, prompt for another store
                long storeId = ConsoleUI.PromptRange($"That store does not have anything in stock; enter a different id, or -1 to return to menu: ", -1, _franchise.Customers.Count);

                if (storeId == -1)
                    throw new UserExitException();

                store = _franchise.GetStoreById(storeId);
            }

            return store;
        }

        private Dictionary<long, int> PromptForInventory(long storeId) {
            Dictionary<long, int> productsRequested = new Dictionary<long, int>();
            var store = _franchise.GetStoreById(storeId);
            bool done = false;

            do {
                string inventory = $"{string.Join(",\n", store.Inventory.Select(kv => $"{kv.Key} x{kv.Value}"))}";
                Console.WriteLine(inventory);

                long targetProduct;
                int productCount;

                // Get the product ID
                targetProduct = ConsoleUI.PromptRange(">", -1, store.Inventory.Count);
                // get the product from the product ID
                Product product = store.GetProductById(targetProduct);

                if (targetProduct == -1)
                    throw new UserExitException();

                if (product != null) {
                    // Get the count of the product
                    productCount = ConsoleUI.PromptRange("Specify a count, or -1 to stop adding products: ", -1, store.Inventory[product.Id]);

                    if (productCount == -1)
                        done = true;
                    else if (productCount != 0)
                        // Add the product
                        productsRequested[product.Id] = productCount;
                } else {
                    Console.WriteLine($"Invalid product ID specified (got '{targetProduct}'); please try again.");
                    continue;
                }

                // No products added
                if (productsRequested.Count == 0) {
                    bool tryAgain = ConsoleUI.PromptForBool("No products were entered, do you wish to retry?", "yes", "no");
                    if (tryAgain)
                        return PromptForInventory(storeId);
                    else throw new UserExitException();
                }
            } while (!done);

            return productsRequested;
        }

        // TODO: Add comment here
        private void PromptOrder() {
            try {
                // Get the customer of the order
                Customer customer = PromptForCustomer();

                // Get the store of the order
                var store = PromptForStore(customer.Id);

                // Get the current date
                DateTime date = DateTime.Now;

                // Get all the products they wish to associate with the order
                Dictionary<long, int> productsRequested = PromptForInventory(store.Id);

                // Add the Order after its creation
                store.SubmitOrder(new Order(customer, store, date, productsRequested));
                DisplaySuccessMessage();
            } catch (UserExitException) {
                Console.WriteLine("Exiting to menu...");
                throw;
            } catch (ArgumentException) {
                // Means there are no customers
                Console.WriteLine("There are no customers in the system; returning to the main menu.");
            }
        }

        // TODO: Add comment here
        private CustomerName PromptCustomerName() {
            string firstName = ConsoleUI.PromptForInput("Please enter their first name: ", false);
            string lastName = ConsoleUI.PromptForInput("Please enter their last name: ", false);
            return new CustomerName(firstName, lastName);
        }

        // TODO: Add comment here
        private Location PromptStoreLocation() {
            string planet = ConsoleUI.PromptForInput("Please enter a planet: ", false);
            string province = ConsoleUI.PromptForInput("Please enter a province: ", false);
            string city = ConsoleUI.PromptForInput("Please enter their a city: ", false);
            return new Location(planet, province, city);
        }

        // TODO: Add comment here
        private void PromptCustomer() {
            _ = CreateCustomer();
            DisplaySuccessMessage();
        }

        // TODO: Add comment here
        private void SearchCustomer() {
            string userInput = ConsoleUI.PromptForInput("Please enter the name of the customer in the form of 'last, first': ", false);
            List<Customer> customers = _franchise.GetCustomersByName(userInput);
            Console.WriteLine($"{customers.Count} results found:\n\t{string.Join(",\n\t", customers)}");
        }

        // TODO: Add comment here
        private void DisplayCustomerDetails() {
            try {
                Customer customer = GetCustomerById("Please enter the customer ID, or -1 to return to menu: ");
                Console.WriteLine($"{customer}");
            } catch (NonExistentIndentifiableException) {
                Console.WriteLine("Customer was not found.");
            }
        }

        // TODO: Add comment here
        private void DisplayStoreOrderHistory() {
            // Prompt for a store
            long storeId = ConsoleUI.PromptRange("Please enter a store id: ", 0, _franchise.Stores.Count);
            // Get its associated orders
            List<Order> orders = _franchise.GetOrdersByCustomerId(storeId);
            if (orders.Count > 0) {
                string ordersString = $"Store for Store#{storeId}:\n\t{string.Join(",\n\t", orders)}";
                Console.WriteLine(ordersString);
            } else Console.WriteLine("There are no orders for that store");
        }

        // TODO: Add comment here
        private void DisplayCustomerOrderHistory() {
            Customer customer = null;
            try {
                // Prompt for a Customer
                bool success = false;
                do {
                    try {
                        customer = GetCustomerById("Please enter a customer id, or -1 to return to menu: ");
                        success = true;
                    } catch (NonExistentIndentifiableException ex) {
                        Console.WriteLine(ex.Message);
                    }
                } while (!success);
            } catch (UserExitException) {
                Console.WriteLine("Exiting to menu...");
                return;
            }

            // Get its associated orders
            List<Order> orders = _franchise.GetOrdersByCustomerId(customer.Id);
            if (orders.Count > 0) {
                string ordersString = $"Orders for Customer#{customer.Id}:\n\t{string.Join(",\n\t", orders)}";
                Console.WriteLine(ordersString);
            } else Console.WriteLine("There are no orders for that customer.");
        }

        // TODO: Add comment here
        public async Task Run() {
            bool quit = false;

            do {
                var actions = new List<Action>
                {
                    PromptCustomer,
                    PromptOrder,
                    SearchCustomer,
                    DisplayCustomerDetails,
                    DisplayStoreOrderHistory,
                    DisplayCustomerOrderHistory,
                    Console.Clear
                };

                var options = new[]
                {
                    "Add a new customer",
                    "Place an order",
                    "Search for a customer",
                    "Display the details of an order",
                    "Display the order history for a store location",
                    "Display the order history of a customer",
                    "Clear the screen"
                };

                Console.WriteLine("Welcome to Arkh Manufacturing! What would you like to do?");
                int userInput = ConsoleUI.PromptForMenuSelection(options, true, false);

                try {
                    if (userInput == -1)
                        quit = true;
                    else actions[userInput]();
                } catch (UserExitException) {
                    quit = true;
                }

            } while (!quit);

            await Save();
        }

        // TODO: Add comment here
        private async Task Save() {
            try {
                await Task.Run(() => _dataSerializer?.Write(_franchise));
            } catch (IOException) { }
        }
    }
}

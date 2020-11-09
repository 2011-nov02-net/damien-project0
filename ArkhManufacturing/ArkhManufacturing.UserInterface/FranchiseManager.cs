using ArkhManufacturing.Library;
using ArkhManufacturing.UserInterface.Exceptions;
using ArkhManufacturing.UserInterface.Serializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ArkhManufacturing.UserInterface
{
    public class FranchiseManager
    {
        private readonly IDataSerializer<Franchise> _dataSerializer;
        private Franchise _franchise;

        public FranchiseManager(IDataSerializer<Franchise> dataSerializer) {
            _dataSerializer = dataSerializer;
            try {
                _franchise = _dataSerializer?.Read();
            } catch (IOException) {
                _franchise = new Franchise();
            }
        }

        private void DisplaySuccessMessage() => Console.WriteLine("Operation completed successfully.\n");

        private Customer GetCustomerById(string message) {
            if (_franchise.Customers.Count == 0)
                return null;

            long customerId = ConsoleUI.PromptRange(message, -1, _franchise.Customers.Count);
            if (customerId == -1)
                throw new UserExitException();

            return _franchise.GetCustomerById(customerId);
        }

        private Store GetStoreById(string message) {
            if (_franchise.Stores.Count == 0)
                return null;

            long storeId = ConsoleUI.PromptRange(message, -1, _franchise.Stores.Count);
            if (storeId == -1)
                throw new UserExitException();

            return _franchise.GetStoreById(storeId);
        }

        private Store CreateStore() {
            // int productCountThreshold, Location location, List<Order> orders, Dictionary<Product, int> inventory
            int productCountThreshold = 0;
            Location location = null;
            Dictionary<Product, int> inventory = new Dictionary<Product, int>();

            bool setProductCountThreshold = ConsoleUI.PromptForBool("Do you wish to set a product count threshold?", "yes", "no");
            if (setProductCountThreshold) {
                productCountThreshold = ConsoleUI.PromptRange("Please enter a product count threshold: ", 0, int.MaxValue);
            }

            bool createNewLocation = ConsoleUI.PromptForBool("Do you wish to create a new location, or use an existing one?", "create", "existing");
            if (createNewLocation) {
                location = PromptStoreLocation();
            }

            return new Store(productCountThreshold, location, new List<Order>(), inventory);
        }

        private Customer CreateCustomer() {
            CustomerName customerName = PromptCustomerName();
            Location storeLocation = null;

            bool doPrompt = ConsoleUI.PromptForBool("Do you wish to add a default store location for this customer: ", "yes", "no");
            if (doPrompt)
                storeLocation = PromptStoreLocation();

            return new Customer(customerName, storeLocation);
        }

        private void PromptOrder() {
            try {
                // Get the customer of the order
                Customer customer = null;
                if (_franchise.Customers.Count == 0 && ConsoleUI.PromptForBool("Do you wish to create a new customer, or use an existing customer? ", "new", "existing")) {
                    // User wishes to create a new one
                    customer = CreateCustomer();
                } else {
                    // User wishes to use an existing one
                    customer = GetCustomerById("Please enter the customer ID that is ordering, or -1 to quit: ");
                }

                while (customer == null) {
                    Console.WriteLine($"Customer ID is invalid; please try again.");
                    customer = GetCustomerById("Please enter the customer ID that is ordering, or -1 to quit: ");
                }

                // Get the store of the order
                Store store = null;
                if (customer.DefaultStoreLocation == null) {
                    bool noStoresExist = _franchise.Stores.Count == 0;
                    bool createNewStore = ConsoleUI.PromptForBool("Do you wish to create a new store, or use an existing store?", "create", "existing");
                    if (createNewStore && noStoresExist) {
                        store = CreateStore();
                        _franchise.AddStore(store);
                    } else store = GetStoreById("Please enter the store you wish to place the order to: ");
                } else {
                    long storeId;
                    if (ConsoleUI.PromptForBool($"Do you wish use the default store ({customer.DefaultStoreLocation})? ", "yes", "no"))
                        storeId = customer.DefaultStoreLocation.Id;
                    else storeId = ConsoleUI.PromptRange($"Please enter the store you wish to place the order to: ", 0, _franchise.Customers.Count);
                    store = _franchise.GetStoreById(storeId);
                }

                // Check if the store has anything in stock
                while (!store.HasStock()) {
                    // if not, prompt for another store
                    long storeId = ConsoleUI.PromptRange($"That store does not have anything in stock; enter a different id: ", 0, _franchise.Customers.Count);
                    store = _franchise.GetStoreById(storeId);
                }

                DateTime date = DateTime.Now;

                string inventory = $"{string.Join(",\n", store.Inventory.Select(kv => $"{kv.Key} x{kv.Value}"))}";
                Console.WriteLine(inventory);

                // Get all the products they wish to associate with the order
                bool done = false;
                Dictionary<Product, int> productsRequested = new Dictionary<Product, int>();

                do {
                    long targetProduct = 0;
                    int productCount = 0;

                    // Get the product ID
                    targetProduct = ConsoleUI.PromptRange(">", -1, store.Inventory.Count);
                    // get the product from the product ID
                    Product product = store.GetProductById(targetProduct);

                    if (targetProduct == -1)
                        done = true;

                    if (product != null) {
                        // Get the count of the product
                        productCount = ConsoleUI.PromptRange("Specify a count, or -1 to stop adding products: ", -1, store.Inventory[product]);

                        if (productCount == -1)
                            done = true;
                        else if (productCount != 0)
                            // Add the product
                            productsRequested.Add(product, productCount);
                    } else {
                        Console.WriteLine($"Invalid product ID specified (got '{targetProduct}'); please try again.");
                        continue;
                    }

                } while (!done);

                // No products added
                if (productsRequested.Count == 0) {
                    bool tryAgain = ConsoleUI.PromptForBool("No products were entered, do you wish to retry?", "yes", "no");
                    if (tryAgain)
                        PromptOrder();
                    else return;
                }

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

        private CustomerName PromptCustomerName() {
            string firstName = ConsoleUI.PromptForInput("Please enter their first name: ", false);
            string lastName = ConsoleUI.PromptForInput("Please enter their last name: ", false);
            return new CustomerName(firstName, lastName);
        }

        private Location PromptStoreLocation() {
            string planet = ConsoleUI.PromptForInput("Please enter a planet: ", false);
            string province = ConsoleUI.PromptForInput("Please enter a province: ", false);
            string city = ConsoleUI.PromptForInput("Please enter their a city: ", false);
            return new Location(planet, province, city);
        }

        private void PromptCustomer() {
            _franchise.AddCustomer(CreateCustomer());
            DisplaySuccessMessage();
        }

        private void SearchCustomer() {
            string userInput = ConsoleUI.PromptForInput("Please enter the name of the customer in the form of 'last, first': ", false);
            List<Customer> customers = _franchise.GetCustomersByName(userInput);
            Console.WriteLine($"{customers.Count} results found:\n\t{string.Join(",\n\t", customers)}");
        }

        private void DisplayCustomerDetails() {
            Customer customer = GetCustomerById("Please enter the customer ID, or -1 to return to menu: ");
            Console.WriteLine(customer != null ? $"{customer}" : "Customer was not found");
        }

        private void DisplayStoreOrderHistory() {
            // Prompt for a store
            long storeId = ConsoleUI.PromptRange("Please enter a store id: ", 0, _franchise.Stores.Count);
            // Get its associated orders
            List<Order> orders = _franchise.GetStoreOrderHistory(storeId);
            if (orders.Count > 0) {
                string ordersString = $"Store for Store#{storeId}:\n\t{string.Join(",\n\t", orders)}";
                Console.WriteLine(ordersString);
            } else Console.WriteLine("There are no orders for that store");
        }

        private void DisplayCustomerOrderHistory() {
            Customer customer = null;
            try {
                // Prompt for a Customer
                customer = GetCustomerById("Please enter a customer id, or -1 to return to menu: ");
                while (customer == null)
                    customer = GetCustomerById("Please enter a customer id, or -1 to return to menu: ");
            } catch (UserExitException) {
                Console.WriteLine("Exiting to menu...");
                return;
            }

            // Get its associated orders
            List<Order> orders = _franchise.GetCustomerOrderHistory(customer.Id);
            if (orders.Count > 0) {
                string ordersString = $"Orders for Customer#{customer.Id}:\n\t{string.Join(",\n\t", orders)}";
                Console.WriteLine(ordersString);
            } else Console.WriteLine("There are no orders for that customer");
        }

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

        private async Task Save() {
            try {
                await Task.Run(() => _dataSerializer?.Write(_franchise));
            } catch (IOException) { }
        }
    }
}

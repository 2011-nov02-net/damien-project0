using ArkhManufacturing.Library;
using ArkhManufacturing.Library.Exception;
using ArkhManufacturing.UserInterface.Exceptions;
using ArkhManufacturing.UserInterface.Serializers;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ArkhManufacturing.UserInterface
{
    /* 
     * This needs to be extrapolated into a manager, and a user interface
     */

    // TODO: Add comment here
    public class FranchiseManager
    {
        private readonly IDataStorage<Franchise> _dataStorage;
        private readonly Franchise _franchise;

        // TODO: Add comment here
        public FranchiseManager(IDataStorage<Franchise> dataStorage) {
            _dataStorage = dataStorage;

            try {
                _franchise = _dataStorage?.Read();
            } catch (IOException) {

            } finally {
                _franchise ??= new Franchise();
            }
        }

        private Location CreateLocation() => null;

        private Customer CreateCustomer() {
            string firstName = ConsoleUI.PromptForInput("Please enter the customer's first name: ", false);
            string lastName = ConsoleUI.PromptForInput("Please enter the customer's last name:  ", false);
            
            bool createNewLocation = ConsoleUI.PromptForBool("Do you wish to create a default store location for the customer?", "yes", "no");
            Location location = createNewLocation ? CreateLocation() : null;

            var customerId = _franchise.CreateCustomer(firstName, lastName, location);
            return _franchise.GetCustomerById(customerId);
        }

        private T PromptCreateOrRetrieveExisting<T>(List<T> itemCollection) {
            bool noItemsExist = itemCollection.Count == 0;
            bool createNewItem = true;

            // if none are found, one must be created
            if (!noItemsExist)
                createNewItem = ConsoleUI.PromptForBool("", "yes", "no");

            // if some are found, user can choose
            if (createNewItem) {
                // Create the new item
            } else {
                // Get the item by its id
            }

            return default;
        }

        private void CreateCustomerPrompt() {
            // Prompt for a customer
            var customer = CreateCustomer();
            Console.WriteLine($"Customer '{customer}' created successfully.");
        }

        private Customer GetCustomerPrompt() {
            if (_franchise.Customers.Count > 0) {
                var customerId = ConsoleUI.PromptRange("Please enter the id of the customer: ", 0, _franchise.GetMaxId<Customer>());
                return _franchise.GetCustomerById(customerId);
            } else {
                // No customers exist
                return null;
            }
        }

        private void CreateOrderPrompt() {
            // Create/Get Customer
            var customer = PromptCreateOrRetrieveExisting(_franchise.Customers);

            // Create/Get Store
            var store = PromptCreateOrRetrieveExisting(_franchise.Stores);

            // Get the Products that are being ordered
            bool done = false;
            Dictionary<long, int> productsRequested = new Dictionary<long, int>();
            do {
                done = !ConsoleUI.PromptForBool("Do you wish to add a product?", "yes", "no");
                if (!done) {
                    var storeProducts = store.Inventory.Keys.Select(id => _franchise.Products.First(p => p.Id == id)).ToList();
                    var product = PromptCreateOrRetrieveExisting(storeProducts);
                    int max = store.Inventory[product.Id];
                    int count = ConsoleUI.PromptRange($"Please enter the number of products you wish to add ({max} max):", 0, max);
                    if (count > 0)
                        productsRequested[product.Id] = count;
                    else { /* Don't add the item */ }
                }
            } while (!done);

            if (productsRequested.Count > 0) {
                // Create the order
                _franchise.CreateOrder(customer.Id, store.Id, productsRequested);
            }
        }

        private void ViewCustomerDataPrompt() {

        }

        private void DisplayOrderDetailsPrompt() {

        }

        private void DisplayStoreOrderHistoryPrompt() {

        }

        private void DisplayCustomerOrderHistorPrompt() {

        }

        // TODO: Add comment here
        public async Task Run() {
            bool quit = false;

            do {
                var actions = new List<Tuple<string, Action>>
                {
                    new Tuple<string, Action>("Create a customer", CreateCustomerPrompt),
                    new Tuple<string, Action>("Place an order", CreateOrderPrompt),
                    new Tuple<string, Action>("View customer data", ViewCustomerDataPrompt),
                    new Tuple<string, Action>("Display the details of an order", DisplayOrderDetailsPrompt),
                    new Tuple<string, Action>("Display the order history of a store", DisplayStoreOrderHistoryPrompt),
                    new Tuple<string, Action>("Display the order histpry of a customer", DisplayCustomerOrderHistorPrompt),
                    new Tuple<string, Action>("Clear Console", Console.Clear)
                };

                Console.WriteLine("Welcome to Arkh Manufacturing! What would you like to do?");
                int userInput = ConsoleUI.PromptForMenuSelection(actions.Select(t => t.Item1).ToArray(), true, false);

                try {
                    if (userInput == -1)
                        quit = true;
                    else actions[userInput].Item2();
                } catch (UserExitException) {
                    quit = true;
                }

                await Commit();

            } while (!quit);

            await Commit();
        }

        // TODO: Add comment here
        private async Task Commit() {
            try {
                await Task.Run(() => _dataStorage?.Commit(_franchise));
            } catch (IOException) { }
        }
    }
}

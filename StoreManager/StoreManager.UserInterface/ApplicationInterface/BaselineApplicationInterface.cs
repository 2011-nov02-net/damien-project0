using System;
using System.Collections.Generic;
using System.Linq;

using StoreManager.Library;
using StoreManager.Library.Data;
using StoreManager.Library.Entity;

using CUI = ConsoleUI.ConsoleUI;

namespace StoreManager.UserInterface.ApplicationInterface
{
    /// <summary>
    /// Represents a baseline interface that interacts with the StoreManager singleton
    /// </summary>
    public class BaselineApplicationInterface : ApplicationInterfaceBase
    {
        private readonly Dictionary<string, Action> _actions;

        public BaselineApplicationInterface(IStorageRepository storage = null, ISerializer serializer = null, IConfigurationOptions configurationOptions = null) :
            base(storage, serializer, configurationOptions) {

            _actions = new Dictionary<string, Action>
            {
                { "Place an Order to a store location", PlaceOrderToStoreLocation }, // <-
                { "Add a new customer", AddCustomer }, // ✔
                { "Search customers by their name", SearchCustomersByName },  // ✔
                { "Display the details of an order", DisplayOrderDetails }, // <-
                { "Display all of the orders of a store location", DisplayStoreOrderHistory }, // <-
                { "Display all of a customer's order history", DisplayCustomerOrderHistory }, // <-
                { "Clear the console", Console.Clear }
            };
        }

        private void PlaceOrderToStoreLocation() {
            var data = base.CreateOrderData();
            _ = StoreManagerApplication.Create<Order>(data);
            Console.WriteLine($"Order placed successfully.");
        }

        private void AddCustomer() {
            var data = CreateCustomerData();
            _ = StoreManagerApplication.Create<Customer>(data);
            Console.WriteLine($"Customer with name: '{data.LastName}, {data.FirstName}' created successfully.");
        }

        private void SearchCustomersByName() {
            // get the name they wish to search for
            string userInput = CUI.PromptForInput("Enter the name in the format of 'first, last'", false);
            // get a list of the results
            var customerIds = StoreManagerApplication.GetCustomerIdsByName(userInput);

            if(customerIds.Any()) {
                // display the results
                customerIds.ForEach(cid => DisplayCustomer(cid, true));
            } else {
                Console.WriteLine($"No customers found with name like '{userInput}'.");
            }
        }

        private void DisplayOrderDetails() {
            // get the order id 
            // display its data
            int orderId = PromptForId<Order>();
            // Display the order
            DisplayOrder(orderId);
        }

        private void DisplayStoreOrderHistory() {
            // Get all of the stores
            var stores = StoreManagerApplication.GetAllData<Store>()
                .Select(data => data as StoreData);
            // Get their names
            var storeNames = stores.Select(sd => sd.Name).ToArray();
            // See which one the user wishes to see
            int selectedOption = CUI.PromptForMenuSelection(storeNames, false);
            // Get the store they wish to see
            var selectedStore = stores.ElementAt(selectedOption);

            if (selectedStore.OrderIds.Any()) {
                // display all of the orders 
                selectedStore.OrderIds.ForEach(o => DisplayOrder(o));
            } else {
                Console.WriteLine($"No orders for '{selectedStore.Name}'");
            }
        }

        private void DisplayCustomerOrderHistory() {
            // get the customer they're looking for
            int customerId = PromptForId<Customer>();
            // get the orders of the customer they're looking for
            var customerOrders = StoreManagerApplication.GetOrderIdsByCustomerId(customerId);

            if(customerOrders.Any()) {
                // display all of the order details
                customerOrders.ForEach(co => DisplayOrder(co, true));
            } else {
                Console.WriteLine($"No orders for '{StoreManagerApplication.GetName<Customer>(customerId)}'");
            }
        }

        public override void Run() {
            int selectedOption;
            // Set up the console options here
            UntilItIsDone(() => {
                selectedOption = CUI.PromptForMenuSelection(_actions.Keys.ToArray(), true);
                if (selectedOption == -1)
                    return true;

                var action = _actions.Values.ElementAt(selectedOption);
                action();
                Console.WriteLine();

                return selectedOption == -1;
            });
        }
    }
}

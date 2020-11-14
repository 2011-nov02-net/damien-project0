using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CUI = ConsoleUI.ConsoleUI;

namespace StoreManager.UserInterface.ApplicationInterface
{
    /// <summary>
    /// Represents a baseline interface that interacts with the StoreManager singleton
    /// </summary>
    public class BaselineApplicationInterface : ApplicationInterfaceBase
    {
        private readonly Dictionary<string, Action> _actions;

        public BaselineApplicationInterface() {
            _actions = new Dictionary<string, Action>
            {
                { "Place an Order to a store location", PlaceOrderToStoreLocation },
                { "Add a new customer", AddCustomer },
                { "Search customers by their name", SearchCustomersByName },
                { "Display the details of an order", DisplayOrderDetails },
                { "Display all of the orders of a store location", DisplayStoreOrderHistory },
                { "Display all of a customer's order history", DisplayCustomerOrderHistory },
                { "Clear the console", Console.Clear }
            };
        }

        private void PlaceOrderToStoreLocation() {
            // Get the customer associated with the order
            // Get the store location
            // Get the items for the order

            // Submit the order
        }

        private void AddCustomer() {
            // Get the first name
            // Get the last name
            // Get the email
            // Get the phone number
            // Get the Address
            // Get the default store location

            // submit the customer
        }

        private void SearchCustomersByName() {
            // get the name they wish to search for
            // get a list of the results
            // display the results
        }

        private void DisplayOrderDetails() {
            // get the order id 
            // display its data
        }

        private void DisplayStoreOrderHistory() {
            // get the store location they're looking for
            // display all of the orders 
            // TODO: Create a display order details method
        }

        private void DisplayCustomerOrderHistory() {
            // get the customer they're looking for
            // get the orders of the customer they're looking for

            // display all of the order details
        }

        public override void Run() {

            int selectedOption;
            // Set up the console options here
            var borderSettings = new CUI.ConsoleFormattingOptions.Border();
            var consoleOptions = new CUI.ConsoleFormattingOptions(borderSettings, false);

            do {

                selectedOption = CUI.PromptForMenuSelection(_actions.Keys.ToArray(), true, consoleOptions);
                if (selectedOption == -1)
                    break;

                var action = _actions.Values.ElementAt(selectedOption);
                action();
                Console.WriteLine();

            } while (selectedOption != -1);
        }
    }
}

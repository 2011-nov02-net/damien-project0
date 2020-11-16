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
            // get a list of the results
            // display the results
        }

        private void DisplayOrderDetails() {
            // get the order id 
            // display its data
            long orderId = PromptForId<Order>();
            var data = StoreManagerApplication.Get<Order>(orderId) as OrderData;
            var customer = StoreManagerApplication.Get<Customer>(data.CustomerId) as CustomerData;
            var operatingLocation = StoreManagerApplication.Get<OperatingLocation>(data.OperatingLocationId) as OperatingLocationData;
            var address = StoreManagerApplication.Get<Address>(operatingLocation.AddressId) as AddressData;
            var store = StoreManagerApplication.Get<Store>(operatingLocation.StoreId) as StoreData;
            var productsRequested = data.ProductsRequested.Select(kv => {
                var product = StoreManagerApplication.Get<Product>(kv.Key) as ProductData;
                string discount = product.Discount.HasValue ? $" ({product.Discount.Value}% off, making it {product.Price * (100 - product.Discount) / 100})" : "";
                return $"{product.Name} - ${product.Price * kv.Value} (x{kv.Value}) {discount}";
            });
            //string product = string.Join("\n", )
            string message = $"Order Details for Order#{orderId} from {store.Name}:\n  Customer: {customer.LastName}, {customer.FirstName}\n  Operating Location: {address}\n  Products Requested:\n    {string.Join("\n    ", productsRequested)}";
            Console.WriteLine(message);
        }

        private void DisplayStoreOrderHistory() {
            // get the store location they're looking for
            var stores = StoreManagerApplication.GetAll<Store>()
                .Select(data => data as StoreData);
            var storeNames = stores.Select(sd => sd.Name).ToArray();
            int selectedOption = CUI.PromptForMenuSelection(storeNames, false);
            var selectedStore = stores.ElementAt(selectedOption);

            var orders = StoreManagerApplication.GetSome<Order>(selectedStore.OrderIds);

            // display all of the orders 
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

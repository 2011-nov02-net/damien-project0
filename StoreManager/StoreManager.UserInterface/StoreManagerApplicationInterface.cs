using System;
using System.Collections.Generic;
using System.Linq;

using StoreManager.Library;
using StoreManager.Library.Data;

// ALias ConsoleUI.ConsoleUI as CUI so it will be easier to reference, as it\
//  will be used often in this class
using CUI = ConsoleUI.ConsoleUI;

namespace StoreManager.UserInterface
{
    /// <summary>
    /// Represents a class that handles the interface with the StoreManager Singleton
    /// </summary>
    public class StoreManagerApplicationInterface
    {
        // Use a singleton pattern to manager another singleton pattern
        private StoreManagerApplicationInterface s_applicationInterface = new StoreManagerApplicationInterface();

        private StoreManagerApplicationInterface() {
            StoreManagerApplication.Initialize();
        }

        public static void Run() {
            // Set up all of the methods that will be used here

            // Set this up in a config file, or something else?
            Dictionary<string, Action> actions = new Dictionary<string, Action>();
            bool runVerbose = CUI.PromptForBool("Do you wish to run verbosely?", "yes", "no");

            if(runVerbose) {
                // Add all of the options
                actions["Manage Stores"] = () => { };
                actions["Manage Customers"] = () => { };
                actions["Manage Products"] = () => { };
                actions["Manage Addresses"] = () => { };
                actions["Manage Orders"] = () => { };
                actions["Manage Operating Locations"] = () => { };
            } else {
                // Add only the required options
            }
            actions["Clear the Console"] = Console.Clear;

            int selectedOption;
            // Set up the console options here
            var borderSettings = new CUI.ConsoleFormattingOptions.Border();
            var consoleOptions = new CUI.ConsoleFormattingOptions(borderSettings, false);

            do {

                selectedOption = CUI.PromptForMenuSelection(actions.Keys.ToArray(), true, consoleOptions);
                if (selectedOption == -1)
                    break;

                var action = actions.Values.ElementAt(selectedOption);
                action();
                Console.WriteLine();

            } while (selectedOption != -1);
        }

        #region Creation Prompts

        public CustomerData CreateCustomerPrompt() {
            return null;
        }

        public StoreData CreateStorePrompt() {
            return null;
        }

        public OrderData CreateOrderPrompt() {
            return null;
        }

        public AddressData CreateAddressPrompt() {
            return null;
        }

        public OperatingLocationData CreateOperatingLocationPrompt() {
            return null;
        }

        public ProductData CreateProductPrompt() {
            return null;
        }

        #endregion

        #region Data Prompts

        public void CustomerPrompt() {

        }

        public void StorePrompt() {
        }

        public void OrderPrompt() {
        }

        public void AddressPrompt() {
        }

        public void OperatingLocationPrompt() {
        }

        public void ProductPrompt() {
        }

        #endregion
    }
}

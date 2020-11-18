using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StoreManager.Library;
using StoreManager.Library.Data;

using CUI = ConsoleUI.ConsoleUI;

namespace StoreManager.UserInterface.ApplicationInterface
{
    /// <summary>
    /// Represents a verbose interface that interacts with the StoreManager singleton
    /// </summary>
    public class VerboseApplicationInterface : ApplicationInterfaceBase
    {
        private readonly Dictionary<string, Action> _actions;

        public VerboseApplicationInterface(IStorageRepository storage = null, ISerializer serializer = null, IConfigurationOptions configurationOptions = null) :
            base(storage, serializer, configurationOptions) {
            _actions = new Dictionary<string, Action>
            {
                { "Manage Customers", ManageCustomers },
                { "Manage Stores", ManageStores },
                { "Manage Products", ManageProducts },
                { "Manage Addresses", ManageAddresses},
                { "Manage Orders", ManageOrders },
                { "Manage Operating Locations", ManageOperatingLocations },
                { "Clear the Console", Console.Clear }
            };
        }

        #region Customer Management

        private void ManageCustomers() {
            Dictionary<string, Action> options = new Dictionary<string, Action>
            {
                { "Create a Customer", CustomerCreation },
                { "Get a Customer/Customers", CustomerRetrieval },
                { "Edit a Customer", CustomerEditing },
                { "Delete a Customer", CustomerDeletion },
            };
        }

        private void CustomerCreation() {

        }

        private void CustomerRetrieval() {

        }

        private void CustomerEditing() {

        }

        private void CustomerDeletion() {

        }

        #endregion

        #region Store Management

        private void ManageStores() {
            Dictionary<string, Action> options = new Dictionary<string, Action>
            {

            };
        }

        #endregion

        #region Product Management

        private void ManageProducts() {
            Dictionary<string, Action> options = new Dictionary<string, Action>
            {

            };
        }

        #endregion

        #region Order Management

        private void ManageOrders() {
            Dictionary<string, Action> options = new Dictionary<string, Action>
            {

            };
        }

        #endregion

        #region Address Management

        private void ManageAddresses() {
            Dictionary<string, Action> options = new Dictionary<string, Action>
            {

            };
        }

        #endregion

        #region Operating Location Management

        private void ManageOperatingLocations() {
            Dictionary<string, Action> options = new Dictionary<string, Action>
            {

            };
        }

        #endregion

        public override void Run() {
            // TODO: Possibly extrapolate this into a separate method?
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

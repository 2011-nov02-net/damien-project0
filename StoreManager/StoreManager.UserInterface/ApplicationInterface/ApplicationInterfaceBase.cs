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

        public long PromptForId<T>()
            where T : SEntity {
            // TODO: Find a way to get the typename of the SEntity being passed into here
            return CUI.PromptRange($"Please enter the ID for {_typeNames[typeof(T)]}", 0, StoreManagerApplication.MaxId<T>());
        }

        public CustomerData CreateCustomerData() {
            /* Data required:
             *  First name
             *  Last name
             *  Email
             *  Phone number
             *  Address -> CreateAddressData() | GetAddressId()
             *  Birth Date
             *  default store location?
             */
            return null;
        }

        public StoreData CreateStoreData() {
            /* Data required:
             *  Name
             *  Operating Locations -> CreateOperatingLocationData() | GetOperatingLocationId()
             *  Inventory -> CreateProductData() | GetProductId()
             */
            return null;
        }

        public OrderData CreateOrderData() {
            /* Data required:
             *  Customer -> CreateCustomerData() | GetCustomerId()
             *  Operating Location -> CreateOperatingLocationData() | GetOperatingLocationId()
             *  Products Requested -> CreateProductData() | GetProductId()
             */
            return null;
        }

        public AddressData CreateAddressData() {
            /* Data required:
             *  Address Line 1
             *  Address Line 2?
             *  City
             *  State?
             *  Country
             *  Zip Code
             */
            return null;
        }

        public OperatingLocationData CreateOperatingLocationData() {
            /* Data required:
             *  Address -> CreateAddressData() | GetAddressId()
             */
            return null;
        }

        public ProductData CreateProductData() {
            /* Data required:
             *  Name
             *  Price
             *  Discount?
             */
            return null;
        }
    }
}

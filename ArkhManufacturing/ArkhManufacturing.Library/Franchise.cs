using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArkhManufacturing.Library
{
    public class Franchise
    {
        public List<Product> ProductCatalog { get; }
        public List<Store> Stores { get; }
        public List<Customer> Customers { get; }

        public Franchise()
        {
            ProductCatalog = new List<Product>();
            Stores = new List<Store>();
            Customers = new List<Customer>();
        }

        public Franchise(List<Product> productCatalog, List<Store> stores, List<Customer> customers)
        {
            ProductCatalog = productCatalog;
            Stores = stores;
            Customers = customers;
        }

        public void AddCustomer(Customer customer)
        {
            if (Customers.Select(c => c.Name).Count(n => n == customer.Name) == 0)
                Customers.Add(customer);
        }
    }
}

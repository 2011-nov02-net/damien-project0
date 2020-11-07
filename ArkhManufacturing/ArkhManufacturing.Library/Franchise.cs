using System.Collections.Generic;
using System.Linq;

namespace ArkhManufacturing.Library
{
    public class Franchise
    {
        public List<Product> ProductCatalog { get; }
        public List<Order> Orders { get; }
        public List<Customer> Customers { get; }

        public Franchise()
        {
            ProductCatalog = new List<Product>();
            Orders = new List<Order>();
            Customers = new List<Customer>();
        }

        public Franchise(List<Product> productCatalog, List<Order> orders, List<Customer> customers)
        {
            ProductCatalog = productCatalog;
            Orders = orders;
            Customers = customers;
        }

        public void AddOrder(Order order)
        {

        }

        public void AddCustomer(Customer customer)
        {
            if (Customers.Select(c => c.Name).Count(n => n == customer.Name) == 0)
                Customers.Add(customer);
        }
    }
}

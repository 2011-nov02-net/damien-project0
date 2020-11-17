using System.Collections.Generic;
using System.Linq;

using StoreManager.Library.Data;
using StoreManager.Library.Entity;

namespace StoreManager.Library.Factory
{
    internal class CustomerFactory : IFactory<Customer>
    {
        private readonly IdGenerator _idGenerator;

        public CustomerFactory() {
            Items = new List<Customer>();
            _idGenerator = new IdGenerator(0);
        }

        public CustomerFactory(List<Customer> customers) {
            Items = customers;
            _idGenerator = new IdGenerator(customers.Max(c => c.Id));
        }

        public List<Customer> Items { get; set; }

        public int Create(IData data) {
            var customer = new Customer(_idGenerator, data as CustomerData);
            Items.Add(customer);
            return customer.Id;
        }

        public Customer Get(int id) {
            return Items.FirstOrDefault(c => c.Id == id);
        }

        public void Update(int id, IData data) {
            var customer = Get(id);

            if (customer is null)
                return;

            customer.Data = data as CustomerData;
        }

        public void Delete(int id) {
            var customer = Get(id);

            if (customer is null)
                return;

            Items.Remove(customer);
        }
    }
}

﻿using System.Collections.Generic;
using System.Linq;

using StoreManager.Library.Data;
using StoreManager.Library.Entity;

namespace StoreManager.Library.Factory
{
    internal class OrderFactory : IFactory<Order>
    {
        private readonly IdGenerator _idGenerator;

        public OrderFactory() {
            Items = new List<Order>();
            _idGenerator = new IdGenerator(0);
        }

        public OrderFactory(List<Order> orders) {
            Items = orders;
            _idGenerator = new IdGenerator(Items.Max(o => o.Id));
        }
        
        private List<Order> _orders;
        public List<Order> Items { get => _orders; set => _orders = value; }

        public long Create(IData data) {
            var order = new Order(_idGenerator, data as OrderData);
            Items.Add(order);
            return order.Id;
        }

        public Order Get(long id) {
            return Items.FirstOrDefault(o => o.Id == id);
        }

        public void Update(long id, IData data) {
            var order = Get(id);

            if (order is null)
                return;

            order.Data = data as OrderData;
        }

        public void Delete(long id) {
            var order = Get(id);

            if (order is null)
                return;

            Items.Remove(order);
        }
    }
}

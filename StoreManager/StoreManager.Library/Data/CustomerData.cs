using System;
using System.Collections.Generic;

using StoreManager.Library.Entity;

namespace StoreManager.Library.Data
{
    public class CustomerData : IData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public long AddressId { get; set; }
        public DateTime BirthDate { get; set; }
        public long DefaultStoreLocationId { get; set; }
        public List<long> OrderIds { get; set; }

        public CustomerData(string firstName, string lastName, string email, string phoneNumber, long address, DateTime birthDate, long defaultStoreLocationId, List<long> orderIds) {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            AddressId = address;
            BirthDate = birthDate;
            DefaultStoreLocationId = defaultStoreLocationId;
            OrderIds = orderIds;
        }

        public CustomerData(CustomerData data) :
            this(data.FirstName, data.LastName, data.Email, data.PhoneNumber, data.AddressId, data.BirthDate, data.DefaultStoreLocationId, data.OrderIds) {
        }
    }
}

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
        public Address Address { get; set; }
        public DateTime BirthDate { get; set; }
        public OperatingLocation DefaultStoreLocation { get; set; }
        public List<Order> Orders { get; set; }

        public CustomerData(string firstName, string lastName, string email, string phoneNumber, Address address, DateTime birthDate, OperatingLocation defaultStoreLocation, List<Order> orders) {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            BirthDate = birthDate;
            DefaultStoreLocation = defaultStoreLocation;
            Orders = orders;
        }

        public CustomerData(CustomerData data) :
            this(data.FirstName, data.LastName, data.Email, data.PhoneNumber, data.Address, data.BirthDate, data.DefaultStoreLocation, data.Orders) {
        }
    }
}

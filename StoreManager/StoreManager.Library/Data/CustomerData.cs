using System;
using System.Collections.Generic;

namespace StoreManager.Library.Data
{
    public class CustomerData : NamedData
    {
        public override string Name 
        { 
            get => $"{FirstName}, {LastName}";
            set => _ = value;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int AddressId { get; set; }
        public DateTime BirthDate { get; set; }
        public int? DefaultStoreLocationId { get; set; }
        public List<int> OrderIds { get; set; }

        public CustomerData() { }

        public CustomerData(string firstName, string lastName, string email, string phoneNumber, int address, DateTime birthDate, int? defaultStoreLocationId, List<int> orderIds) {
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

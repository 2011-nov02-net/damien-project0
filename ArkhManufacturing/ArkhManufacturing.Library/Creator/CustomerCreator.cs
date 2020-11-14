using System;

using ArkhManufacturing.Library.CreationData;
using ArkhManufacturing.Library.Creator;

namespace ArkhManufacturing.Library.Creators
{
    public class CustomerCreator : ICreator<Identifiable>
    {
        public Identifiable Create(ICreationData creationData) {
            if (creationData is CustomerCreationData) {
                var customerCreationData = creationData as CustomerCreationData;
                return new Customer(customerCreationData.FirstName, customerCreationData.LastName, customerCreationData.DefaultStoreLocation);
            } else throw new ArgumentException($"Did not get CustomerCreationData passed in; instead got {creationData.GetType()}");
        }
    }
}

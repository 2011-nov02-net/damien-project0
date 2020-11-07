using ArkhManufacturing.Library;
using ArkhManufacturing.UserInterface.Serializers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ArkhManufacturing.UserInterface
{
    public class FranchiseManager
    {
        private readonly IDataSerializer<Franchise> _dataSerializer;
        private Franchise _franchise;

        public FranchiseManager(IDataSerializer<Franchise> dataSerializer)
        {
            _dataSerializer = dataSerializer;
            try
            {
                _franchise = _dataSerializer?.Read();
            }
            catch (IOException)
            {
                _franchise = new Franchise();
            }
        }

        private void Display()
        {
            // TODO: Display the Franchise data here
        }

        private Order PromptOrder()
        {
            // TODO: Fill out PromptOrder
            throw new NotImplementedException();
        }

        private Order PromptCustomer()
        {
            // TODO: Fill out PromptCustomer
            throw new NotImplementedException();
        }

        public async Task Run()
        {
            // A brunt of the application will be here
            bool quit = false;

            do
            {
                Console.Clear();
                Display();

            } while (!quit);

            await Save();
        }

        private async Task Save()
        {
            try
            {
                await Task.Run(() => _dataSerializer?.Write(_franchise));
            }
            catch (IOException) { }
        }
    }
}

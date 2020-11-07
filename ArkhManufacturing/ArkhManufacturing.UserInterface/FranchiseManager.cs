using ArkhManufacturing.Library;
using ArkhManufacturing.UserInterface.Serializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

        public async void Run()
        {
            // A brunt of the application will be here
            bool quit = false;

            do
            {



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

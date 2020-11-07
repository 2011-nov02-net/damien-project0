using ArkhManufacturing.Library;
using ArkhManufacturing.UserInterface;
using ArkhManufacturing.UserInterface.Serializers;
using System.Threading.Tasks;

namespace ArkhManufacturing.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // On start-up, ask the user for a configuration file
            ConsoleUI.SetRetryCount(3);
            string dataFilepath = ConsoleUI.PromptForInput("Please enter a filepath where data is stored/will be stored: ", true);

            FranchiseManager franchiseManager = new FranchiseManager(new JsonDataSerializer<Franchise>(dataFilepath));

            franchiseManager.Run();
        }
    }
}

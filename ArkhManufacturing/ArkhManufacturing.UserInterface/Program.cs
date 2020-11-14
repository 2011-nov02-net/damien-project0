using ArkhManufacturing.Library;
using ArkhManufacturing.UserInterface;
using ArkhManufacturing.UserInterface.Serializers;

using System.IO;
using System.Threading.Tasks;

namespace ArkhManufacturing.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args) {
            // On start-up, ask the user for a configuration file
            ConsoleUI.SetRetryCount(3);
            bool setupDataStorage = ConsoleUI.PromptForBool("Do you wish to set up data storage?", "yes", "no");
            IDataStorage<Franchise> dataSerializer = null;

            if (setupDataStorage) {
                string[] options = { "xml", "json" };
                int fileType = ConsoleUI.PromptForMenuSelection(options, true, true);

                if (fileType == -1)
                    return;
                else if (fileType != 2) {
                    // Get the filepath where the data will be stored, if empty, data only lasts for the lifetime of the application
                    string dataFilepath = ConsoleUI.PromptForInput("Please enter a filepath where data is stored/will be stored, or press enter: ", true);

                    switch (fileType) {
                        case 0:
                            dataSerializer = new XmlDataSerializer<Franchise>(dataFilepath);
                            break;

                        case 1:
                            dataSerializer = new JsonDataSerializer<Franchise>(dataFilepath);
                            break;

                        default:
                            break;
                    }
                }
            }

            FranchiseManager franchiseManager = new FranchiseManager(dataSerializer);

            await franchiseManager.Run();
        }
    }
}

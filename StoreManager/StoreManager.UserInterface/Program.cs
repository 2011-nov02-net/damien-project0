using System;

using StoreManager.Library;
using StoreManager.Library.Data;
using StoreManager.UserInterface.ApplicationInterface;
using StoreManager.UserInterface.StorageRepository;

using CUI = ConsoleUI.ConsoleUI;

namespace StoreManager.UserInterface
{
    public class Program
    {
        static void Main(string[] _) {
            ApplicationInterfaceBase applicationInterface;

            // var options = new[] { "db", "xml", "json" };
            // int selectedOption = CUI.PromptForMenuSelection(options, true);
            IStorageRepository storageRepository = null;

            // if(selectedOption == -1) {
            //     Console.WriteLine("Exiting...");
            //     return;
            // } else if(selectedOption != 0) {
            //     // database not chosen
            //     string filepath = CUI.PromptForInput("Enter the filepath to store:", // false);
            // 
            //     switch (selectedOption) {
            //         case 1:
            //             storageRepository = new XMLStorageRepository(filepath);
            //             break;
            // 
            //         case 2:
            //             storageRepository = new JSONStorageRepository(filepath);
            //             break;
            //     }
            // } // else if database was chosen, leave storageRepository as null

            bool verbose = CUI.PromptForBool("Do you wish to run the application verbosely?", "yes", "no");

            if(verbose) {
                applicationInterface = new VerboseApplicationInterface(storageRepository);
            } else {
                applicationInterface = new BaselineApplicationInterface(storageRepository);
            }

            applicationInterface.Run();
        }
    }
}

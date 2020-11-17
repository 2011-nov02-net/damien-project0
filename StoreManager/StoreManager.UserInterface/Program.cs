using System;
using System.IO;
using System.Text.Json;

using StoreManager.Library;
using StoreManager.Library.Data;
using StoreManager.Library.Database;
using StoreManager.UserInterface.ApplicationInterface;
using StoreManager.UserInterface.StorageRepository;

using CUI = ConsoleUI.ConsoleUI;

namespace StoreManager.UserInterface
{
    public class Program
    {
        static void Main(string[] _) {
            ApplicationInterfaceBase applicationInterface;

            string filepath = @"C:/Users/Khypr/Desktop/store_manager_configuration.json";
            string json = File.ReadAllText(filepath);
            string connString = JsonSerializer.Deserialize<string>(json);
            // var options = new[] { "db", "xml", "json" };
            // int selectedOption = CUI.PromptForMenuSelection(options, true);
            IStorageRepository storageRepository = null;
            IConfigurationOptions configurationOptions = new DatabaseConfigurationOptions(connString);

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

            bool verbose = false; // CUI.PromptForBool("Do you wish to run the application verbosely?", "yes", "no");

            if(verbose) {
                applicationInterface = new VerboseApplicationInterface(storageRepository, configurationOptions);
            } else {
                applicationInterface = new BaselineApplicationInterface(storageRepository, configurationOptions);
            }

            applicationInterface.Run();
        }
    }
}

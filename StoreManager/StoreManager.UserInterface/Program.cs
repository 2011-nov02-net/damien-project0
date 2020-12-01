using System;
using System.IO;
using System.Text.Json;

using StoreManager.Library;
using StoreManager.Library.Data;
using StoreManager.Library.Database;
using StoreManager.Library.Logger;
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
            ISerializer serializer = null;
            IConfigurationOptions configurationOptions = new DatabaseConfigurationOptions(new FileLogger("C:/Users/Khypr/Desktop/store_manager.log"), connString);


            bool verbose = false; // CUI.PromptForBool("Do you wish to run the application verbosely?", "yes", "no");

            if (verbose) {
                applicationInterface = new VerboseApplicationInterface(null, serializer, configurationOptions);
            } else {
                applicationInterface = new BaselineApplicationInterface(null, serializer, configurationOptions);
            }

            applicationInterface.Run();
        }
    }
}

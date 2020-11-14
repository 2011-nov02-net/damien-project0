using System;

using StoreManager.UserInterface.ApplicationInterface;

using CUI = ConsoleUI.ConsoleUI;

namespace StoreManager.UserInterface
{
    class Program
    {
        static void Main(string[] _) {
            ApplicationInterfaceBase applicationInterface;

            bool verbose = CUI.PromptForBool("Do you wish to run the application verbosely?", "yes", "no");
            if(verbose) {
                applicationInterface = new VerboseApplicationInterface();
            } else {
                applicationInterface = new BaselineApplicationInterface();
            }

            applicationInterface.Run();
        }
    }
}

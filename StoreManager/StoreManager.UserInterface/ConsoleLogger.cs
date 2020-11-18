using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StoreManager.Library.Logger;

namespace StoreManager.UserInterface
{
    public class ConsoleLogger : ILogger
    {
        public ConsoleLogger() {
            // TODO: Spawn another Console to display output to
        }

        public void Log(string message) {
            Console.WriteLine(message);
        }
    }
}

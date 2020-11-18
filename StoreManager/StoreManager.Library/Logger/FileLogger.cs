using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreManager.Library.Logger
{
    public class FileLogger : ILogger
    {
        private readonly string _filepath;
        public FileLogger(string filepath) {
            _filepath = filepath;
        }

        public void Log(string message) {
            File.AppendAllText(_filepath, $"{message}\n\n");
        }
    }
}

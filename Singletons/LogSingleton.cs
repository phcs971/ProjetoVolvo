using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace ProjetoVolvo.Singletons {
    public class LogSingleton {
        private LogSingleton() {}

        private string FileName {
            get {
                var now = DateTime.Now;
                return $"{now.Year.ToString().PadLeft(4, '0')}-{now.Month.ToString().PadLeft(2, '0')}-{now.Day.ToString().PadLeft(2, '0')}.txt";
            }
        }
        public static LogSingleton Instance { get; } = new LogSingleton();

        public void SystemLog(string message) {
            var m = $"{DateTime.Now.ToUniversalTime()} - {message}\n\n";
            var file = $"Logs/System/{FileName}";
            if (File.Exists(file)) File.AppendAllText(file, m);
            else File.WriteAllText(file, m);
        }

        public void ErrorLog(string message) {
            var m = $"{DateTime.Now.ToUniversalTime()} - {message}\n\n";
            var file = $"Logs/Errors/{FileName}";
            if (File.Exists(file)) File.AppendAllText(file, m);
            else File.WriteAllText(file, m);
        }
    }
}
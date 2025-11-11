using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Sharp_var_10_lab_5
{
    public static class Logger
    {
        private static readonly string _path = Constants.logPath;

        public static void Init(bool overwrite)
        {
            if (overwrite && File.Exists(_path))
            {
                File.WriteAllText(_path, string.Empty); // очищаем файл
            }
        }

        public static void Log(string message, string type = "INFO")
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} |\t{type}\t|\t{message}";
            File.AppendAllText(_path, logEntry + Environment.NewLine);
        }
    }
}
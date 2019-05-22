using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestingFramework.Interfaces;
using TestingFramework.Models;

namespace TestingFramework.Extensions
{
    public class LoggingUtil
    {
        static LoggingUtil _instance { get; set; }

        readonly string _logsDirectory;
        readonly object _writerLock;


        public static void Initialize()
        {
            _instance = new LoggingUtil();                    
        }

        public LoggingUtil()
        {
            _writerLock = new object();
            _logsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");

            if (!Directory.Exists(_logsDirectory))
            {
                Directory.CreateDirectory(_logsDirectory);
            }
        }

        public static void AddEntry(string userName, string content)
        {
            var entry = new LogEntryModel
            {
                Timestamp = DateTime.Now,
                UserName = userName,
                Content = content,
            };

            _instance.WriteEntry(entry);
        }

        public static void AddException(Exception ex)
        {
            var entry = new LogEntryModel
            {
                Timestamp = DateTime.Now,
                UserName = "EXCEPTION",
                Content = ex.Message
            };

            _instance.WriteEntry(entry);
        }

        void WriteEntry(LogEntryModel entry)
        {
            var path = GetCurrentLogPath();

            var entries = GetExistingEntries(path);
            entries.Add(entry);

            lock (_writerLock)
            {
                File.WriteAllText(path, JsonConvert.SerializeObject(entries));
            }
        }

        string GetCurrentLogPath()
        {
            var day = DateTime.Now.ToString("yyyy-dd-MM");

            return Path.Combine(_logsDirectory, $"{day}.log");
        }

        List<LogEntryModel> GetExistingEntries(string path)
        {
            if (!File.Exists(path))
            {
                return new List<LogEntryModel>();
            }

            var contents = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<List<LogEntryModel>>(contents);
        }

        public static IEnumerable<ILogEntry> GetCurrentLogEntries()
        {
            var path = _instance.GetCurrentLogPath();

            var entries = _instance.GetExistingEntries(path).OrderBy(e => e.Timestamp);

            return entries.Reverse();
        }
    }
}

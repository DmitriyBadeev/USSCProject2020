using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Globalization;

namespace USSC.Services.LogServices
{
    public class EventLogService : IEventLogService
    {
        private readonly string _format = "yyyyMMdd";

        public IEnumerable<string> GetLogs(string path, DateTime date)
        {
            var dateString = date.ToString(_format);
            var fileName = "logs-" + dateString + ".txt";
            var filePath = Path.Combine(path, fileName);

            foreach (var log in File.ReadAllLines(filePath))
                yield return log;
        }

        public List<DateTime> GetAllLogDates(string path)
        {
            var files = Directory.GetFiles(path);
            var dates = new List<DateTime>();

            foreach (var file in files)
                dates.Add(GetDateFromName(file));

            return dates;
        }

        private DateTime GetDateFromName(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            var fileName = fileInfo.Name;

            var required = "logs-";
            var dateString = fileName
                .Substring(required.Length)
                .Split('.')
                .First();

            var logDate = DateTime.ParseExact(dateString, _format, CultureInfo.InvariantCulture);

            return logDate;
        }
    }
}

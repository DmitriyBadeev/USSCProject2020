using System;
using System.Collections.Generic;
using System.Text;

namespace USSC.Services.LogServices
{
    public interface IEventLogService
    {
        IEnumerable<string> GetLogs(string path, DateTime date);
        List<DateTime> GetAllLogDates(string path);
    }
}

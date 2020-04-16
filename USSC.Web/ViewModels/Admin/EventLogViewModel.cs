using System;
using System.Collections.Generic;

namespace USSC.Web.ViewModels.Admin
{
    public class EventLogViewModel
    {
        public List<DateTime> Dates { get; set; }
        public int CurrentDate { get; set; }
        public IEnumerable<string> Logs { get; set; }
    }
}

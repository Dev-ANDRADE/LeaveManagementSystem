using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeaveManagementSystem.Helpers
{
    public class GoogleCalendarEvent
    {
        public string Summary { get; set; }

        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End{ get; set; }

    }
}
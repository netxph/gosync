using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoSync
{
    public class OutlookCalendarEvent : CalendarEvent
    {

        public string ID { get; set; }
        public string SourceID { get; set; }

        public override string GetKey()
        {
            return ID;
        }

        public override string GetSourceKey()
        {
            return SourceID;
        }

    }
}

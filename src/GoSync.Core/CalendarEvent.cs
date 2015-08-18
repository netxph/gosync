using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoSync
{
    public abstract class CalendarEvent
    {
        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public string Title { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public virtual string GetKey()
        {
            return string.Format("{0}|{1}-{2}", Title, DateFrom, DateTo);
        }

        public virtual string GetSourceKey()
        {
            return string.Format("{0}|{1}-{2}", Title, DateFrom, DateTo);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoSync
{
    public interface ICalendarProvider
    {

        List<CalendarEvent> GetCalendarEvents(DateTime rangeFrom, DateTime rangeTo);
        void AddEvent(CalendarEvent calendarEvent);
    }
}

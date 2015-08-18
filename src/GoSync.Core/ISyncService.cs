using System.Collections.Generic;

namespace GoSync
{
    public interface ISyncService
    {
        void AddEvents(IEnumerable<CalendarEvent> events);
        IEnumerable<CalendarEvent> GetNewCalendarItems();
    }
}
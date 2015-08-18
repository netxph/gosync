using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoSync
{
    public class SyncService : ISyncService
    {

        private readonly ICalendarProvider _sourceProvider;
        private readonly ICalendarProvider _destinationProvider;

        public SyncService(ICalendarProvider sourceProvider, ICalendarProvider destinationProvider)
        {
            _sourceProvider = sourceProvider;
            _destinationProvider = destinationProvider;
        }

        protected ICalendarProvider SourceProvider { get { return _sourceProvider; } }
        protected ICalendarProvider DestinationProvider { get { return _destinationProvider; } }

        public  IEnumerable<CalendarEvent> GetSourceCalendarItems()
        {
            return SourceProvider.GetCalendarEvents(DateTime.Now.Date, DateTime.Now.Date.AddDays(7));
        }


        public IEnumerable<CalendarEvent> GetDestinationCalendarItems()
        {
            return DestinationProvider.GetCalendarEvents(DateTime.Now.Date, DateTime.Now.Date.AddDays(7)); 
        }

        public IEnumerable<CalendarEvent> GetNewCalendarItems()
        {
            var sourceEvents = GetSourceCalendarItems();
            var destinationEvents = GetDestinationCalendarItems();
    
            var newEvents = new List<CalendarEvent>();

            if (sourceEvents != null)
            {
                foreach (var sourceEvent in sourceEvents)
                {
                    CalendarEvent matchEvent = null;

                    if (destinationEvents != null)
                    {
                        matchEvent = destinationEvents.FirstOrDefault(e => e.GetSourceKey() == sourceEvent.GetKey());
                    }

                    if (matchEvent == null)
                    {
                        newEvents.Add(sourceEvent);
                    }
                }
            }

            return newEvents;
        }

        public void AddEvents(IEnumerable<CalendarEvent> events)
        {
            foreach (var calendarEvent in events)
            {
                DestinationProvider.AddEvent(calendarEvent);
            }
        }
    }
}

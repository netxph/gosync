using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoSync
{
    class Program
    {
        static void Main(string[] args)
        {

            var service = new SyncService(new OutlookCalendarProvider(), new GoogleCalendarProvider("marc.vitalis"));

            var events = service.GetNewCalendarItems();
            
            foreach (var calendarEvent in events)
            {
                Console.WriteLine(calendarEvent.DateFrom);
                Console.WriteLine(calendarEvent.DateTo);
                Console.WriteLine(calendarEvent.Title);
                Console.WriteLine(calendarEvent.Location);
                Console.WriteLine(calendarEvent.Description);
                Console.WriteLine(calendarEvent.GetKey());
                Console.WriteLine(calendarEvent.GetSourceKey());
                Console.WriteLine();
            }

            Console.WriteLine("New events.");
            Console.ReadLine();

            service.AddEvents(events);

            Console.WriteLine("Import done.");
            Console.ReadLine();

        }
    }
}

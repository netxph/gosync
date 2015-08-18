using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace GoSync
{
    public class OutlookCalendarProvider : ICalendarProvider
    {
        public List<CalendarEvent> GetCalendarEvents(DateTime rangeFrom, DateTime rangeTo)
        {
            var app = new Outlook.Application();
            

            Outlook.Folder calendarFolder = app.Session.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderCalendar) as Outlook.Folder;

            Outlook.Items outlookEvents = getAppointmentsInRange(calendarFolder, rangeFrom, rangeTo);

            List<CalendarEvent> calendarEvents = new List<CalendarEvent>();

            foreach (Outlook.AppointmentItem outlookEvent in outlookEvents)
            {
                var calendarEvent = new OutlookCalendarEvent()
                {
                    DateFrom = outlookEvent.Start,
                    DateTo = outlookEvent.End,
                    Description = string.Format("{0}", outlookEvent.Body),
                    Location = outlookEvent.Location,
                    Title = outlookEvent.Subject,
                    ID = outlookEvent.GlobalAppointmentID,
                    SourceID = outlookEvent.ItemProperties["SourceID"] != null ? outlookEvent.ItemProperties["SourceID"].ToString() : outlookEvent.GlobalAppointmentID
                };

                calendarEvents.Add(calendarEvent);
            }

            return calendarEvents;
            
        }

        private Outlook.Items getAppointmentsInRange(Outlook.Folder folder, DateTime startTime, DateTime endTime)
        {
            string filter = "[Start] >= '"
                + startTime.ToString("g")
                + "' AND [End] <= '"
                + endTime.ToString("g") + "'";

            try
            {
                Outlook.Items calItems = folder.Items;
                calItems.IncludeRecurrences = true;
                calItems.Sort("[Start]", Type.Missing);
                Outlook.Items restrictItems = calItems.Restrict(filter);
                if (restrictItems.Count > 0)
                {
                    return restrictItems;
                }
                else
                {
                    return null;
                }
            }
            catch { return null; }
        }


        public void AddEvent(CalendarEvent calendarEvent)
        {
            throw new NotImplementedException();
        }
    }
}

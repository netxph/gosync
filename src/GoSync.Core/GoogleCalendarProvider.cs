using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace GoSync
{
    public class GoogleCalendarProvider : ICalendarProvider
    {

        private readonly string _userId;

        public GoogleCalendarProvider(string userId)
        {
            _userId = userId;
        }

        private static string[] SCOPES = { CalendarService.Scope.Calendar };

        public List<CalendarEvent> GetCalendarEvents(DateTime rangeFrom, DateTime rangeTo)
        {
            UserCredential credential = GetCredentials();

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "GoSync"
            });

            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = rangeFrom;
            request.TimeMax = rangeTo;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 1000;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            Events events = request.Execute();

            List<CalendarEvent> calendarEvents = new List<CalendarEvent>();

            foreach (var calendarEvent in events.Items) 
            {
                var sourceId = calendarEvent.Id;

                if (calendarEvent.ExtendedProperties != null && calendarEvent.ExtendedProperties.Shared != null && calendarEvent.ExtendedProperties.Shared.ContainsKey("SourceID"))
                {
                    sourceId = calendarEvent.ExtendedProperties.Shared["SourceID"];
                }

                calendarEvents.Add(new GoogleCalendarEvent()
                {
                    DateFrom = calendarEvent.Start.DateTime.GetValueOrDefault(),
                    DateTo = calendarEvent.End.DateTime.GetValueOrDefault(),
                    Description = calendarEvent.Description,
                    Location = calendarEvent.Location,
                    Title = calendarEvent.Summary,
                    ID = calendarEvent.Id,
                    SourceID = sourceId
                });
            }

            return calendarEvents;
        }

        protected virtual UserCredential GetCredentials()
        {
            UserCredential credential;

            using (var stream = new FileStream("auth.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

                credPath = Path.Combine(credPath, ".credentials");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, SCOPES, _userId, CancellationToken.None, new FileDataStore(credPath, true)).Result;
            }
            return credential;
        }


        public void AddEvent(CalendarEvent calendarEvent)
        {
            UserCredential credential = GetCredentials();

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "GoSync"
            });

            var googleEvent = new Event();
            googleEvent.End = new EventDateTime() { DateTime =  calendarEvent.DateTo };
            googleEvent.Start = new EventDateTime() { DateTime = calendarEvent.DateFrom };
            googleEvent.Summary = calendarEvent.Title;
            googleEvent.Description = calendarEvent.Description;
            googleEvent.Location = calendarEvent.Location;

            googleEvent.ExtendedProperties = new Event.ExtendedPropertiesData();
            googleEvent.ExtendedProperties.Shared = new Dictionary<string, string>();

            googleEvent.ExtendedProperties.Shared["SourceID"] = calendarEvent.GetKey();

            var response = service.Events.Insert(googleEvent, "primary").Execute();
        }
    }
}

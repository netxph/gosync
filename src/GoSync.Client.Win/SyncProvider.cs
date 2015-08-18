using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace GoSync.Client.Win
{
    public abstract class SyncProvider
    {

        [ThreadStatic]
        private static SyncProvider _instance;
        
        protected static SyncProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DefaultSyncProvider(
                        new SyncService(new OutlookCalendarProvider(), 
                        new GoogleCalendarProvider(ConfigurationManager.AppSettings["Google.UserID"])));
                }

                return _instance;
            }
            set { _instance = value; }
        }


        protected abstract IEnumerable<CalendarEvent> StartSync();

        public static IEnumerable<CalendarEvent> Sync()
        {
            return Instance.StartSync();
        }

    }
}

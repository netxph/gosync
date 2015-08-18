using System;
using System.Collections.Generic;

namespace GoSync.Client.Win
{
    public class DefaultSyncProvider : SyncProvider
    {

        private readonly ISyncService _syncService;

        protected ISyncService SyncService {  get { return _syncService; } }

        public DefaultSyncProvider(ISyncService syncService)
        {
            _syncService = syncService;
        }

        protected override IEnumerable<CalendarEvent> StartSync()
        {
            var events = SyncService.GetNewCalendarItems();
            SyncService.AddEvents(events);

            return events;
        }
    }
}
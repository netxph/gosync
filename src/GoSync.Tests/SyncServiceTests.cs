using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using FluentAssertions;
using Moq;

namespace GoSync.Tests
{
    public class SyncServiceTests
    {

        public class GetNewCalendarItemsMethod
        {

            [Fact]
            public void WhenGetNewItems_ShouldReturnItem()
            { 
                var sourceProvider = new Mock<ICalendarProvider>();
                sourceProvider
                    .Setup(p => p.GetCalendarEvents(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(new List<CalendarEvent>() { new OutlookCalendarEvent() });

                var destinationProvider = new Mock<ICalendarProvider>();


                SyncService service = new SyncService(sourceProvider.Object, destinationProvider.Object);
                var events = service.GetNewCalendarItems();

                events.Count().Should().Be(1);
            }

        }

        public class GetDestinationCalendarItemsMethod
        {

            [Fact]
            public void WhenGetItems_ShouldReturnOneItem()
            {
                var sourceProvider = new Mock<ICalendarProvider>();

                var destinationProvider = new Mock<ICalendarProvider>();
                destinationProvider
                    .Setup(p => p.GetCalendarEvents(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(new List<CalendarEvent>()
                    {
                        new OutlookCalendarEvent()
                    });

                SyncService service = new SyncService(sourceProvider.Object, destinationProvider.Object);

                IEnumerable<CalendarEvent> calendarEvents = service.GetDestinationCalendarItems();

                calendarEvents.Count().Should().BeGreaterThan(0); 

            }

            [Fact]
            public void WhenGetItems_ShouldMatchValue()
            {

                var sourceProvider = new Mock<ICalendarProvider>();

                var destinationProvider = new Mock<ICalendarProvider>();
                destinationProvider
                    .Setup(p => p.GetCalendarEvents(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(new List<CalendarEvent>()
                    {
                        new OutlookCalendarEvent()
                        {
                            DateFrom = new DateTime(2015, 12, 1),
                            DateTo = new DateTime(2015, 12, 2),
                            Title = "Birthday Leave",
                            Location = "TBA",
                            Description = "Bring your own food"
                        }
                    });

                SyncService service = new SyncService(sourceProvider.Object, destinationProvider.Object);

                IEnumerable<CalendarEvent> calendarEvents = service.GetDestinationCalendarItems();

                var expected = new OutlookCalendarEvent()
                {
                    DateFrom = new DateTime(2015, 12, 1),
                    DateTo = new DateTime(2015, 12, 2),
                    Title = "Birthday Leave",
                    Location = "TBA",
                    Description = "Bring your own food"
                };

                var calendarEvent = calendarEvents.First();

                calendarEvent.ShouldBeEquivalentTo(expected);
            }

        }

        public class GetSourceCalendarItemsMethod
        {

            [Fact]
            public void WhenGetItems_ShouldReturnOneItem()
            {

                var destinationProvider = new Mock<ICalendarProvider>();

                var sourceProvider = new Mock<ICalendarProvider>();
                sourceProvider
                    .Setup(p => p.GetCalendarEvents(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(new List<CalendarEvent>()
                    {
                        new OutlookCalendarEvent()
                    });

                SyncService service = new SyncService(sourceProvider.Object, destinationProvider.Object);

                IEnumerable<CalendarEvent> calendarEvents = service.GetSourceCalendarItems();

                calendarEvents.Count().Should().BeGreaterThan(0); 

            }

            [Fact]
            public void WhenGetItems_ShouldMatchValue()
            {

                var destinationProvider = new Mock<ICalendarProvider>();

                var sourceProvider = new Mock<ICalendarProvider>();
                sourceProvider
                    .Setup(p => p.GetCalendarEvents(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(new List<CalendarEvent>()
                    {
                        new OutlookCalendarEvent()
                        {
                            DateFrom = new DateTime(2015, 12, 1),
                            DateTo = new DateTime(2015, 12, 2),
                            Title = "Birthday Leave",
                            Location = "TBA",
                            Description = "Bring your own food"
                        }
                    });

                SyncService service = new SyncService(sourceProvider.Object, destinationProvider.Object);

                IEnumerable<CalendarEvent> calendarEvents = service.GetSourceCalendarItems();

                var expected = new OutlookCalendarEvent()
                {
                    DateFrom = new DateTime(2015, 12, 1),
                    DateTo = new DateTime(2015, 12, 2),
                    Title = "Birthday Leave",
                    Location = "TBA",
                    Description = "Bring your own food"
                };

                var calendarEvent = calendarEvents.First();

                calendarEvent.ShouldBeEquivalentTo(expected);
            }
            
        }

    }
}

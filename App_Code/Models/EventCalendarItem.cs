using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RevistaUFO.Models
{

    /// <summary>
    /// Summary description for EventCalendarItem
    /// </summary>
    public class EventCalendarItem
    {
        public EventCalendarItem()
        {
        }

        //[{ "date": "1337594400000", "type": "meeting", "title": "Project A meeting", "description": "Lorem Ipsum dolor set", "url": "http://www.event1.com/" }]
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1);
        public long ToUnixTime(DateTime dateTime)
        {
            return (dateTime - UnixEpoch).Ticks / TimeSpan.TicksPerMillisecond;
        }

        public long date { get { return ToUnixTime(LongDate); } }

        public string type
        {
            get { return "event"; }
        }

        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public DateTime LongDate { get; set; }
    }

}

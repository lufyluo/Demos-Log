using System;
using System.Collections.Generic;
using Nest;

namespace Project.Log.Log4netAppender
{
    //[ElasticsearchType(IdProperty = "Id", Name = "LogEvent")]
    public class LogEvent
    {
        public LogEvent()
        {
            Errors = new List<EventMessage>();
        }
        //public int Id =>3;
        public string EventType
        {
            get;
            set;
        }
        public string ServerTag
        {
            get;
            set;
        }
        public string Message
        {
            get;
            set;
        }
        public string AppName
        {
            get;
            set;
        }
        public DateTime CreateTime
        {
            get;
            set;
        }
        public List<EventMessage> Errors
        {
            get;
            set;
        }

    }
}

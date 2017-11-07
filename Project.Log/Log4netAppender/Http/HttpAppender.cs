using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using log4net.Util;

namespace Project.Log.Log4netAppender.Http
{

    public class HttpAppender : log4net.Appender.AppenderSkeleton
    {
        public HttpAppender()
        {
            Timer = 5000;
            MaxRecords = 300;
        }

        #region Field
        public string Host
        {
            get;
            set;
        }

        public string ServerTag
        {
            get;
            set;
        }

        public string AppName
        {
            get;
            set;
        }

        public int MaxRecords
        {
            get;
            set;
        }

        public int Timer
        {
            get;
            set;
        }


        #endregion
        
        private System.Threading.Timer _mTimer;

        private void CreateTime()
        {
            lock (this)
            {
                if (_mTimer == null)
                    _mTimer = new System.Threading.Timer(Upload, null, Timer, Timer);
            }
        }

        private static readonly Type DeclaringType = typeof(HttpAppender);

        private readonly Queue<LogEvent> _mEvents = new Queue<LogEvent>(1000);

        private void Add(LogEvent item)
        {
            CreateTime();
            lock (_mEvents)
            {
                _mEvents.Enqueue(item);
                if (_mEvents.Count > MaxRecords)
                    Upload(null);
            }
        }

        private void Upload(object state)
        {
            List<LogEvent> items = new List<LogEvent>();
            lock (_mEvents)
            {
                while (_mEvents.Count > 0)
                {
                    items.Add(_mEvents.Dequeue());
                }
            }
            if (items.Count > 0)
            {
                OnUpload(items);
            }
        }

        private void OnUpload(object state)
        {
            try
            {
                List<LogEvent> items = (List<LogEvent>)state;
                string param = "LogData=" + Newtonsoft.Json.JsonConvert.SerializeObject(items);
                byte[] data = Encoding.UTF8.GetBytes(param);
                HttpWebRequest req =
                    (HttpWebRequest)HttpWebRequest.Create(Host);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = data.Length;
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                }
                using (WebResponse wr = req.GetResponse())
                {

                }
            }
            catch (Exception e_)
            {
                LogLog.Error(DeclaringType, e_.Message);
            }
        }



        protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        {
            try
            {
                LogEvent le = new LogEvent();
                le.CreateTime = loggingEvent.TimeStamp;
                le.EventType = loggingEvent.Level.ToString();
                le.Message = loggingEvent.RenderedMessage;
                le.AppName = AppName;
                le.ServerTag = ServerTag;
                AddError(le, loggingEvent.ExceptionObject);
                Add(le);


            }
            catch (Exception e_)
            {
                LogLog.Error(DeclaringType, e_.Message);
            }
        }

        private void AddError(LogEvent e, Exception err)
        {
            if (err != null)
            {
                e.Errors.Add(new EventMessage { Message = err.Message, StackTrace = err.StackTrace });
                err = err.InnerException;
                while (err != null)
                {
                    e.Errors.Add(new EventMessage { Message = err.Message, StackTrace = err.StackTrace });
                    err = err.InnerException;
                }
            }

        }
    }
}

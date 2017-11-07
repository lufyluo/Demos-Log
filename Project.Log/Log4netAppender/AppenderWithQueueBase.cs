using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Appender;

namespace Project.Log.Log4netAppender
{
    public abstract class AppenderWithQueueBase<T>: AppenderSkeleton
    {

        private readonly Queue<T> _mEvents = new Queue<T>(1000);

        public int Timer
        {
            get;
            set;
        }
        public int DueTime
        {
            get;
            set;
        }
        public int MaxRecords
        {
            get;
            set;
        }
        protected System.Threading.Timer _mTimer;

        protected void CreateTime()
        {
            lock (this)
            {
                if (_mTimer == null)
                    _mTimer = new System.Threading.Timer(Upload, null, DueTime, Timer);
            }
        }
        protected void Add(T item)
        {
            CreateTime();
            lock (_mEvents)
            {
                _mEvents.Enqueue(item);
                if (_mEvents.Count > MaxRecords)
                    Upload(null);
            }
        }
        protected void Upload(object state)
        {
            List<T> items = new List<T>();
            lock (_mEvents)
            {
                while (_mEvents.Count > 0)
                {
                    items.Add(_mEvents.Dequeue());
                }
            }
            if (items.Count > 0)
            {
                Flush(items);
            }
        }

        protected abstract  void Flush(List<T> state);

      
    }
}

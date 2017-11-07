using System;
using System.Collections.Generic;
using Elasticsearch.Net;
using log4net.Core;
using log4net.Util;
using Nest;

namespace Project.Log.Log4netAppender.Elasticsearch
{
    public class EsAppender: AppenderWithQueueBase<LogEvent>
    {
        private static readonly Type DeclaringType = typeof(EsAppender);
        public string ElasticsearchServer { get; set; }
        private static ElasticClient _client { get; set; }

        private ElasticClient Client
        {
            get
            {
                if (_client == null)
                {
                    lock (this)
                    {
                        if (_client == null)
                        {
                            InitElasticClient();
                        }
                    }
                }
                return _client;
            }
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            try
            {
                LogEvent le = new LogEvent();
                le.CreateTime = loggingEvent.TimeStamp;
                le.EventType = loggingEvent.Level.ToString();
                le.Message = loggingEvent.RenderedMessage;
                Add(le);
            }
            catch (Exception e_)
            {
                LogLog.Error(DeclaringType, e_.Message);
            }
        }

        protected override void Flush(List<LogEvent> state)
        {
            Client.IndexMany<LogEvent>(state,"test");
        }

        private void InitPoor()
        {
            var nodes = new Uri[]
            {
                new Uri("http://***:9200")
            };

            var pool = new StaticConnectionPool(nodes);
            var settings = new ConnectionSettings(pool);
            _client = new ElasticClient(settings);
        }

        private void InitElasticClient()
        {
            var uri = new Uri(ElasticsearchServer);

            var settings = new ConnectionSettings(uri);
            _client = new ElasticClient(settings);
        }
    }
}
